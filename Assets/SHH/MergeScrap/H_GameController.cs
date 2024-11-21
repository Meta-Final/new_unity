using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class H_GameController : MonoBehaviour
{
    public static H_GameController instance;

    public H_Slot[] slots;

    private Vector3 mergetarget;
    private H_ItemInfo carryingItem;

    private Dictionary<int, H_Slot> slotDictionary;

    private void Awake() 
    {
        instance = this;
        //리소스 초기화
        H_Utils.InitResources();
    }

    private void Start()
    {
        slotDictionary = new Dictionary<int, H_Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].id = i;
            slotDictionary.Add(i, slots[i]);
        }
    }
    private void Update()
    {
        //마우스 왼쪽 버튼 클릭하면 잡기
        if (Input.GetMouseButtonDown(0))
        {
            HandlePointerClick();
            print("잡기");
        }

        // 키보드 'V'를 눌렀을 때 랜덤 슬롯에 아이템 배치
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaceRandomItem();
        }
     
    }
    // 슬롯과의 상호작용 처리 메서드
    void HandleSlotInteraction(H_Slot slot)
    {
        // 1. 슬롯이 비어 있고, 아이템을 들고 있는 경우
        if (slot.state == SlotState.Empty && carryingItem != null)
        {
            // 아이템을 슬롯에 배치
            slot.CreateItem(carryingItem.itemid);
            Destroy(carryingItem.gameObject); // 들고 있던 아이템 제거
            carryingItem = null; // 현재 들고 있는 아이템 초기화
        }
        // 2. 슬롯에 아이템이 있고, 아이템을 들고 있지 않은 경우
        else if (slot.state == SlotState.Full && carryingItem == null)
        {
            // 슬롯에서 아이템을 집기
            var itemGO = Instantiate(Resources.Load<GameObject>("Prefabs/ItemDummy"));
            itemGO.transform.position = slot.transform.position;
            itemGO.transform.localScale = Vector3.one * 2; // 아이템 크기 조정

            carryingItem = itemGO.GetComponent<H_ItemInfo>();
            carryingItem.InitDummy(slot.id, slot.currentItem.id);

            slot.ItemGrabbed(); // 슬롯 상태 업데이트
        }
        // 3. 슬롯에 아이템이 있고, 현재 들고 있는 아이템이 동일한 경우
        else if (slot.state == SlotState.Full && carryingItem != null)
        {
            if (slot.currentItem.id == carryingItem.itemid)
            {
                Debug.Log("Items merged!");
                OnItemMergedWithTarget(slot.id); // 병합 처리
            }
            else
            {
                Debug.Log("Merge failed!");
                OnItemCarryFail(carryingItem); // 병합 실패 처리
            }
        }
    }
    // 클릭한 UI 객체를 탐지하여 슬롯 상호작용 처리
    void HandlePointerClick()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // 마우스 현재 위치 설정
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results); // 현재 마우스 위치에서 모든 UI 객체 탐지

        foreach (RaycastResult result in results)
        {
            var slot = result.gameObject.GetComponent<H_Slot>();
            if (slot != null)
            {
                HandleSlotInteraction(slot); // 슬롯과 상호작용 처리
                return;
            }
        }
    }

    // 드롭된 아이템과 슬롯의 상호작용 처리
    public void HandlePointerDrop(H_ItemInfo droppedItem)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // 드롭된 위치
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            var slot = result.gameObject.GetComponent<H_Slot>();
            if (slot != null)
            {
                // 빈 슬롯일 경우 아이템 배치
                if (slot.state == SlotState.Empty)
                {
                    slot.CreateItem(droppedItem.itemid);
                    Destroy(droppedItem.gameObject); // 드롭된 아이템 삭제
                }
                // 이미 아이템이 있는 슬롯일 경우 병합 로직 실행
                else if (slot.state == SlotState.Full)
                {
                    if (slot.currentItem.id == droppedItem.itemid)
                    {
                        OnItemMergedWithTarget(slot.id);
                    }
                    else
                    {
                        OnItemCarryFail(droppedItem); // 병합 실패 처리
                    }
                }
                return;
            }
        }

        // 실패 시, 아이템을 원래 슬롯으로 복귀
        OnItemCarryFail(droppedItem);
    }

    // 병합 성공 처리
    void OnItemMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.currentItem.gameObject); // 기존 아이템 제거
        slot.CreateItem(slot.currentItem.id + 1); // 병합 후 새로운 아이템 생성
    }

    // 병합 실패 처리
    void OnItemCarryFail(H_ItemInfo failedItem)
    {
        var slot = GetSlotById(failedItem.slotid);
        slot.CreateItem(failedItem.itemid); // 아이템을 원래 슬롯에 복구
        Destroy(failedItem.gameObject); // 임시로 들고 있던 아이템 제거
    }

    // 랜덤한 빈 슬롯에 아이템 배치
    void PlaceRandomItem()
    {
        if (AllSlotsOccupied())
        {
            Debug.Log("No empty slot available!"); // 빈 슬롯이 없으면 로그 출력
            return;
        }

        // 빈 슬롯을 찾을 때까지 랜덤하게 반복
        var rand = UnityEngine.Random.Range(0, slots.Length);
        var slot = GetSlotById(rand);

        while (slot.state == SlotState.Full)
        {
            rand = UnityEngine.Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }

        slot.CreateItem(0); // 빈 슬롯에 아이템 생성
    }

    // 모든 슬롯이 가득 찼는지 확인
    bool AllSlotsOccupied()
    {
        foreach (var slot in slots)
        {
            if (slot.state == SlotState.Empty)
            {
                return false; // 빈 슬롯이 하나라도 있으면 false 반환
            }
        }
        return true; // 모든 슬롯이 가득 찼을 경우 true 반환
    }

    // 슬롯 ID로 슬롯 객체 가져오기
    H_Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}