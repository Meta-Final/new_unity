using UnityEngine;

public class PrefabManager_KJS : MonoBehaviour
{
    public GameObject uiTool; // UI 오브젝트 (MagazineView 2의 자식)
    public float detectionRange = 5f; // 감지 범위

    private GameObject player;
    private bool isPlayerInRange = false;

    private SaveMgr_KJS saveMgr;  // SaveMgr_KJS 인스턴스 참조
    private bool isUIActive = false; // UI의 현재 상태를 추적

    void Start()
    {
        // "Player" 태그가 붙은 오브젝트 찾기
        player = GameObject.FindGameObjectWithTag("Player");

        // SaveMgr_KJS 스크립트가 부착된 오브젝트 찾기
        saveMgr = FindObjectOfType<SaveMgr_KJS>();

        // "MagazineView 2" 오브젝트를 찾고, 그 자식 중 "Tool 2"를 찾아 할당
        GameObject magazineView = GameObject.Find("MagazineView 2");
        if (magazineView != null)
        {
            uiTool = magazineView.transform.Find("Tool 2")?.gameObject;
        }

        // UI가 정상적으로 할당되었으면 비활성화
        if (uiTool != null)
            uiTool.SetActive(false);
        else
            Debug.LogWarning("uiTool을 찾을 수 없습니다.");
    }

    void Update()
    {
        if (player != null)
        {
            // 플레이어와의 거리 계산
            float distance = Vector3.Distance(player.transform.position, transform.position);

            // 범위 안에 들어오면 isPlayerInRange를 true로 설정
            if (distance <= detectionRange)
            {
                isPlayerInRange = true;
            }
            else
            {
                isPlayerInRange = false;

                // 플레이어가 범위를 벗어나면 UI를 자동으로 비활성화
                if (isUIActive)
                {
                    ToggleUI(false);
                }
            }

            // 범위 내에 있을 때 V 키를 누르면 UI를 토글
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.V))
            {
                ToggleUI(!isUIActive);  // 현재 상태 반전
            }
        }
    }

    // UI 활성화/비활성화 처리
    private void ToggleUI(bool isActive)
    {
        if (uiTool != null)
        {
            uiTool.SetActive(isActive);
            isUIActive = isActive;
        }

        if (isActive)
        {
            // UI가 활성화될 때 SaveMgr_KJS의 LoadObjectsFromFile 호출
            saveMgr.CreateObjectsFromFile();
        }
    }
}
