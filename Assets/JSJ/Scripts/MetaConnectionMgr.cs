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
            // ������ ������ ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
            print("������ ������ ���� ����!");

            PhotonNetwork.AutomaticallySyncScene = true;
        }
        else
        {
            // ������ ������ ���� �õ�
            PhotonNetwork.ConnectUsingSettings();
            print("������ ������ ���� ����!");
        }
    }

    void Update()
    {

    }

    
    public void JoinLobby(UserInfo userInfo)
    {
        PhotonNetwork.NickName = userInfo.nickName;
        print(PhotonNetwork.NickName);

        // Lobby ����
        PhotonNetwork.JoinLobby();
    }

    // Lobby �� ������ �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("�κ� ���� �Ϸ�!");

        // Channel ������ �̵�
        SceneManager.LoadScene("Meta_Channel_Scene");
    }

    // Room �� ��������. ����, �ش� Room �� ������ Room �� ����ڴ�.
    public void JoinOrCreateRoom(string mapName)
    {
        loadLevelName = mapName;

        // �� ���� �ɼ�
        RoomOptions roomOptions = new RoomOptions();
        // �濡 ��� �� �� �ִ� �ִ� �ο� ����
        roomOptions.MaxPlayers = 20;
        // �κ� ���� ���̰� �� ���̴�?
        roomOptions.IsVisible = true;
        // �濡 ������ �� �� �ִ�?
        roomOptions.IsOpen = true;

        // Room ���� or ����
        PhotonNetwork.JoinOrCreateRoom("My_ScrapBook : " + PhotonNetwork.NickName, roomOptions, TypedLobby.Default);
    }

    public void JoinMap()
    {


    }

    // Town �� ��������. 
    public void JoinOrCreateTown(string mapName)
    {
        loadLevelName = mapName;

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

        PhotonNetwork.LoadLevel(loadLevelName);
        // ��Ƽ�÷��� ������ ��� �� �ִ� ����
        if (PhotonNetwork.IsMasterClient)
        {
            
        }
    }

}
