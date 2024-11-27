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

    // Trigger 영역에 Player 가 들어갔을 때
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

    // Trigger 영역에 Player 가 나갔을 때
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
