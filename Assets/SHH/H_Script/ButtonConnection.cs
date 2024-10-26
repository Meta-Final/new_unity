using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ButtonConnection : MonoBehaviour
{
   // public GameObject MagCanvas;
    public GameObject ChannelCanvas;
    public GameObject Panel_notice;
    public Button btnnotice;
    public GameObject btn_Exit;
    void Start()
    {
       // MagCanvas = GameObject.Find("CanvasMag");
        ChannelCanvas = GameObject.Find("ChannelCanvas");
        Panel_notice = GameObject.Find("Panel_notice");
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
       // MagCanvas.SetActive(true);
        ChannelCanvas.SetActive(false);
    }
    public void OnButtonClicknotice()
    {
        Panel_notice.SetActive(!Panel_notice.activeSelf);
    }
    public void OnClickExit() 
    {
        Panel_notice.SetActive(false);
    }
}


