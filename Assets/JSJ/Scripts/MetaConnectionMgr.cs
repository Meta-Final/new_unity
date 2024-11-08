using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MetaConnectionMgr : MonoBehaviourPunCallbacks
{
    public static MetaConnectionMgr instance;

    public string loadLevelName;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            // 마스터 서버에 접속 시도
            PhotonNetwork.ConnectUsingSettings();
            print("마스터 서버에 접속 성공!");

            PhotonNetwork.AutomaticallySyncScene = true;
        }
        else
        {
            // 마스터 서버에 접속 시도
            PhotonNetwork.ConnectUsingSettings();
            print("마스터 서버에 접속 성공!");
        }
    }

    void Update()
    {

    }

    
    public void JoinLobby(UserInfo userInfo)
    {
        PhotonNetwork.NickName = userInfo.nickName;
        print(PhotonNetwork.NickName);

        // Lobby 입장
        PhotonNetwork.JoinLobby();
    }

    // Lobby 에 접속을 성공하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 입장 완료!");

        // Channel 씬으로 이동
        SceneManager.LoadScene("Meta_Channel_Scene");
    }

    // Room 에 입장하자. 만일, 해당 Room 이 없으면 Room 을 만들겠다.
    public void JoinOrCreateRoom(string mapName)
    {
        loadLevelName = mapName;

        // 방 생성 옵션
        RoomOptions roomOptions = new RoomOptions();
        // 방에 들어 올 수 있는 최대 인원 설정
        roomOptions.MaxPlayers = 20;
        // 로비에 방을 보이게 할 것이니?
        roomOptions.IsVisible = true;
        // 방에 참여를 할 수 있니?
        roomOptions.IsOpen = true;

        // Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom("My_ScrapBook : " + PhotonNetwork.NickName, roomOptions, TypedLobby.Default);
    }

    public void JoinMap()
    {


    }

    // Town 에 입장하자. 
    public void JoinOrCreateTown(string mapName)
    {
        loadLevelName = mapName;

        // 방 생성 옵션
        RoomOptions roomOptions = new RoomOptions();
        // 방에 들어 올 수 있는 최대 인원 설정
        roomOptions.MaxPlayers = 20;
        // 로비에 방을 보이게 할 것이니?
        roomOptions.IsVisible = true;
        // 방에 참여를 할 수 있니?
        roomOptions.IsOpen = true;

        // Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom("Town", roomOptions, TypedLobby.Default);
    }

    // 방 생성 성공했을 때 호출되는 함수
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    // 방 입장 성공했을 때 호출되는 함수
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("방 입장 완료");

        PhotonNetwork.LoadLevel(loadLevelName);
        // 멀티플레이 컨텐츠 즐길 수 있는 상태
        if (PhotonNetwork.IsMasterClient)
        {
            
        }
    }

}
