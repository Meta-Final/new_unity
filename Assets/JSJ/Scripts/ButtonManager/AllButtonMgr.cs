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

    // Town -> ScrapBook
    public void TownToScrapBook()
    {
        MetaConnectionMgr.instance.TownToScrapBook();
    }

    // Town -> Folder
    public void TownToChannel()
    {
        MetaConnectionMgr.instance.TownToChannel();
    }

    
}
