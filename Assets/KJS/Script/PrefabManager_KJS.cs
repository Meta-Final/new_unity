using UnityEngine;

public class UIToggleOnProximity : MonoBehaviour
{
    public GameObject uiTool; // UI 오브젝트 (MagazineView 2의 자식)
    public float detectionRange = 5f; // 감지 범위

    private GameObject player;
    private bool isPlayerInRange = false;

    private SaveMgr_KJS saveMgr;  // SaveMgr_KJS 인스턴스 참조

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
            float distance = Vector3.Distance(player.transform.position, transform.position);

            if (distance <= detectionRange && !isPlayerInRange)
            {
                ToggleUI(true);  // 플레이어가 범위 안에 들어오면 UI 활성화
                isPlayerInRange = true;
            }
            else if (distance > detectionRange && isPlayerInRange)
            {
                ToggleUI(false);  // 플레이어가 범위를 벗어나면 UI 비활성화
                isPlayerInRange = false;
            }
        }
    }

    private void ToggleUI(bool isActive)
    {
        if (uiTool != null)
            uiTool.SetActive(isActive);

        if (isActive)
        {
            // UI가 활성화될 때 SaveMgr_KJS의 LoadObjectsFromFile 호출
            saveMgr.LoadObjectsFromFile();
        }
    }
}