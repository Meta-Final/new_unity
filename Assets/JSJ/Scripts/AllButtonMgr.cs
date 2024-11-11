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

    // √§≥Œ -> Ω∫≈©∑¶ ∫œ
    public void ChannelToScrapBook()
    {
        MetaConnectionMgr.instance.JoinOrCreateRoom();
    }

    // √§≥Œ -> ∏ 
    public void ChannelToMap()
    {
        MetaConnectionMgr.instance.JoinMap();
    }

    // Ω∫≈©∑¶ ∫œ -> √§≥Œ
    public void ScrapBookToChannel()
    {
        MetaConnectionMgr.instance.ScrapBookToChannel();
    }

    // Ω∫≈©∑¶ ∫œ -> ∏ 
    public void ScrapBookToMap()
    {
        MetaConnectionMgr.instance.ScrapBookToMap();
    }

















    //// ∞‘¿” ¡æ∑·
    //public void GameOff()
    //{

    //}
}

