using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenubarMgr : MonoBehaviour
{
    public GameObject Menubar;
    void Start()
    {
        Menubar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Menubar != null && Input.GetKeyDown(KeyCode.Tab))
        {
            Menubar.SetActive(!Menubar.activeSelf);

            

        }
        
           
    }
}
