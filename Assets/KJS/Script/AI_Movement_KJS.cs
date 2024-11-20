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

    private bool isAgentEnabled = true; // NavMeshAgent Ȱ��ȭ ���� �÷���
    private bool isRotatingToPlayer = false; // ���� �÷��̾� �������� ȸ�� �÷���
    private Transform localPlayerTransform; // ���� �÷��̾��� Transform

    public float rotationSpeed = 2f; // ȸ�� �ӵ�

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
                // tool UI, NPC�� ��Ȱ��ȭ�� ���·� �ʱ�ȭ
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
        if (playerTransform != null && photonView.IsMine && isAgentEnabled)
        {
            // �÷��̾��� ȸ���� ������ ��� ��ġ ���
            Vector3 rotatedOffset = playerTransform.rotation * offset;
            Vector3 targetPosition = playerTransform.position + rotatedOffset;

            agent.SetDestination(targetPosition);
        }

        // ���� �÷��̾� �������� ȸ��
        if (isRotatingToPlayer && localPlayerTransform != null)
        {
            RotateToPlayer();
        }

        if (Input.GetMouseButtonDown(0) && photonView.IsMine) // ���콺 ���� Ŭ��
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
                        OnMouseDown();
                    }
                }
            }
        }

        // ESC Ű�� ������ �� tool UI�� ��Ȱ��ȭ�ϰ� NavMeshAgent�� �ٽ� Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Escape) && Chat != null)
        {
            Chat.SetActive(false);

            if (agent != null && !isAgentEnabled)
            {
                agent.enabled = true; // NavMeshAgent �ٽ� Ȱ��ȭ
                isAgentEnabled = true;
                isRotatingToPlayer = false; // ���� �÷��̾� ���� ȸ�� �ߴ�
            }
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
                playerTransform = player.transform; // AI�� ����ٴϴ� �÷��̾� ����
                localPlayerTransform = player.transform; // ���� �÷��̾��� Transform ����
                break;
            }
        }

        if (playerTransform == null)
        {
            Debug.LogError("Local player object with tag 'Player' not found.");
        }
    }

    public void OnMouseDown()
    {
        // �� ��ũ��Ʈ�� �Ҵ�� ������Ʈ�� ���� �÷��̾ Ŭ���� ��� tool UI�� Ȱ��ȭ
        if (photonView.IsMine && Chat != null)
        {
            Chat.SetActive(true);
        }

        // NavMeshAgent ��Ȱ��ȭ
        if (agent != null && isAgentEnabled)
        {
            agent.enabled = false; // NavMeshAgent ��Ȱ��ȭ
            isAgentEnabled = false;

            // ���� �÷��̾� �������� ȸ�� ����
            isRotatingToPlayer = true;
        }

        // CameraManager�� MoveCameraToPosition ȣ��
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            cameraManager.MoveCameraToPosition();
        }
    }

    private void RotateToPlayer()
    {
        if (localPlayerTransform == null) return;

        // ���� �÷��̾� ���� ���
        Vector3 directionToPlayer = localPlayerTransform.position - transform.position;
        directionToPlayer.y = 0; // Y�� ���� (���� �������θ� ȸ��)

        // ���� ���⿡�� ���� �÷��̾� �������� ���������� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ���� �÷��̾� �������� ���� ȸ�� �Ϸ� ��
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isRotatingToPlayer = false; // ȸ�� �ߴ�
            Debug.Log("���� �÷��̾� �������� ȸ�� �Ϸ�!");
        }
    }
    public void ResetAgentState()
    {
        // NavMeshAgent Ȱ��ȭ
        if (agent != null && !isAgentEnabled)
        {
            agent.enabled = true; // NavMeshAgent �ٽ� Ȱ��ȭ
            isAgentEnabled = true;
        }

        // ���� �÷��̾� ���� ȸ�� �ߴ�
        isRotatingToPlayer = false;

        // Chat UI ��Ȱ��ȭ (���� Ȯ��)
        if (Chat != null && Chat.activeSelf)
        {
            Chat.SetActive(false);
        }

        Debug.Log("AI_Movement_KJS: NavMeshAgent �� ���°� �ʱ�ȭ�Ǿ����ϴ�.");
    }
}