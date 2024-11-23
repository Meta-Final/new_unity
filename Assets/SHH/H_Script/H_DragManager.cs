using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H_DragManager : MonoBehaviour
{
    public static H_DragManager inst;
    public bool isDragging = false;
    public LayerMask layermask;
    RaycastHit hitInfo;
    Vector3 targetPos;

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        OnMouseButtonDown();
        
        if (Input.GetMouseButton(0))
        {
            OnMouseDragging();
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            OnMouseButtonUp();
        }
    }
    private void OnMouseButtonDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isDragging = true;  
            print("Á¦¹ß!");
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
                targetPos = hitInfo.point;
                targetPos.z = hitInfo.transform.position.z;
                hitInfo.transform.position = targetPos;
            }
          
        }

    }

    private void OnMouseButtonUp()
    {
        isDragging = false;
    }

}
