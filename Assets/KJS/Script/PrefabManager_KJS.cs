using UnityEngine;

public class UIToggleOnProximity : MonoBehaviour
{
    public GameObject uiTool; // UI 오브젝트 할당
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

        // "Tool"이라는 이름의 오브젝트를 찾아서 uiTool에 할당
        if (uiTool == null)
        {
            uiTool = GameObject.Find("Tool");
        }

        // UI가 정상적으로 할당되었으면 비활성화
        if (uiTool != null)
            uiTool.SetActive(false);
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