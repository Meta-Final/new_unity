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

        // 플레이어를 생성 (현재 Room 에 접속 되어있는 친구들도 보이게)
        player = PhotonNetwork.Instantiate("Player_" + code, Vector3.zero, Quaternion.identity);
    }
}
