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

    // 게시판 오브젝트 클릭시, 해당 게시판 UI 화면이 뜸
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

    // X 클릭시, 해당 게시판 UI가 꺼짐
    public void OnClickX()
    {
        bestEditorPanel.SetActive(false);
        bestRoomPanel.SetActive(false);
    }
}
