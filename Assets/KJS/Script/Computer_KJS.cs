using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer_KJS : MonoBehaviour
{
    public GameObject uiElement; // 활성화할 UI 오브젝트
    public float activationDistance = 5f; // UI가 활성화되는 거리
    private GameObject player; // 동적으로 생성된 플레이어 참조
    private CharacterController playerController; // 플레이어의 CharacterController
    private bool isPlayerInRange = false; // 플레이어가 범위 안에 있는지 여부

    void Start()
    {
        uiElement.SetActive(false); // 시작할 때 UI 비활성화
        StartCoroutine(FindPlayer()); // 플레이어 찾기 시도
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= activationDistance)
            {
                isPlayerInRange = true; // 범위 안에 있음
            }
            else
            {
                isPlayerInRange = false; // 범위 벗어남
                DisableUI(); // 범위를 벗어나면 UI 비활성화
            }

            // 플레이어가 범위 내에 있고, 엔터 키를 누르면 UI 활성화
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.Return))
            {
                EnableUI(); // UI 활성화
            }

            // Esc 키를 누르면 UI 비활성화
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DisableUI(); // UI 비활성화
            }
        }
    }

    // 동적으로 생성된 플레이어를 찾는 코루틴
    private IEnumerator FindPlayer()
    {
        while (player == null) // 플레이어가 없으면 계속 시도
        {
            player = GameObject.FindGameObjectWithTag("Player"); // "Player" 태그로 플레이어 찾기

            if (player != null) // 찾았을 경우
            {
                playerController = player.GetComponent<CharacterController>(); // CharacterController 참조
                if (playerController == null)
                {
                    Debug.LogWarning("플레이어에 CharacterController가 없습니다.");
                }
            }

            yield return new WaitForSeconds(1f); // 1초마다 재시도
        }
    }

    // UI 활성화 시 호출되는 메서드
    void EnableUI()
    {
        uiElement.SetActive(true); // UI 활성화
        if (playerController != null)
        {
            playerController.enabled = false; // 플레이어 조작 비활성화
        }
    }

    // UI 비활성화 시 호출되는 메서드
    void DisableUI()
    {
        uiElement.SetActive(false); // UI 비활성화
        if (playerController != null)
        {
            playerController.enabled = true; // 플레이어 조작 활성화
        }
    }
}
