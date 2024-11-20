using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            SendRayCast();
            print("잡기");
        }
        //마우스 왼쪽 버튼 클릭하면서 마우스 위치 이동
        if (Input.GetMouseButton(0) && carryingItem)
        {
            OnItemSelected();
        }
        //마우스 왼쪽 버튼 떼면서 머지템 놓기
        if (Input.GetMouseButtonUp(0))
        {
            SendRayCast();
        }
        //빈 슬롯에 랜덤 아이템을 배치
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaceRandomItem();
        }
    }
    void SendRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //아이템들끼리 충돌하면
        if (hit.collider != null)
        {
            // 슬롯이 가득 차 있고
            // 아이템을 들고 있지 않으면
            var slot = hit.transform.GetComponent<H_Slot>();
            if (slot.state == SlotState.Full && carryingItem == null)
            {
                var itemGO = (GameObject)Instantiate(Resources.Load("Prefabs/ItemDummy"));
                itemGO.transform.position = slot.transform.position;
                //생성된 아이템 오브젝트의 크기를 2배로 설정
                itemGO.transform.localScale = Vector3.one * 2;

                carryingItem = itemGO.GetComponent<H_ItemInfo>();
                carryingItem.InitDummy(slot.id, slot.currentItem.id);

                slot.ItemGrabbed();
            }
            else if (slot.state == SlotState.Empty && carryingItem != null)
            {
                slot.CreateItem(carryingItem.itemid);
                Destroy(carryingItem.gameObject);
            }

            else if (slot.state == SlotState.Full && carryingItem != null)
            {
                if (slot.currentItem.id == carryingItem.itemid)
                {
                    print("merged");
                    OnItemMergedWithTarget(slot.id);
                }
                else
                {
                    OnItemCarryFail();
                }
            }
        }
        else
        {
            if (!carryingItem)
            {
                return;
            }
            OnItemCarryFail();
        }
    }
        void OnItemSelected()
        {
        // 마우스 위치를 월드 좌표로 변환하여 mergetarget에 저장
        mergetarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // z 좌표는 0으로 설정
        mergetarget.z = 0;
            var delta = 10 * Time.deltaTime;

            delta *= Vector3.Distance(transform.position, mergetarget);
            carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, mergetarget, delta);
        }

        void OnItemMergedWithTarget(int targetSlotId)
        {
            // 주어진 슬롯 ID로 슬롯 객체를 찾음
            var slot = GetSlotById(targetSlotId);
            
            // 타겟 슬롯에 있는 아이템을 제거
            Destroy(slot.currentItem.gameObject);

             // 현재 아이템 ID에 1을 더한 새로운 아이템을 생성하여 슬롯에 배치
             slot.CreateItem(carryingItem.itemid + 1);
            
            // 현재 아이템을 파괴 (머지하고 사라져라 뿅)
            Destroy(carryingItem.gameObject);
        }

        void OnItemCarryFail()
        {
            var slot = GetSlotById(carryingItem.slotid);
            slot.CreateItem(carryingItem.itemid);
            Destroy(carryingItem.gameObject);
        }

        void PlaceRandomItem()
        {
            if (AllSlotsOccupied())
            {
                Debug.Log("No empty slot available!");
                return;
            }

            var rand = UnityEngine.Random.Range(0, slots.Length);
            var slot = GetSlotById(rand);

            while (slot.state == SlotState.Full)
            {
                rand = UnityEngine.Random.Range(0, slots.Length);
                slot = GetSlotById(rand);
            }

            slot.CreateItem(0);
        }

        bool AllSlotsOccupied()
        {
            foreach (var slot in slots)
            {
                if (slot.state == SlotState.Empty)
                {
                    //empty slot found
                    return false;
                }
            }
            //no slot empty 
            return true;
        }

        H_Slot GetSlotById(int id)
        {
            return slotDictionary[id];
        }
    }
