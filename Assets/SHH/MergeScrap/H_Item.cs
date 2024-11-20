using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_Item : MonoBehaviour
{
    //아이템의 고유 ID
    public int id;
    public H_Slot parentSlot;

    public SpriteRenderer visualRenderer;
    public void Init(int id, H_Slot slot)
    {
        this.id = id;
        this.parentSlot = slot;
        visualRenderer.sprite = H_Utils.GetItemVisualById(id);
    }
}
