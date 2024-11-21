using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    
    private void Awake()
    {
        int code = AvatarManager.instance.avatarCode;

        // �÷��̾ ���� (���� Room �� ���� �Ǿ��ִ� ģ���鵵 ���̰�)
        player = PhotonNetwork.Instantiate("Player_" + code, Vector3.zero, Quaternion.identity);
    }
}
