using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_MergeDrag : MonoBehaviour
{
    private Vector2 mousePosition;
    private float offsetX, offsetY;
    private static bool dragging = false;

    private void OnMouseDown()
    {
        dragging = true;
        offsetX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.position.x;
        offsetY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        if (dragging)
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector2(mousePosition.x - offsetX, mousePosition.y - offsetY);
        }
    }

    private void OnMouseUp()
    {
        dragging = false;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!dragging) return;

        // 현재 오브젝트와 충돌한 오브젝트의 ID를 비교
        H_MergeItem thisItem = GetComponent<H_MergeItem>();
        H_MergeItem collisionItem = collision.GetComponent<H_MergeItem>();

        if (thisItem != null && collisionItem != null && thisItem.id == collisionItem.id)
        {
            MergeItems(collision, thisItem.id + 1);
        }
    }

    private void MergeItems(Collider2D collision, int newId)
    {
        //InventoryMgr.inst.CreateUpgradeItem(newId, transform.position); // 병합된 새로운 아이템 생성
        dragging = false;
        Destroy(collision.gameObject); // 충돌된 아이템 삭제
        Destroy(gameObject); // 현재 아이템 삭제
    }
}
