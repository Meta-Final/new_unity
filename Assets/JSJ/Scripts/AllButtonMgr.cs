using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllButtonMgr : MonoBehaviour
{

    public Button btn_Exit;

    GameObject Magazinepanel = null;
    void Start()
    {
        btn_Exit.onClick.AddListener(OnClickExitbtn);
    }

    void Update()
    {

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
    public void OnClickExitbtn()
    {

        if(Magazinepanel == null)
        {
            Magazinepanel = GameObject.Find("MagazineView 2");
        }
        Magazinepanel.transform.GetChild(0).gameObject.SetActive(false);
    }
   
}

