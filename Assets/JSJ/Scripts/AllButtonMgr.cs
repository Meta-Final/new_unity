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

    // ä�� -> ��ũ�� ��
    public void ChannelToScrapBook()
    {
        MetaConnectionMgr.instance.JoinOrCreateRoom();
    }

    // ä�� -> ��
    public void ChannelToMap()
    {
        MetaConnectionMgr.instance.JoinMap();
    }

    // ��ũ�� �� -> ä��
    public void ScrapBookToChannel()
    {
        MetaConnectionMgr.instance.ScrapBookToChannel();
    }

    // ��ũ�� �� -> ��
    public void ScrapBookToMap()
    {
        MetaConnectionMgr.instance.ScrapBookToMap();
    }

















    //// ���� ����
    //public void GameOff()
    //{

    //}
}

