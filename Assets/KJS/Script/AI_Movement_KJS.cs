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

        // Ŭ���̾�Ʈ�� ���� �÷��̾� ã��
        FindLocalPlayer();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            // �÷��̾��� ȸ���� ������ ��� ��ġ ���
            Vector3 rotatedOffset = playerTransform.rotation * offset;
            Vector3 targetPosition = playerTransform.position + rotatedOffset;

            agent.SetDestination(targetPosition);
        }
    }

    private void FindLocalPlayer()
    {
        // ��� �÷��̾� ������Ʈ�� ã�� ���� �÷��̾ ����
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();

            // �� Ŭ���̾�Ʈ�� ���� �÷��̾����� Ȯ��
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