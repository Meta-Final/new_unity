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
            // ������ ������ ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
            print("������ ������ ���� ����!");

            //PhotonNetwork.AutomaticallySyncScene = true;
        }
        else
        {
            print("������ ������ ���� �Ǿ� ����");
        }
    }

    // Lobby ����
    public void JoinLobby(UserInfo userInfo)
    {
        PhotonNetwork.NickName = userInfo.nickName;
        print(PhotonNetwork.NickName);

        PhotonNetwork.JoinLobby();
    }

    // Lobby �� ������ �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� �Ϸ�!");

        JoinChannel();
    }

    // Channel ����
    public void JoinChannel()
    {
        // ä�ξ����� �̵�
        PhotonNetwork.LoadLevel("Meta_Channel_Scene");
    }

    // Room ���� �� ����
    public void CreateRoom()
    {
        loadLevelName = "Meta_ScrapBook_Scene";

        string roomName = PhotonNetwork.NickName + "'s ScrapBook";

        // �� ���� �ɼ�
        RoomOptions roomOptions = new RoomOptions();
        // �濡 ��� �� �� �ִ� �ִ� �ο� ����
        roomOptions.MaxPlayers = 20;
        // �κ� ���� ���̰� �� ���̴�?
        roomOptions.IsVisible = true;
        // �濡 ������ �� �� �ִ�?
        roomOptions.IsOpen = true;

        // Room ���� or ����
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    // Room ����
    public void JoinRoom()
    {
        string roomName = PhotonNetwork.NickName + "'s ScrapBook";

        PhotonNetwork.JoinRoom(roomName);
    }


    // Map ����
    public void JoinMap()
    {
        PhotonNetwork.LoadLevel("Meta_Map_Scene");
    }

    // Town ���� �� ����
    public void CreateTown()
    {
        loadLevelName = "Meta_Town_Scene";

        // �� ���� �ɼ�
        RoomOptions roomOptions = new RoomOptions();
        // �濡 ��� �� �� �ִ� �ִ� �ο� ����
        roomOptions.MaxPlayers = 20;
        // �κ� ���� ���̰� �� ���̴�?
        roomOptions.IsVisible = true;
        // �濡 ������ �� �� �ִ�?
        roomOptions.IsOpen = true;

        // Room ���� or ����
        PhotonNetwork.JoinOrCreateRoom("Town", roomOptions, TypedLobby.Default);
    }

    // Town ����
    public void JoinTown()
    {
        PhotonNetwork.JoinRoom("Town");
    }

    

    // �� ���� �������� �� ȣ��Ǵ� �Լ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("�� ���� �Ϸ�");
    }

    // �� ���� �������� �� ȣ��Ǵ� �Լ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("�� ���� �Ϸ�");

        print("���� �� �̸� : " + PhotonNetwork.CurrentRoom.Name);

        PhotonNetwork.LoadLevel(loadLevelName);
        // ��Ƽ�÷��� ������ ��� �� �ִ� ����
    }

}
