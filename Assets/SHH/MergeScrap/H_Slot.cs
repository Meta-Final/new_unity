using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class H_Slot : MonoBehaviour
{
    //���� ����ID
    public int id;
    public H_Item currentItem;
    public SlotState state = SlotState.Empty;

    // �־��� ID�� �������� �����ϰ� ���Կ� ��ġ
    public void CreateItem(int id)
    {
        // "Prefabs/Item" ��ο��� ������ �������� �����ͼ� ����
        var itemGo = (GameObject)Instantiate(Resources.Load("Prefabs/Item"));

        // �������� �� ������ �ڽ����� ����
        itemGo.transform.SetParent(this.transform);
        //itemGo.transform.localPosition = Vector3.zero;
        itemGo.transform.localScale = Vector3.one;

        currentItem = itemGo.GetComponent<H_Item>();
        currentItem.Init(id, this);

        ChangeStateTo(SlotState.Full);


    }

    private void ChangeStateTo(SlotState targetState)
    {
        state = targetState;
    }
    public void ItemGrabbed()
    {
        Destroy(currentItem.gameObject);
        ChangeStateTo(SlotState.Empty);
    }

    private void ReceiveItem(int id)
    {
        switch (state)
        {
            case SlotState.Empty:

                CreateItem(id);
                ChangeStateTo(SlotState.Full);
                break;

            case SlotState.Full:
                if (currentItem.id == id) // ���� �����۰� �޾ƿ� ������ ID�� ������ ���� ó��
                {
                    //����
                    Debug.Log("Merged");
                }
                else
                {
                    Debug.Log("Push back"); // ���յ��� ������ �������� �ٽ� �о� �ֱ� ó��
                }
                break;
        }
    }
}
public enum SlotState
    {
        Empty,
        Full
    }

