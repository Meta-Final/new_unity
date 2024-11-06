using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefabSpawner : MonoBehaviour
{
    public GameObject prefab; // �� ��ư���� ������ ������
    private GameObject player; // �÷��̾� ������Ʈ ����

    private Button button; // ��ư ������Ʈ ����

    private void Awake()
    {
        // ��ư ������Ʈ ��������
        button = GetComponent<Button>();

        // ��ư Ŭ�� �̺�Ʈ�� �޼��� ���
        if (button != null)
        {
            button.onClick.AddListener(SpawnPrefab);
        }
        else
        {
            Debug.LogError("Button ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // Player �±׸� ���� ������Ʈ �ڵ� �˻�
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void SpawnPrefab()
    {
        if (prefab != null && player != null)
        {
            // �÷��̾� ��ġ���� (1, 1, 1) ������ ����
            Vector3 spawnPosition = player.transform.position + new Vector3(1, 0, 0);

            // ������ ����
            GameObject spawnedPrefab = Instantiate(prefab, spawnPosition, Quaternion.identity);

            // ������ �������� ũ�⸦ 15�� ����
            spawnedPrefab.transform.localScale = Vector3.one * 1;
        }
        else if (player == null)
        {
            Debug.LogWarning("Player ������Ʈ�� ã�� �� �����ϴ�.");
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} ��ư�� �������� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ��� ���� (�޸� ���� ����)
        if (button != null)
        {
            button.onClick.RemoveListener(SpawnPrefab);
        }
    }
}