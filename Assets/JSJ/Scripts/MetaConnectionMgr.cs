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

    int roomNumber = 0;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            // 마스터 서버에 접속 시도
            PhotonNetwork.ConnectUsingSettings();
            print("마스터 서버에 접속 성공!");

            //PhotonNetwork.AutomaticallySyncScene = true;
        }
        else
        {
            print("마스터 서버에 접속 되어 있음");
        }
    }


    // Lobby 입장
    public void JoinLobby(UserInfo userInfo)
    {
        PhotonNetwork.NickName = userInfo.nickName;
        print(PhotonNetwork.NickName);

        PhotonNetwork.JoinLobby();
    }

    // Lobby 에 접속을 성공하면 호출되는 함수
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("로비 입장 완료!");

        JoinChannel();
    }


    // Channel 입장
    public void JoinChannel()
    {
        // 채널씬으로 이동
        PhotonNetwork.LoadLevel("Meta_Channel_Scene");
    }


    // [Room]-------------------------------------------------------------------------------------------------------
    // ScrapBook 생성 후 입장
    public void JoinOrCreateRoom()
    {
        loadLevelName = "Meta_ScrapBook_Scene";

        string roomName = PhotonNetwork.NickName + "'s ScrapBook";

        // 방 생성 옵션
        RoomOptions roomOptions = new RoomOptions();
        // 방에 들어 올 수 있는 최대 인원 설정
        roomOptions.MaxPlayers = 20;
        // 로비에 방을 보이게 할 것이니?
        roomOptions.IsVisible = true;
        // 방에 참여를 할 수 있니?
        roomOptions.IsOpen = true;

        // Room 참여 or 생성
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // ScrapBook 입장
    public void JoinRoom()
    {
        string roomName = PhotonNetwork.NickName + "'s ScrapBook";

        PhotonNetwork.JoinRoom(roomName);
    }


    // [Folder]----------------------------------------------------------------------------------------------------------
    // Folder 입장
    public void JoinFolder()
    {
        PhotonNetwork.LoadLevel("Meta_Folder_Scene");
    }


    // [Town]----------------------------------------------------------------------------------------------------------
    // Town 생성 후 입장
    public void JoinOrCreateTown()
    {
        loadLevelName = "Meta_Town_Scene";

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

    // Town 입장
    public void JoinTown()
    {
        PhotonNetwork.JoinRoom("Town");
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

        print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LoadLevel(loadLevelName);
        // 멀티플레이 컨텐츠 즐길 수 있는 상태
    }


    // [Room 에서 나가기]--------------------------------------------------------------------------------------------------
    // ScrapBook -> Channel
    public void ScrapBookToChannel()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 1;
    }

    // ScrapBook -> Folder
    public void ScrapBookToFolder()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 2;
    }

    // Room 에서 나오면 호출되는 함수
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("방 나와서 이동 : " + roomNumber);

        // 만약 룸넘버가 1이면, Channel 로 이동한다.
        if (roomNumber == 1)
        {
            JoinChannel();
        }
        // 만약 룸넘버가 2면, Folder 로 이동한다.
        if (roomNumber == 2)
        {
            JoinFolder();
        }
        
    }


    

    

    

}
