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
    public Button btnnotice;
    public GameObject btn_Exit;
    void Start()
    {

        ChannelCanvas = GameObject.Find("H_ChannelCanvas");
        Panel_notice = GameObject.Find("Panel_notice");
        MagCanvas = GameObject.Find("MagazineView 2");
        Panel_notice.SetActive(false);
       


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


