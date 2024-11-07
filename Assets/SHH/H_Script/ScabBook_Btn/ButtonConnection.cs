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
    public Button btnexit;
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
        btnprofile.onClick.AddListener(OnClickprofile);
        btnnotice.onClick.AddListener(OnClicknotice);
        btnexit.onClick.AddListener(OnClicknoticeExit);
    }
    private void Update()
    {
        ExitKey();
    }
    #region ��ưŬ���Լ�
    public void OnButtonClick()
    {
        // ĵ���� Ȱ��ȭ/��Ȱ��ȭ
        //MagCanvas.SetActive(!MagCanvas.activeSelf);
       //ChannelCanvas.SetActive(false);
    }

    public void OnClicknotice()
    {
        notice.SetActive(true);
        
    }
    public void ExitKey() 
    {
        if (profile != null && Input.GetKeyDown(KeyCode.Escape))
        {
            profile.SetActive(false);
        }
    }
    public void OnClickprofile()
    {
        profile.SetActive(true);
        

    }
    public void OnClicknoticeExit()
    {
        notice.SetActive(false);
    }
    public void Onclickpoweroff()
    {
        Application.Quit();
        print("�� ����");
    }

    // public void OnButtonMagazine()
    // {
    //     Magazine.SetActive(true);
    // }
    // public void OnButtonChannel()
    // {
    //     ChannelCanvas.SetActive(true);
    // }
    #endregion
}


