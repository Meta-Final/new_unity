using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefabSpawner : MonoBehaviour
{
    public GameObject prefab; // 각 버튼마다 설정할 프리팹

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
    }

    // 버튼 클릭 시 호출되는 메서드
    private void SpawnPrefab()
    {
        if (prefab != null)
        {
            // (0, 0, 0) 위치에 프리팹 생성
            Instantiate(prefab, Vector3.zero, Quaternion.identity);
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