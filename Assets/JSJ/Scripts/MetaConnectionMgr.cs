using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class MetaConnectionMgr : MonoBehaviourPunCallbacks
{
    void Start()
    {
        Connect();
    }

    void Update()
    {

    }

    public void Connect()
    {
        // ������ ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();

        print("������ ������ ���� ����!");
    }

    


}
