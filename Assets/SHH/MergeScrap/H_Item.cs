using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Item : MonoBehaviour
{
    //�������� ���� ID
    public int id;
    public H_Slot parentSlot;

    public SpriteRenderer visualRenderer;
    public void Init(int id, H_Slot Slot)
    {
        this.id = id;
        this.parentSlot = Slot;
        visualRenderer.sprite = H_Utils.GetItemVisualById(id);
    }
}
