using Dummiesman;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class ButtonPrefabSpawner2_KJS : MonoBehaviourPunCallbacks
{
    public string prefabResourcePath; // �� ��ư���� �ε��� �������� Resources ���
    private InventoryText_KJS inventoryText; // �θ��� InventoryText_KJS ��ũ��Ʈ ����
    private Button button; // ��ư ������Ʈ ����
    private static int currentPostIdIndex = 0; // POSTID ����Ʈ �ε��� ����
    private string assignedPostId; // �� ��ư�� �Ҵ�� postId

    private void Awake()
    {
        button = GetComponent<Button>();

        // �θ� ������Ʈ���� InventoryText_KJS ������Ʈ ã��
        inventoryText = GetComponentInParent<InventoryText_KJS>();

        if (inventoryText == null)
        {
            Debug.LogError("InventoryText_KJS ��ũ��Ʈ�� �θ� ������Ʈ���� ã�� �� �����ϴ�.");
            return;
        }

        AssignPostId();

        if (button != null)
        {
            button.onClick.AddListener(OnButtonClicked);
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

    private void OnButtonClicked()
    {
        // ��� Ŭ���̾�Ʈ���� �������� �����ϴ� RPC ȣ��
        photonView.RPC("SpawnPrefabFromResource_RPC", RpcTarget.AllBuffered, assignedPostId);
    }

    [PunRPC]
    private void SpawnPrefabFromResource_RPC(string postId)
    {
        if (string.IsNullOrEmpty(prefabResourcePath))
        {
            Debug.LogError("�������� Resources ��ΰ� �������� �ʾҽ��ϴ�.");
            return;
        }

        // Resources �������� ������ �ε�
        GameObject prefab = Resources.Load<GameObject>(prefabResourcePath);
        if (prefab == null)
        {
            Debug.LogError($"��� '{prefabResourcePath}'���� �������� ã�� �� �����ϴ�.");
            return;
        }

        // ������ �ν��Ͻ�ȭ
        GameObject loadedObject = Instantiate(prefab);

        // ������ ������Ʈ�� �±׸� "Item"���� ����
        loadedObject.tag = "Item";

        // �÷��̾� ��ġ���� ���� ���� Z ����(forward)���� 0.5��ŭ ������ ��ġ ���
        Vector3 spawnPosition = transform.parent.position + transform.parent.forward.normalized * 0.5f;
        loadedObject.transform.position = spawnPosition;
        loadedObject.transform.localScale = Vector3.one;

        // ������Ʈ�� �θ� ������Ʈ�� ���� ������ �ٶ󺸵��� ȸ�� ����
        loadedObject.transform.rotation = transform.parent.rotation;

        // PrefabManager_KJS ������Ʈ�� �߰��ϰ� postId ����
        PrefabManager_KJS prefabManager = loadedObject.GetComponent<PrefabManager_KJS>();
        if (prefabManager == null)
        {
            prefabManager = loadedObject.AddComponent<PrefabManager_KJS>();
        }
        prefabManager.postId = postId;
        Debug.Log($"PrefabManager_KJS�� �Ҵ�� postId: {prefabManager.postId}");

        Text prefabText = loadedObject.GetComponentInChildren<Text>();
        if (prefabText != null)
        {
            prefabText.text = postId;
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
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClicked);
        }
    }
}