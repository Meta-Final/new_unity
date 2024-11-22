using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMgr : MonoBehaviour
{
    //InventoryManager 인스턴스에 바로 접근하기 위한 static 변수
    public static InventoryMgr inst;
    // 슬롯들
    SlotMgr[] inventorySlots;
    //Slot 컨포넌트들을 참조할때 기준점으로 사용할 부모 트랜스폼
    public Transform innerPanelTransform;
    // Item의 프리팹
    public GameObject itemPrefab;

    public Button button;
    void Start()
    {
        inst = this;
        // 모든 슬롯 참조하기
        inventorySlots = innerPanelTransform.GetComponentsInChildren<SlotMgr>();

        button.onClick.AddListener(CreateItem);
    }
    // 빈슬롯을 배열로 반환하는 함수
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

    // 신규 아이템 생성 함수
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

    // 병합 된 신규 아이템 생성 함수
    public void CreateUpgradeItem(int newNumber, Transform newParent)
    {
        var item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        item.GetComponent<Item>().SetItem(newNumber, newParent);
    }
}
