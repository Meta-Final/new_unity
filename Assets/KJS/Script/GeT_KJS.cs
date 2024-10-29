using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeT_KJS : MonoBehaviour
{
    public float detectionRange = 5.0f; // 감지 범위
    private GameObject targetFood = null; // 현재 들고 있는 food 오브젝트
    private bool isCarrying = false; // 오브젝트를 들고 있는지 여부

    void Update()
    {
        // G 키가 눌리면 감지된 오브젝트를 들기/놓기 처리
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (!isCarrying)
                DetectAndPickUpFood(); // 범위 내 오브젝트 감지 및 집기
            else
                DropFood(); // 들고 있는 오브젝트 놓기
        }

        // 오브젝트를 들고 있을 때, 플레이어의 앞쪽으로 이동시킴
        if (isCarrying && targetFood != null)
        {
            CarryFoodWithPlayer();
        }
    }

    // 범위 내의 가장 가까운 food 오브젝트를 찾고 집는 메서드
    void DetectAndPickUpFood()
    {
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");
        float closestDistance = detectionRange;
        targetFood = null;

        foreach (GameObject food in foods)
        {
            float distance = Vector3.Distance(transform.position, food.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                targetFood = food; // 가장 가까운 food 저장
            }
        }

        if (targetFood != null)
        {
            isCarrying = true;
            Debug.Log($"{targetFood.name}을(를) 집었습니다.");
        }
    }

    // 들고 있는 오브젝트를 플레이어의 앞쪽으로 이동시키는 메서드
    void CarryFoodWithPlayer()
    {
        // 플레이어가 바라보는 방향으로 약간 앞에 위치하도록 설정
        Vector3 carryPosition = transform.position + transform.forward * 0.5f + new Vector3(0, 1, 0);
        targetFood.transform.position = carryPosition;
    }

    // 들고 있는 오브젝트를 현재 위치의 Y축 0인 지점에 내려놓는 메서드
    void DropFood()
    {
        if (targetFood != null)
        {
            // 현재 위치의 y축을 0으로 설정하여 오브젝트를 놓음
            Vector3 dropPosition = targetFood.transform.position;
            dropPosition.y = 0;
            targetFood.transform.position = dropPosition;

            Debug.Log($"{targetFood.name}을(를) 내려놓았습니다.");
            targetFood = null; // 참조 해제
        }
        isCarrying = false;
    }
}