using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWithKeyword : MonoBehaviour
{
    public GameObject player;
    public GameObject Canvas_KeywordNotice;

    bool enterKeyword = false;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
    }

    // Trigger ������ Player �� ���� ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enterKeyword)
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                Canvas_KeywordNotice.SetActive(true);
            }
            
            enterKeyword = true;
        }
    }

    // Trigger ������ Player �� ������ ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enterKeyword)
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                Canvas_KeywordNotice.SetActive(false);
            }
            
            enterKeyword = false;
        }
    }
}
