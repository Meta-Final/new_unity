using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPoint;

    private void Awake()
    {
        // �÷��̾ ���� (���� Room �� ���� �Ǿ��ִ� ģ���鵵 ���̰�)
        player = PhotonNetwork.Instantiate("Player", spawnPoint.position, Quaternion.identity);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
