using System.Collections;
using System.Collections.Generic;
using UniGLTF;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConnection : MonoBehaviour
{
    //public GameObject MagCanvas;
    //public GameObject Magazine;
    //public GameObject ChannelCanvas;
    public GameObject notice;
    public GameObject profile;
    public Button btnnotice;
    public Button btnprofile;
    public Button btnPoweroff;
    void Start()
    {
        //ChannelCanvas = GameObject.Find("H_ChannelCanvas");
        //Magazine = GameObject.Find("Tool 2");
      //  if (notice)
      //  {
            notice.SetActive(false);
       // }
        //if(MagCanvas)
        //{
        //    MagCanvas.SetActive(false);
        //}
        btnPoweroff.onClick.AddListener(Onclickpoweroff);
        
    }

    public void OnButtonClick()
    {
        // 캔버스 활성화/비활성화
        //MagCanvas.SetActive(!MagCanvas.activeSelf);
       //ChannelCanvas.SetActive(false);
    }

        public void OnClicknotice()
    {
        notice.SetActive(true);
        
    }
    public void OnClickprofile()
    {
        notice.SetActive(true);

    }
    public void OnClickprofileExit()
    {
       // Magazine.SetActive(true);
       // ChannelCanvas.SetActive(true);
    }
        public void OnClicknoiticeExit() 
    {
        notice.SetActive(false);
    }
    public void Onclickpoweroff()
    {
        Application.Quit();
        print("다 끈다");
    }

    // public void OnButtonMagazine()
    // {
    //     Magazine.SetActive(true);
    // }
    // public void OnButtonChannel()
    // {
    //     ChannelCanvas.SetActive(true);
    // }

}


