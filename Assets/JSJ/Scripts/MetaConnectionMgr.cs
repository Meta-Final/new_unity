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
        // inputNickName �� ������ ����� �� ȣ��Ǵ� �Լ� ���
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
        // ������ ������ ���� �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // ������ ������ ���� �����ϸ� ȣ��Ǵ� �Լ�
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        // �г��� ����
        PhotonNetwork.NickName = inputNickName.text;
        // �κ������ ��ȯ
        print("�κ������ ����");
    }


}
