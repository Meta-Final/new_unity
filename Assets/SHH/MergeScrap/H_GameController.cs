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
        //���ҽ� �ʱ�ȭ
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
        //���콺 ���� ��ư Ŭ���ϸ� ���
        if (Input.GetMouseButtonDown(0))
        {
            HandlePointerClick();
            print("���");
        }

        // Ű���� 'V'�� ������ �� ���� ���Կ� ������ ��ġ
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaceRandomItem();
        }
     
    }
    // ���԰��� ��ȣ�ۿ� ó�� �޼���
    void HandleSlotInteraction(H_Slot slot)
    {
        // 1. ������ ��� �ְ�, �������� ��� �ִ� ���
        if (slot.state == SlotState.Empty && carryingItem != null)
        {
            // �������� ���Կ� ��ġ
            slot.CreateItem(carryingItem.itemid);
            Destroy(carryingItem.gameObject); // ��� �ִ� ������ ����
            carryingItem = null; // ���� ��� �ִ� ������ �ʱ�ȭ
        }
        // 2. ���Կ� �������� �ְ�, �������� ��� ���� ���� ���
        else if (slot.state == SlotState.Full && carryingItem == null)
        {
            // ���Կ��� �������� ����
            var itemGO = Instantiate(Resources.Load<GameObject>("Prefabs/ItemDummy"));
            itemGO.transform.position = slot.transform.position;
            itemGO.transform.localScale = Vector3.one * 2; // ������ ũ�� ����

            carryingItem = itemGO.GetComponent<H_ItemInfo>();
            carryingItem.InitDummy(slot.id, slot.currentItem.id);

            slot.ItemGrabbed(); // ���� ���� ������Ʈ
        }
        // 3. ���Կ� �������� �ְ�, ���� ��� �ִ� �������� ������ ���
        else if (slot.state == SlotState.Full && carryingItem != null)
        {
            if (slot.currentItem.id == carryingItem.itemid)
            {
                Debug.Log("Items merged!");
                OnItemMergedWithTarget(slot.id); // ���� ó��
            }
            else
            {
                Debug.Log("Merge failed!");
                OnItemCarryFail(carryingItem); // ���� ���� ó��
            }
        }
    }
    // Ŭ���� UI ��ü�� Ž���Ͽ� ���� ��ȣ�ۿ� ó��
    void HandlePointerClick()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // ���콺 ���� ��ġ ����
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results); // ���� ���콺 ��ġ���� ��� UI ��ü Ž��

        foreach (RaycastResult result in results)
        {
            var slot = result.gameObject.GetComponent<H_Slot>();
            if (slot != null)
            {
                HandleSlotInteraction(slot); // ���԰� ��ȣ�ۿ� ó��
                return;
            }
        }
    }

    // ��ӵ� �����۰� ������ ��ȣ�ۿ� ó��
    public void HandlePointerDrop(H_ItemInfo droppedItem)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // ��ӵ� ��ġ
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            var slot = result.gameObject.GetComponent<H_Slot>();
            if (slot != null)
            {
                // �� ������ ��� ������ ��ġ
                if (slot.state == SlotState.Empty)
                {
                    slot.CreateItem(droppedItem.itemid);
                    Destroy(droppedItem.gameObject); // ��ӵ� ������ ����
                }
                // �̹� �������� �ִ� ������ ��� ���� ���� ����
                else if (slot.state == SlotState.Full)
                {
                    if (slot.currentItem.id == droppedItem.itemid)
                    {
                        OnItemMergedWithTarget(slot.id);
                    }
                    else
                    {
                        OnItemCarryFail(droppedItem); // ���� ���� ó��
                    }
                }
                return;
            }
        }

        // ���� ��, �������� ���� �������� ����
        OnItemCarryFail(droppedItem);
    }

    // ���� ���� ó��
    void OnItemMergedWithTarget(int targetSlotId)
    {
        var slot = GetSlotById(targetSlotId);
        Destroy(slot.currentItem.gameObject); // ���� ������ ����
        slot.CreateItem(slot.currentItem.id + 1); // ���� �� ���ο� ������ ����
    }

    // ���� ���� ó��
    void OnItemCarryFail(H_ItemInfo failedItem)
    {
        var slot = GetSlotById(failedItem.slotid);
        slot.CreateItem(failedItem.itemid); // �������� ���� ���Կ� ����
        Destroy(failedItem.gameObject); // �ӽ÷� ��� �ִ� ������ ����
    }

    // ������ �� ���Կ� ������ ��ġ
    void PlaceRandomItem()
    {
        if (AllSlotsOccupied())
        {
            Debug.Log("No empty slot available!"); // �� ������ ������ �α� ���
            return;
        }

        // �� ������ ã�� ������ �����ϰ� �ݺ�
        var rand = UnityEngine.Random.Range(0, slots.Length);
        var slot = GetSlotById(rand);

        while (slot.state == SlotState.Full)
        {
            rand = UnityEngine.Random.Range(0, slots.Length);
            slot = GetSlotById(rand);
        }

        slot.CreateItem(0); // �� ���Կ� ������ ����
    }

    // ��� ������ ���� á���� Ȯ��
    bool AllSlotsOccupied()
    {
        foreach (var slot in slots)
        {
            if (slot.state == SlotState.Empty)
            {
                return false; // �� ������ �ϳ��� ������ false ��ȯ
            }
        }
        return true; // ��� ������ ���� á�� ��� true ��ȯ
    }

    // ���� ID�� ���� ��ü ��������
    H_Slot GetSlotById(int id)
    {
        return slotDictionary[id];
    }
}