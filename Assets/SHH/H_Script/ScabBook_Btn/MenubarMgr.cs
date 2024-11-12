using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class MenubarMgr : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject roomMenu;
    void Start()
    {
        Transform roomMenu = MenuPanel.transform.Find("RoomBar");

        if(roomMenu != null)
        {
            roomMenu.gameObject.SetActive(false);
        }
    }
     

    void Update()
    {
        if (roomMenu != null && Input.GetKeyDown(KeyCode.Tab))
        {
            roomMenu.SetActive(!roomMenu.activeSelf);
        }
    }

   
   
}
