using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMgr : MonoBehaviour
{
    //InventoryManager �ν��Ͻ��� �ٷ� �����ϱ� ���� static ����
    public static InventoryMgr inst;
    // ���Ե�
    SlotMgr[] inventorySlots;
    //Slot ������Ʈ���� �����Ҷ� ���������� ����� �θ� Ʈ������
    public Transform innerPanelTransform;
    // Item�� ������
    public GameObject itemPrefab;

    public Button button;
    void Start()
    {
        inst = this;
        // ��� ���� �����ϱ�
        inventorySlots = innerPanelTransform.GetComponentsInChildren<SlotMgr>();

        button.onClick.AddListener(CreateItem);
    }
    // �󽽷��� �迭�� ��ȯ�ϴ� �Լ�
    GameObject[] GetEmptyInventorySlots()
    {
        List<GameObject> emptySlots = new List<GameObject>();

        foreach (SlotMgr s in inventorySlots)
        {
            if (s.item == null)
                emptySlots.Add(s.gameObject);
        }

        if (emptySlots.Count == 0)
            return null;
        else
            return emptySlots.ToArray();
    }

    // �ű� ������ ���� �Լ�
    public void CreateItem()
    {
        GameObject[] emptySlots = GetEmptyInventorySlots();

        if (emptySlots != null)
        {
            int randomNum = Random.Range(0, emptySlots.Length);

            var item = Instantiate(itemPrefab, emptySlots[randomNum].transform.position, Quaternion.identity);
            item.GetComponent<Item>().SetItem(1, emptySlots[randomNum].transform);
        }
    }

    // ���� �� �ű� ������ ���� �Լ�
    public void CreateUpgradeItem(int newNumber, Transform newParent)
    {
        var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        item.GetComponent<Item>().SetItem(newNumber, newParent);
    }
}
