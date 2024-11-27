using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWithKeyword : MonoBehaviourPun
{
    public GameObject player;
    public GameObject Canvas_KeywordNotice;

    bool enterKeyword = false;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;
    }

    // Trigger 영역에 Player 가 들어갔을 때
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player") && !enterKeyword)
        {
            if (photonView.IsMine)
            {
                Canvas_KeywordNotice.SetActive(true);
            }
            
            enterKeyword = true;
        }
    }

    // Trigger 영역에 Player 가 나갔을 때
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && enterKeyword)
        {
            if (photonView.IsMine)
            {
                Canvas_KeywordNotice.SetActive(false);
            }
            
            enterKeyword = false;
        }
    }
}
