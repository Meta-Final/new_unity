using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWithPicket : MonoBehaviourPun
{
    public GameObject player;
    public GameObject canvas_PicketNotice;

    bool enterPicketZone = false;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
    }

    // Trigger ������ Player �� ���� ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !enterPicketZone)
        {
            if (photonView.IsMine)
            {
                canvas_PicketNotice.SetActive(true);
            }

            enterPicketZone = true;
        }
    }

    // Trigger ������ Player �� ������ ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enterPicketZone)
        {
            if (photonView.IsMine)
            {
                canvas_PicketNotice.SetActive(false);
            }
            
            enterPicketZone = false;
        }
    }
}
