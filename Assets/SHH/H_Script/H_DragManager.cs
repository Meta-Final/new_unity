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
            //print("Á¦¹ß!");
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
    }


    //public void SetFolder()
    //{
    //    if(postIts.Count >= 2)
    //    {
    //        GameObject go = Instantiate(postit, transform.position, Quaternion.identity, noticepos);
    //        go.transform.localEulerAngles = new Vector3(-180, -180, 0);
    //        for (int i = 0; i < postIts.Count; i++)
    //        {
    //            texs.Add(postIts[i].GetComponent<H_MergePostIt>().image);
    //            texs[i] = go.GetComponent<H_NewFolder>().texs[i];

    //            Destroy(postIts[i]);
    //            postIts.RemoveAt(i);
    //            texs.RemoveAt(i);
    //        }
    //    }
    //}


    //public void GetGameObjs(GameObject obj)
    //{
    //    postIts.Add(obj);
    //}
}
