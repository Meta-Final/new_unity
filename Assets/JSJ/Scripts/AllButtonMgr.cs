using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllButtonMgr : MonoBehaviour
{
    public Button btn_exit_1;
    public Button btn_exit_2;
    public GameObject Magazinepanel1;
    public GameObject Magazinepanel2;
    public GameObject roombtns;

    void Start()
    {
        btn_exit_1.onClick.AddListener(OnClickExitbtn1);
        btn_exit_2.onClick.AddListener(OnClickExitbtn2);
    }

    void Update()
    {
        if(Magazinepanel1.transform.GetChild(0).gameObject.activeSelf == false)
        {
            roombtns.SetActive(true);
        }
        else
        {
            roombtns.SetActive(false);
        }
    }

    // Channel -> ScrapBook
    public void ChannelToScrapBook()
    {
        MetaConnectionMgr.instance.JoinOrCreateRoom();
    }

    // Channel -> Folder
    public void ChannelToFolder()
    {
        MetaConnectionMgr.instance.JoinFolder();
    }

    // ScrapBook -> Channel
    public void ScrapBookToChannel()
    {
        MetaConnectionMgr.instance.ScrapBookToChannel();
    }

    // ScrapBook -> Folder
    public void ScrapBookToFolder()
    {
        MetaConnectionMgr.instance.ScrapBookToFolder();
    }

    // Town -> ScrapBook
    public void TownToScrapBook()
    {
        MetaConnectionMgr.instance.TownToScrapBook();
    }

    // Town -> Folder
    public void TownToFolder()
    {
        MetaConnectionMgr.instance.TownToFolder();
    }


    //게임종료
    public void Onclickpoweroff()
    {
        Application.Quit();
    }

    //크리에이터툴 닫기
    public void OnClickExitbtn1()
    {
        Magazinepanel1.transform.GetChild(0).gameObject.SetActive(false);
    }

    //크리에이터툴 닫기
    public void OnClickExitbtn2()
    {
        Magazinepanel2.transform.GetChild(0).gameObject.SetActive(false);
    }
   
}

