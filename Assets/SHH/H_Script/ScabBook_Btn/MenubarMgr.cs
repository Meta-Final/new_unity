using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

public class MenubarMgr : MonoBehaviour
{

    public GameObject panelMenu;
    void Start()
    {
       Transform roomMenu = panelMenu.transform.Find("RoomMenu");

        if(roomMenu != null)
        {
            roomMenu.gameObject.SetActive(false);
        }
    }
     

    void Update()
    {
        if (panelMenu != null && Input.GetKeyDown(KeyCode.Tab))
        {
            panelMenu.SetActive(!panelMenu.activeSelf);

            

        }
    }
   
}
