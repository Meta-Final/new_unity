using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllButtonMgr : MonoBehaviour
{
    void Start()
    {

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

















    //// 게임 종료
    //public void GameOff()
    //{

    //}
}

