using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeT_KJS : MonoBehaviour
{
    public float detectionRange = 5.0f; // 검사할 거리 범위

    // 매 프레임 호출
    void Update()
    {
        // G 키가 눌렸을 때 실행
        if (Input.GetKeyDown(KeyCode.G))
        {
            DestroyNearbyFood();
        }
    }

    // 가까운 food 태그를 가진 오브젝트를 찾아 파괴하는 메서드
    void DestroyNearbyFood()
    {
        // 모든 food 태그를 가진 오브젝트를 배열로 가져오기
        GameObject[] foods = GameObject.FindGameObjectsWithTag("food");

        foreach (GameObject food in foods)
        {
            // 플레이어(현재 오브젝트)와 food 사이의 거리 계산
            float distance = Vector3.Distance(transform.position, food.transform.position);

            // 설정된 범위 내에 있을 경우 오브젝트 파괴
            if (distance <= detectionRange)
            {
                Destroy(food);
                Debug.Log($"{food.name} 오브젝트가 파괴되었습니다.");
                break; // 한 번에 하나의 오브젝트만 파괴하고 종료
            }
        }
    }
}
