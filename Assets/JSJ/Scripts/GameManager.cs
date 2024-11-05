using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    Vector3 playerPos = new Vector3(0, 2, 0);

    private void Awake()
    {
        // 플레이어를 생성 (현재 Room 에 접속 되어있는 친구들도 보이게)
        player = PhotonNetwork.Instantiate("Player", playerPos, Quaternion.identity);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
