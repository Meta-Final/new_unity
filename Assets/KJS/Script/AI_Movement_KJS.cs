using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class AI_Movement_KJS : MonoBehaviourPun
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    private Vector3 offset = new Vector3(1, 3, -1);

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // 클라이언트의 로컬 플레이어 찾기
        FindLocalPlayer();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // 플레이어의 회전을 적용한 상대 위치 계산
            Vector3 rotatedOffset = playerTransform.rotation * offset;
            Vector3 targetPosition = playerTransform.position + rotatedOffset;

            agent.SetDestination(targetPosition);
        }
    }

    private void FindLocalPlayer()
    {
        // 모든 플레이어 오브젝트를 찾아 로컬 플레이어를 설정
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();

            // 이 클라이언트에 속한 플레이어인지 확인
            if (photonView != null && photonView.IsMine)
            {
                playerTransform = player.transform;
                break;
            }
        }

        if (playerTransform == null)
        {
            Debug.LogError("Local player object with tag 'Player' not found.");
        }
    }
}