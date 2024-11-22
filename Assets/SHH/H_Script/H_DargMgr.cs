using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_DargMg : MonoBehaviour
{
    private Vector3 H_offset;
   // private bool isDragging = false;
    public Transform noticePos;
    RaycastHit hitInfo;
    Vector3 targetPos;
    //public static H_DargMg instance; 

    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        instance = this;
    //    }
    //    else Destroy(gameObject);

    //}
    private void Start()
    {
        //noticePos = transform.parent;
    }
    private void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    OnMouseDown();
        //}
        //else if(Input.GetMouseButton(0))
        //{
        //    OnMouseDrag();
        //}
        //else
        //{
        //    OnMouseUp();
        //}
    }
    //�巡�� ���� �Լ�
    //private void OnMouseDown()
    //{
    //    isDragging = true;
    //    //targetPos.z = transform.position.z;
    //}
    //private void OnMouseDrag()
    //{
    //    if (isDragging)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out hitInfo, 1 << 11))
    //        {
    //            targetPos = hitInfo.point;
    //            targetPos.z = transform.position.z;
    //            transform.position = targetPos;
    //        }
    //        //Vector3 targetPosition = targetPos;
    //        ////targetPosition.z = targetPos.z;
    //        //targetPosition = ClampToBounds(targetPosition);
    //        //transform.position = targetPosition;
    //    }

    //}

    //private void OnMouseUp()
    //{
    //    isDragging = false;
    //}
    private Vector3 ClampToBounds(Vector3 targetPosition)
    {
        if (noticePos == null)
            return targetPosition;

        // noticePos�� ��谪 ���
        Bounds bounds = GetNoticePosBounds();
        

        // X, Y .z�� ����
        targetPosition.x = Mathf.Clamp(targetPosition.x, bounds.min.x, bounds.max.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, bounds.min.y, bounds.max.y);
        targetPosition.z = 0;

        return targetPosition;



    }

    private Bounds GetNoticePosBounds()
    {
        // noticePos�� �߽ɰ� ũ�⸦ ������� ��� ����
        Renderer renderer = noticePos.GetComponent<Renderer>();
        if (renderer != null)
        {
            return renderer.bounds;
        }

        // Renderer�� ���� ���, ��ü ��� ����
        Vector3 size = new Vector3(10f, 10f, 0); // X, Y, Z ũ�� ����
        return new Bounds(noticePos.position, size);
    }
}

