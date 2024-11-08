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
        // 마스터 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();

        print("마스터 서버에 접속 성공!");
    }

    


}
