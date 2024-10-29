using UnityEngine;

public class PrefabManager_KJS : MonoBehaviour
{
    public GameObject uiTool; // UI ������Ʈ (MagazineView 2�� �ڽ�)
    public float detectionRange = 5f; // ���� ����

    private GameObject player;
    private bool isPlayerInRange = false;

    private SaveMgr_KJS saveMgr;  // SaveMgr_KJS �ν��Ͻ� ����
    private bool isUIActive = false; // UI�� ���� ���¸� ����

    void Start()
    {
        // "Player" �±װ� ���� ������Ʈ ã��
        player = GameObject.FindGameObjectWithTag("Player");

        // SaveMgr_KJS ��ũ��Ʈ�� ������ ������Ʈ ã��
        saveMgr = FindObjectOfType<SaveMgr_KJS>();

        // "MagazineView 2" ������Ʈ�� ã��, �� �ڽ� �� "Tool 2"�� ã�� �Ҵ�
        GameObject magazineView = GameObject.Find("MagazineView 2");
        if (magazineView != null)
        {
            uiTool = magazineView.transform.Find("Tool 2")?.gameObject;
        }

        // UI�� ���������� �Ҵ�Ǿ����� ��Ȱ��ȭ
        if (uiTool != null)
            uiTool.SetActive(false);
        else
            Debug.LogWarning("uiTool�� ã�� �� �����ϴ�.");
    }

    void Update()
    {
        if (player != null)
        {
            // �÷��̾���� �Ÿ� ���
            float distance = Vector3.Distance(player.transform.position, transform.position);

            // ���� �ȿ� ������ isPlayerInRange�� true�� ����
            if (distance <= detectionRange)
            {
                isPlayerInRange = true;
            }
            else
            {
                isPlayerInRange = false;

                // �÷��̾ ������ ����� UI�� �ڵ����� ��Ȱ��ȭ
                if (isUIActive)
                {
                    ToggleUI(false);
                }
            }

            // ���� ���� ���� �� V Ű�� ������ UI�� ���
            if (isPlayerInRange && Input.GetKeyDown(KeyCode.V))
            {
                ToggleUI(!isUIActive);  // ���� ���� ����
            }
        }
    }

    // UI Ȱ��ȭ/��Ȱ��ȭ ó��
    private void ToggleUI(bool isActive)
    {
        if (uiTool != null)
        {
            uiTool.SetActive(isActive);
            isUIActive = isActive;
        }

        if (isActive)
        {
            // UI�� Ȱ��ȭ�� �� SaveMgr_KJS�� LoadObjectsFromFile ȣ��
            saveMgr.LoadObjectsFromFile();
        }
    }
}
