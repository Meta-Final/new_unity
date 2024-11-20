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

    AvatarManager avatarManager;
    
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

        avatarManager = GetComponent<AvatarManager>();
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


    // [Room]-------------------------------------------------------------------------------------------------------
    // ScrapBook ���� �� ����
    public void JoinOrCreateRoom()
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


    // [Folder]----------------------------------------------------------------------------------------------------------
    // Folder ����
    public void JoinFolder()
    {
        PhotonNetwork.LoadLevel("Meta_Folder_Scene");
    }


    // [Town]----------------------------------------------------------------------------------------------------------
    // Town ���� �� ����
    public void JoinOrCreateTown()
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

        avatarManager.AssignRandomAvatar();

        PhotonNetwork.LoadLevel(loadLevelName);
        // ��Ƽ�÷��� ������ ��� �� �ִ� ����

    }


    // [Room ���� ������]--------------------------------------------------------------------------------------------------
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

    // Town -> Channel
    public void TownToChannel()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 3;
    }

    // Town -> Folder
    public void TownToFolder()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 4;
    }

    // Town -> ScrapBook
    public void TownToScrapBook()
    {
        PhotonNetwork.LeaveRoom();
        roomNumber = 5;
    }


    // Room ���� ������ ȣ��Ǵ� �Լ�
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("�� ���ͼ� �̵� : " + roomNumber);

        // ������ ������ ���������� �ʴٸ�,
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            // ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
        }
        // �̹� ������ ������ �������ִٸ�,
        else
        {
            // �� �̵�
            RoomTransition();
        }
    }

    // ������ ������ �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("������ ������ �������߽��ϴ�.");

        // �� �̵�
        RoomTransition();
    }

    // �� �̵� �Լ�
    public void RoomTransition()
    {
        // ���� ��ѹ��� 1�̰ų� 3�̸�, Channel �� �̵��Ѵ�.
        if (roomNumber == 1 || roomNumber == 3)
        {
            JoinChannel();
        }
        // ���� ��ѹ��� 2�̰ų� 4��, Folder �� �̵��Ѵ�.
        if (roomNumber == 2 || roomNumber == 4 )
        {
            JoinFolder();
        }
        // ���� ��ѹ��� 5�̸�, ScrapBook ���� �̵��Ѵ�.
        if (roomNumber == 5)
        {
            JoinOrCreateRoom();
        }

        // �̵� �� �ʱ�ȭ
        roomNumber = 0;
    }
}
