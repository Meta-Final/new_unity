using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeController : MonoBehaviour
{
    public GameObject bestEditorPanel;
    public GameObject bestRoomPanel;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClickNotice();
        }


    }

    public void OnClickNotice()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.name == "BestEditor")
            {
                bestEditorPanel.SetActive(true);
            }
            else if (hitInfo.transform.name == "BestRoom")
            {
                bestRoomPanel.SetActive(true);
            }
            
            
        }
    }






}
