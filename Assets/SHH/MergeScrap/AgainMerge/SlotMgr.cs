using UnityEngine;
using UnityEngine.EventSystems;

public class SlotMgr : MonoBehaviour, IDropHandler
{
    // ���Կ� ��ġ�� ������(GameObject)
    public GameObject item
    {
        get
        {
            // ���Կ� �ڽ� ������Ʈ�� ������ �ش� ������ ��ȯ
            if (transform.childCount > 0)
                return transform.GetChild(0).gameObject;
            return null;
        }
    }

    // ��� �̺�Ʈ ó��
    public void OnDrop(PointerEventData eventData)
    {
        // ���� ������ ��� �ִٸ�
        if (!item)
        {
            // �巡�׵� �������� ���� ������ �ڽ����� ����
           //H_MergeDrag.beingDraggedItem.transform.SetParent(transform);
        }
        else
        {
            // ���� ������ �����۰� �巡�׵� ������ ����
            //H_MergeItem dragItem = H_MergeDrag.beingDraggedItem.GetComponent<H_MergeItem>();
            H_MergeItem slotItem = item.GetComponent<H_MergeItem>();

            // �� �������� ID�� ������ ����
           // if (dragItem.id == slotItem.id)
            {
                //int newId = dragItem.id + 1; // ���� �� ���ο� ID
                //Destroy(dragItem.gameObject); // �巡�׵� ������ ����
                //Destroy(slotItem.gameObject); // ������ ������ ����

                // ���յ� ���ο� ������ ����
                //InventoryMgr.inst.CreateUpgradeItem(newId, transform);
            }
            //selse
            {
                // ID�� �ٸ��� �� �������� ��ġ�� ��ȯ
               // H_MergeDrag.beingDraggedItem.transform.SetParent(transform);
                //item.transform.SetParent(H_MergeDrag.beingDraggedItem.GetComponent<H_MergeDrag>().startParent);
            }
        }
    }
}
