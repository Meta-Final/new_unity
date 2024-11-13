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

    // Scene���� tool UI�� �����ϱ� ���� ����
    private GameObject Chat;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Ŭ���̾�Ʈ�� ���� �÷��̾� ã��
        FindLocalPlayer();

        // "MagazineView" ������Ʈ �ȿ� �ִ� "Tool" UI�� ã��
        GameObject magazineView = GameObject.Find("MagazineView");
        if (magazineView != null)
        {
            Chat = magazineView.transform.Find("Chat")?.gameObject;
            if (Chat != null)
            {
                // tool UI�� ��Ȱ��ȭ�� ���·� �ʱ�ȭ
                Chat.SetActive(false);
            }
            else
            {
                Debug.LogError("Tool UI not found within MagazineView.");
            }
        }
        else
        {
            Debug.LogError("MagazineView object not found in the scene.");
        }
    }

    void Update()
    {
        // AI�� �� Ŭ���̾�Ʈ�� ���� �÷��̾ ���󰡵��� ����
        if (playerTransform != null && photonView.IsMine)
        {
            // �÷��̾��� ȸ���� ������ ��� ��ġ ���
            Vector3 rotatedOffset = playerTransform.rotation * offset;
            Vector3 targetPosition = playerTransform.position + rotatedOffset;

            agent.SetDestination(targetPosition);
        }

        if (Input.GetMouseButtonDown(0) && photonView.IsMine)  // ���콺 ���� Ŭ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // �� ��ũ��Ʈ�� ������ ������Ʈ�� Ŭ���ߴ��� Ȯ��
                if (hit.transform == transform)
                {
                    if (Chat != null)
                    {
                        Chat.SetActive(true);
                    }
                }
            }
        }

        // ESC Ű�� ������ �� tool UI�� ��Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Escape) && Chat != null)
        {
            Chat.SetActive(false);
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

    private void OnMouseDown()
    {
        // �� ��ũ��Ʈ�� �Ҵ�� ������Ʈ�� ���� �÷��̾ Ŭ���� ��� tool UI�� Ȱ��ȭ
        if (photonView.IsMine && Chat != null)
        {
            Chat.SetActive(true);
        }
    }
}