using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class MetaConnectionMgr : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputNickName;
    public Button btn_Connect;

    void Start()
    {
        // inputNickName 의 내용이 변경될 때 호출되는 함수 등록
        inputNickName.onValueChanged.AddListener(OnValueChanged);

        btn_Connect.onClick.AddListener(OnClickConnect);
       
    }

    void Update()
    {
        
    }

    void OnValueChanged(string s)
    {
        btn_Connect.interactable = s.Length > 0;
    }

    public void OnClickConnect()
    {
        // 마스터 서버에 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 접속 성공하면 호출되는 함수
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // 닉네임 설정
        PhotonNetwork.NickName = inputNickName.text;
        // 로비씬으로 전환
        print("로비씬으로 가자");
    }


}
