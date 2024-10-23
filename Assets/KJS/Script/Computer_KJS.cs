using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer_KJS : MonoBehaviour
{
    public GameObject uiElement; // Ȱ��ȭ�� UI ������Ʈ
    public float activationDistance = 5f; // UI�� Ȱ��ȭ�Ǵ� �Ÿ�
    private GameObject player; // �������� ������ �÷��̾� ����
    private CharacterController playerController; // �÷��̾��� CharacterController
    private bool isPlayerInRange = false; // �÷��̾ ���� �ȿ� �ִ��� ����

    void Start()
    {
        uiElement.SetActive(false); // ������ �� UI ��Ȱ��ȭ
        StartCoroutine(FindPlayer()); // �÷��̾� ã�� �õ�
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= activationDistance)
            {
                isPlayerInRange = true; // ���� �ȿ� ����
            }
            else
            {
                isPlayerInRange = false; // ���� ���
                DisableUI(); // ������ ����� UI ��Ȱ��ȭ
            }

            // �÷��̾ ���� ���� �ְ�, ���� Ű�� ������ UI Ȱ��ȭ
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.Return))
            {
                EnableUI(); // UI Ȱ��ȭ
            }

            // Esc Ű�� ������ UI ��Ȱ��ȭ
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DisableUI(); // UI ��Ȱ��ȭ
            }
        }
    }

    // �������� ������ �÷��̾ ã�� �ڷ�ƾ
    private IEnumerator FindPlayer()
    {
        while (player == null) // �÷��̾ ������ ��� �õ�
        {
            player = GameObject.FindGameObjectWithTag("Player"); // "Player" �±׷� �÷��̾� ã��

            if (player != null) // ã���� ���
            {
                playerController = player.GetComponent<CharacterController>(); // CharacterController ����
                if (playerController == null)
                {
                    Debug.LogWarning("�÷��̾ CharacterController�� �����ϴ�.");
                }
            }

            yield return new WaitForSeconds(1f); // 1�ʸ��� ��õ�
        }
    }

    // UI Ȱ��ȭ �� ȣ��Ǵ� �޼���
    void EnableUI()
    {
        uiElement.SetActive(true); // UI Ȱ��ȭ
        if (playerController != null)
        {
            playerController.enabled = false; // �÷��̾� ���� ��Ȱ��ȭ
        }
    }

    // UI ��Ȱ��ȭ �� ȣ��Ǵ� �޼���
    void DisableUI()
    {
        uiElement.SetActive(false); // UI ��Ȱ��ȭ
        if (playerController != null)
        {
            playerController.enabled = true; // �÷��̾� ���� Ȱ��ȭ
        }
    }
}
