using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefabSpawner : MonoBehaviour
{
    public GameObject prefab; // 각 버튼마다 설정할 프리팹
    private GameObject player; // 플레이어 오브젝트 참조

    private Button button; // 버튼 컴포넌트 참조

    private void Awake()
    {
        // 버튼 컴포넌트 가져오기
        button = GetComponent<Button>();

        // 버튼 클릭 이벤트에 메서드 등록
        if (button != null)
        {
            button.onClick.AddListener(SpawnPrefab);
        }
        else
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
        }

        // Player 태그를 가진 오브젝트 자동 검색
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다.");
        }
    }

    // 버튼 클릭 시 호출되는 메서드
    private void SpawnPrefab()
    {
        if (prefab != null && player != null)
        {
            // 플레이어 위치에서 (1, 1, 1) 오프셋 적용
            Vector3 spawnPosition = player.transform.position + new Vector3(1, 0, 0);

            // 프리팹 생성
            GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);

            // 생성된 프리팹의 크기를 15로 조정
            spawnedPrefab.transform.localScale = Vector3.one * 1;
        }
        else if (player == null)
        {
            Debug.LogWarning("Player 오브젝트를 찾을 수 없습니다.");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} 버튼에 프리팹이 할당되지 않았습니다.");
        }
    }

    private void OnDestroy()
    {
        // 이벤트 등록 해제 (메모리 누수 방지)
        if (button != null)
        {
            button.onClick.RemoveListener(SpawnPrefab);
        }
    }
}