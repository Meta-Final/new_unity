using UnityEngine;

public class H_ItemInfo : MonoBehaviour
{
    public int slotid;
    public int itemid;

    public SpriteRenderer visualRenderer;

    public void InitDummy(int slotid, int itemid)
    {
        this.slotid = slotid;
        this.itemid = itemid;
        visualRenderer.sprite = H_Utils.GetItemVisualById(itemid);
    }

}
