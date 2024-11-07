using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonPrefabSpawner_KJS : MonoBehaviour
{
    public string objPath; // �� ��ư���� �ε��� .obj ���� ���
    private GameObject player; // �÷��̾� ������Ʈ ����
    private InventoryText_KJS inventoryText; // Player�� InventoryText_KJS ��ũ��Ʈ ����
    private Button button; // ��ư ������Ʈ ����
    private static int currentPostIdIndex = 0; // POSTID ����Ʈ �ε��� ����
    private string assignedPostId; // �� ��ư�� �Ҵ�� postId

    private void Awake()
    {
        // ��ư ������Ʈ ��������
        button = GetComponent<Button>();

        // Player �±׸� ���� ������Ʈ �ڵ� �˻�
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Player ������Ʈ���� InventoryText_KJS ��ũ��Ʈ ��������
            inventoryText = player.GetComponent<InventoryText_KJS>();

            if (inventoryText == null)
            {
                Debug.LogError("InventoryText_KJS ��ũ��Ʈ�� ã�� �� �����ϴ�.");
                return;
            }
        }
        else
        {
            Debug.LogError("Player �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // POSTID ����Ʈ���� ��ư ���� ������ ���� postId �Ҵ�
        AssignPostId();

        // ��ư Ŭ�� �̺�Ʈ�� �޼��� ���
        if (button != null)
        {
            button.onClick.AddListener(SpawnPrefabFromObj);
        }
        else
        {
            Debug.LogError("Button ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void AssignPostId()
    {
        List<string> postIdList = inventoryText.inventoryPostIds;

        // POSTID ����Ʈ�� ��� �ִ��� Ȯ��
        if (postIdList == null || postIdList.Count == 0)
        {
            Debug.LogWarning("POSTID ����Ʈ�� ����ֽ��ϴ�.");
            return;
        }

        // ���� �ε����� ����Ʈ ���� ���� �ִ��� Ȯ���ϰ� �Ҵ�
        if (currentPostIdIndex < postIdList.Count)
        {
            assignedPostId = postIdList[currentPostIdIndex];
            Debug.Log($"��ư ���� ������ ���� �Ҵ�� POSTID: {assignedPostId}");
            currentPostIdIndex++; // ���� ��ư�� ������ �� ���� POSTID ���
        }
        else
        {
            Debug.LogWarning("POSTID ����Ʈ�� ��� �׸��� �̹� ����߽��ϴ�.");
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    private void SpawnPrefabFromObj()
    {
        if (!string.IsNullOrEmpty(objPath) && player != null && !string.IsNullOrEmpty(assignedPostId))
        {
            // .obj ������ �����ϴ��� Ȯ��
            if (!File.Exists(objPath))
            {
                Debug.LogError("������ ��ο� .obj ������ �������� �ʽ��ϴ�.");
                return;
            }

            // OBJLoader�� �̿��Ͽ� .obj ���� �ε�
            GameObject loadedObject = new OBJLoader().Load(objPath);
            if (loadedObject == null)
            {
                Debug.LogError("OBJ ���� �ε忡 �����߽��ϴ�.");
                return;
            }

            // �÷��̾� ��ġ���� (1, 0, 0) ������ ����
            Vector3 spawnPosition = player.transform.position + new Vector3(1, 0, 0);
            loadedObject.transform.position = spawnPosition;

            // ������ �������� ũ�⸦ 1�� ����
            loadedObject.transform.localScale = Vector3.one;

            // PrefabManager_KJS ������Ʈ �߰� �� postId ����
            PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
            if (prefabManager == null)
            {
                // PrefabManager_KJS ������Ʈ�� ������ �߰�
                prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
            }
            prefabManager.postId = assignedPostId;  // ��ư�� �̸� �Ҵ�� postId ���
            Debug.Log($"PrefabManager_KJS�� �Ҵ�� postId: {prefabManager.postId}");

            // ������ �������� �ؽ�Ʈ ������Ʈ�� POSTID ǥ��
            Text prefabText = loadedObject.GetComponentInChildren<Text>();
            if (prefabText != null)
            {
                prefabText.text = assignedPostId;
            }
            else
            {
                Debug.LogWarning("������ �����տ� Text ������Ʈ�� �����ϴ�.");
            }

            // ������ ������Ʈ�� ��� Material�� URP�� Lit ���̴��� ����
            Shader urpLitShader = Shader.Find("Universal Render Pipeline/Lit");
            if (urpLitShader != null)
            {
                MeshRenderer[] meshRenderers = loadedObject.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer renderer in meshRenderers)
                {
                    foreach (Material mat in renderer.materials)
                    {
                        mat.shader = urpLitShader;
                    }
                }
            }
            else
            {
                Debug.LogError("URP�� Lit ���̴��� ã�� �� �����ϴ�. URP�� ������Ʈ�� ����Ǿ����� Ȯ���ϼ���.");
            }
        }
        else
        {
            Debug.LogWarning("Prefab�� ������ �� �����ϴ�. �ʼ� ���� ��Ұ� �����Ǿ����ϴ�.");
        }
    }

    private void OnDestroy()
    {
        // �̺�Ʈ ��� ���� (�޸� ���� ����)
        if (button != null)
        {
            button.onClick.RemoveListener(SpawnPrefabFromObj);
        }
    }
}