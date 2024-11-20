using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class H_Slot : MonoBehaviour
{
    //슬롯 고유ID
    public int id;
    public H_Item currentItem;
    public SlotState state = SlotState.Empty;

    // 주어진 ID로 아이템을 생성하고 슬롯에 배치
    public void CreateItem(int id)
    {
        // "Prefabs/Item" 경로에서 아이템 프리팹을 가져와서 생성
        var itemGo = (GameObject)Instantiate(Resources.Load("Prefabs/Item"));

        // 아이템을 이 슬롯의 자식으로 설정
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
                if (currentItem.id == id) // 현재 아이템과 받아온 아이템 ID가 같으면 병합 처리
                {
                    //머지
                    Debug.Log("Merged");
                }
                else
                {
                    Debug.Log("Push back"); // 병합되지 않으면 아이템을 다시 밀어 넣기 처리
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

