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
        // �÷��̾ ���� (���� Room �� ���� �Ǿ��ִ� ģ���鵵 ���̰�)
        player = PhotonNetwork.Instantiate("Player", playerPos, Quaternion.identity);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
