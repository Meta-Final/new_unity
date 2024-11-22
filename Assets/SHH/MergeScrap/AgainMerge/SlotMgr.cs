using UnityEngine;
using UnityEngine.EventSystems;

public class SlotMgr : MonoBehaviour, IDropHandler
{
    // 슬롯에 배치된 아이템(GameObject)
    public GameObject item
    {
        get
        {
            // 슬롯에 자식 오브젝트가 있으면 해당 아이템 반환
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            return null;
        }
    }

    // 드롭 이벤트 처리
    public void OnDrop(PointerEventData eventData)
    {
        // 현재 슬롯이 비어 있다면
        if (!item)
        {
            // 드래그된 아이템을 현재 슬롯의 자식으로 설정
           //H_MergeDrag.beingDraggedItem.transform.SetParent(transform);
        }
        else
        {
            // 현재 슬롯의 아이템과 드래그된 아이템 참조
            //H_MergeItem dragItem = H_MergeDrag.beingDraggedItem.GetComponent<H_MergeItem>();
            H_MergeItem slotItem = item.GetComponent<H_MergeItem>();

            // 두 아이템의 ID가 같으면 병합
           // if (dragItem.id == slotItem.id)
            {
                //int newId = dragItem.id + 1; // 병합 후 새로운 ID
                //Destroy(dragItem.gameObject); // 드래그된 아이템 삭제
                //Destroy(slotItem.gameObject); // 슬롯의 아이템 삭제

                // 병합된 새로운 아이템 생성
                //InventoryMgr.inst.CreateUpgradeItem(newId, transform);
            }
            //selse
            {
                // ID가 다르면 두 아이템의 위치만 교환
               // H_MergeDrag.beingDraggedItem.transform.SetParent(transform);
                //item.transform.SetParent(H_MergeDrag.beingDraggedItem.GetComponent<H_MergeDrag>().startParent);
            }
        }
    }
}
