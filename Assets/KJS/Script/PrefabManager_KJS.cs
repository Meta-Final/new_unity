using UnityEngine;

public class UIToggleOnProximity : MonoBehaviour
{
    public GameObject uiTool; // UI ������Ʈ �Ҵ�
    public float detectionRange = 5f; // ���� ����

    private GameObject player;
    private bool isPlayerInRange = false;

    private SaveMgr_KJS saveMgr;  // SaveMgr_KJS �ν��Ͻ� ����

    void Start()
    {
        // "Player" �±װ� ���� ������Ʈ ã��
        player = GameObject.FindGameObjectWithTag("Player");

        // SaveMgr_KJS ��ũ��Ʈ�� ������ ������Ʈ ã��
        saveMgr = FindObjectOfType<SaveMgr_KJS>();

        // "Tool"�̶�� �̸��� ������Ʈ�� ã�Ƽ� uiTool�� �Ҵ�
        if (uiTool == null)
        {
            uiTool = GameObject.Find("Tool");
        }

        // UI�� ���������� �Ҵ�Ǿ����� ��Ȱ��ȭ
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
                ToggleUI(true);  // �÷��̾ ���� �ȿ� ������ UI Ȱ��ȭ
                isPlayerInRange = true;
            }
            else if (distance > detectionRange && isPlayerInRange)
            {
                ToggleUI(false);  // �÷��̾ ������ ����� UI ��Ȱ��ȭ
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
            // UI�� Ȱ��ȭ�� �� SaveMgr_KJS�� LoadObjectsFromFile ȣ��
            saveMgr.LoadObjectsFromFile();
        }
    }
}