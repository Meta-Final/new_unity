using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class H_DragManager : MonoBehaviour
{
    public static H_DragManager inst;
    public bool isDragging = false;
    public GameObject postit;
    public Transform noticepos;

    public LayerMask layermask;
    RaycastHit hitInfo;
    Vector3 targetPos;

    public bool isColliding = false;

    public GameObject currentPostIt;

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(gameObject);
    }
    void Start()
    {

    }

    void Update()
    {

        OnMouseButtonDown();

        if (Input.GetMouseButton(0))
        {
            OnMouseDragging();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnMouseButtonUp();
        }

    }
    private void OnMouseButtonDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
        }
    }
    private void OnMouseDragging()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layermask))
            {
                print(hitInfo.transform.name);
                currentPostIt = hitInfo.transform.gameObject;
                targetPos = hitInfo.point;
                targetPos.z = hitInfo.transform.position.z;
                hitInfo.transform.position = targetPos;
            }

        }

    }

    private void OnMouseButtonUp()
    {
        isDragging = false;

        // Ŭ���� ����Ʈ�� Ȯ��
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, float.MaxValue, layermask))
        {
            GameObject clickedPostIt = hitInfo.transform.gameObject;

            // ����Ʈ�տ� ��� �̹����� UI�� ����
            if (clickedPostIt.GetComponent<H_MergePostIt>() != null)
            {
                Texture postItTexture = clickedPostIt.GetComponent<H_MergePostIt>().image;

                // H_NewFolder�� UI�� �̹��� ǥ��
                if (H_NewFolder.inst != null)
                {
                    H_NewFolder.inst.texs.Add(postItTexture);
                    H_NewFolder.inst.MergeContentView();
                }
            }


        }
    }
}
