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
        dragging = false;
        offsetX = Camera.main.ScreenToViewportPoint(Input.mousePosition).x - transform.position.x;
        offsetY = Camera.main.ScreenToViewportPoint(Input.mousePosition).y - transform.position.y;
    }

    private void OnMouseDrag()
    {
        mousePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = new Vector2(mousePosition.x - offsetX,mousePosition.y-offsetY);
    }
    private void OnMouseUp()
    {
          dragging = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //���� ������Ʈ �̸�
        string thisObjName;
        //�浹 �� ������Ʈ �̸�
        string collObjName;

        //���� ������Ʈ �̸��� _ ���� �κи� ����
        thisObjName = gameObject.name.Substring(0, name.IndexOf("_"));
        collObjName = collision.gameObject.name.Substring(0, name.IndexOf("_"));

        //���� ������Ʈ �̸��� "1"�̸�,
        //���� ������Ʈ�� �浹�� _ ���� �κ��� ������ ��� ����.
        if (dragging && thisObjName == "1" && thisObjName == collObjName) 
        {
            NewMethod(collision, "2_merge");
        }
        if (dragging && thisObjName == "2" && thisObjName == collObjName)
        {
            NewMethod(collision, "3_merge");
        }

            void NewMethod(Collider2D collision, string name)
        {
            GameObject loadPrefab = Resources.Load<GameObject>(name);
            Instantiate(loadPrefab, transform.position, Quaternion.identity);
            dragging = false;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
