using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_DragManager : MonoBehaviour
{
    public static H_DragManager inst;
    private bool isDragging = false;
    RaycastHit hitInfo;
    Vector3 targetPos;

    private void Awake()
    {
        if (inst != null) inst = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        OnMouseButtonDown();
        
        if (Input.GetMouseButton(0) && isDragging)
        {
            OnMouseDragging();
        }
        else
        {
            OnMouseButtonUp();
        }
    }
    private void OnMouseButtonDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isDragging = true;  
        }
        print("Á¦¹ß!");
        //targetPos.z = transform.position.z;
    }
    private void OnMouseDragging()
    {
        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 1 << 11))
            {
                print(hitInfo.transform.name);
                targetPos = hitInfo.point;
                targetPos.z = hitInfo.transform.position.z;
                hitInfo.transform.position = targetPos;
            }
            //Vector3 targetPosition = targetPos;
            ////targetPosition.z = targetPos.z;
            //targetPosition = ClampToBounds(targetPosition);
            //transform.position = targetPosition;
        }

    }

    private void OnMouseButtonUp()
    {
        isDragging = false;
    }

}
