using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonConnection : MonoBehaviour
{
   // public GameObject MagCanvas;
    public GameObject ChannelCanvas;
    public GameObject Panel_notice;
    public GameObject MagCanvas;
    public GameObject Magazine;
    public Button btnnotice;
    public GameObject btn_Exit;
    void Start()
    {

        ChannelCanvas = GameObject.Find("H_ChannelCanvas");
        Magazine = GameObject.Find("MagazineView 2");

        if (Panel_notice)
        {
            Panel_notice.SetActive(false);
        }
        if(MagCanvas)
        {
            MagCanvas.SetActive(false);
        }


    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnButtonClick()
    {
        // 캔버스 활성화/비활성화
        //MagCanvas.SetActive(!MagCanvas.activeSelf);
       MagCanvas.SetActive(false);
       ChannelCanvas.SetActive(false);
    }

    public void OnButtonMagazine()
    {
        Magazine.SetActive(true);
    }
    public void OnButtonChannel()
    {
        ChannelCanvas.SetActive(true);
    }
        public void OnButtonClicknotice()
    {
        Panel_notice.SetActive(true);
    }
    public void OnClickExit() 
    {
        Panel_notice.SetActive(false);
    }
}


