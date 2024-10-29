using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefabSpawner : MonoBehaviour
{
    public GameObject prefab; // �� ��ư���� ������ ������

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
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void SpawnPrefab()
    {
        if (prefab != null)
        {
            // (0, 0, 0) ��ġ�� ������ ����
            Instantiate(prefab, Vector3.zero, Quaternion.identity);
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