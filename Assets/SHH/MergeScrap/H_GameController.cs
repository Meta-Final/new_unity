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
        //���콺 ���� ��ư Ŭ���ϸ� ���
        if (Input.GetMouseButtonDown(0))
        {
            SendRayCast();
            print("���");
        }
        //���콺 ���� ��ư Ŭ���ϸ鼭 ���콺 ��ġ �̵�
        if (Input.GetMouseButton(0) && carryingItem)
        {
            OnItemSelected();
        }
        //���콺 ���� ��ư ���鼭 ������ ����
        if (Input.GetMouseButtonUp(0))
        {
            SendRayCast();
        }
        //�� ���Կ� ���� �������� ��ġ
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlaceRandomItem();
        }
    }
    void SendRayCast()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        //�����۵鳢�� �浹�ϸ�
        if (hit.collider != null)
        {
            // ������ ���� �� �ְ�
            // �������� ��� ���� ������
            var slot = hit.transform.GetComponent<H_Slot>();
            if (slot.state == SlotState.Full && carryingItem == null)
            {
                var itemGO = (GameObject)Instantiate(Resources.Load("Prefabs/ItemDummy"));
                itemGO.transform.position = slot.transform.position;
                //������ ������ ������Ʈ�� ũ�⸦ 2��� ����
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
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ�Ͽ� mergetarget�� ����
        mergetarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // z ��ǥ�� 0���� ����
        mergetarget.z = 0;
            var delta = 10 * Time.deltaTime;

            delta *= Vector3.Distance(transform.position, mergetarget);
            carryingItem.transform.position = Vector3.MoveTowards(carryingItem.transform.position, mergetarget, delta);
        }

        void OnItemMergedWithTarget(int targetSlotId)
        {
            // �־��� ���� ID�� ���� ��ü�� ã��
            var slot = GetSlotById(targetSlotId);
            
            // Ÿ�� ���Կ� �ִ� �������� ����
            Destroy(slot.currentItem.gameObject);

             // ���� ������ ID�� 1�� ���� ���ο� �������� �����Ͽ� ���Կ� ��ġ
             slot.CreateItem(carryingItem.itemid + 1);
            
            // ���� �������� �ı� (�����ϰ� ������� ��)
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
