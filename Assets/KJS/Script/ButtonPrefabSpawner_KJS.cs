using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonPrefabSpawner_KJS : MonoBehaviour
{
    public string objPath; // �� ��ư���� �ε��� .obj ���� ���
    public string texturePath; // �� ��ư���� �ε��� �ؽ�ó ���� ��� (��: .png, .jpg ����)
    public GameObject player; // �÷��̾� ������Ʈ ����
    private InventoryText_KJS inventoryText; // Player�� InventoryText_KJS ��ũ��Ʈ ����
    private Button button; // ��ư ������Ʈ ����
    private static int currentPostIdIndex = 0; // POSTID ����Ʈ �ε��� ����
    private string assignedPostId; // �� ��ư�� �Ҵ�� postId

    private void Awake()
    {
        button = GetComponent<Button>();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
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

        AssignPostId();

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

        if (postIdList == null || postIdList.Count == 0)
        {
            Debug.LogWarning("POSTID ����Ʈ�� ����ֽ��ϴ�.");
            return;
        }

        if (currentPostIdIndex < postIdList.Count)
        {
            assignedPostId = postIdList[currentPostIdIndex];
            Debug.Log($"��ư ���� ������ ���� �Ҵ�� POSTID: {assignedPostId}");
            currentPostIdIndex++;
        }
        else
        {
            Debug.LogWarning("POSTID ����Ʈ�� ��� �׸��� �̹� ����߽��ϴ�.");
        }
    }

    private void SpawnPrefabFromObj()
    {
        if (!string.IsNullOrEmpty(objPath) && player != null && !string.IsNullOrEmpty(assignedPostId))
        {
            if (!File.Exists(objPath))
            {
                Debug.LogError("������ ��ο� .obj ������ �������� �ʽ��ϴ�.");
                return;
            }

            GameObject loadedObject = new OBJLoader().Load(objPath);
            if (loadedObject == null)
            {
                Debug.LogError("OBJ ���� �ε忡 �����߽��ϴ�.");
                return;
            }

            // ������ ������Ʈ�� �±׸� "Item"���� ����
            loadedObject.tag = "Item";

            // �÷��̾� ��ġ���� ���� ���� Z ����(forward)���� 0.5��ŭ ������ ��ġ ���
            Vector3 spawnPosition = player.transform.position + player.transform.forward.normalized * 0.5f;
            loadedObject.transform.position = spawnPosition;
            loadedObject.transform.localScale = Vector3.one;

            // ������Ʈ�� �÷��̾�� ���� ������ �ٶ󺸵��� ȸ�� ���� (�ʿ信 ���� ����)
            loadedObject.transform.rotation = player.transform.rotation;

            PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
            if (prefabManager == null)
            {
                prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
            }
            prefabManager.postId = assignedPostId;
            Debug.Log($"PrefabManager_KJS�� �Ҵ�� postId: {prefabManager.postId}");

            Text prefabText = loadedObject.GetComponentInChildren<Text>();
            if (prefabText != null)
            {
                prefabText.text = assignedPostId;
            }
            else
            {
                Debug.LogWarning("������ �����տ� Text ������Ʈ�� �����ϴ�.");
            }

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

            // �ؽ�ó ��ΰ� �����Ǿ����� �ؽ�ó�� �ε��Ͽ� ����
            if (!string.IsNullOrEmpty(texturePath) && File.Exists(texturePath))
            {
                byte[] fileData = File.ReadAllBytes(texturePath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(fileData)) // �ؽ�ó �ε� ����
                {
                    foreach (MeshRenderer renderer in loadedObject.GetComponentsInChildren<MeshRenderer>())
                    {
                        foreach (Material mat in renderer.materials)
                        {
                            mat.mainTexture = texture; // �ؽ�ó �Ҵ�
                        }
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(SpawnPrefabFromObj);
        }
    }
}