using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Pun;

public class InventoryText_KJS : MonoBehaviourPun
{
    public List<string> inventoryPostIds = new List<string>(); // �ν����Ϳ��� �Ҵ��� postId ����Ʈ
    public string inventorySlotPrefabPath = "Prefabs/InventorySlotUI"; // Resources ���� �� ������ ���
    public Transform inventoryPanel;

    private List<GameObject> inventorySlots = new List<GameObject>(); // �κ��丮 ���� UI ���
    private Dictionary<string, H_PostInfo> postInfoDict = new Dictionary<string, H_PostInfo>(); // postId���� ������ �����͸� ������ ��ųʸ�

    private int previousItemCount = 0; // ���� inventoryPostIds ������ �����Ͽ� ��ȭ ����

    void Start()
    {
        StartCoroutine(LoadPostInfoFromJson());
    }

    void Update()
    {
        // inventoryPostIds�� ���� ��ȭ ����
        if (inventoryPostIds.Count != previousItemCount)
        {
            UpdateInventorySlots();
            previousItemCount = inventoryPostIds.Count;
        }
    }

    // JSON �����͸� �ε��Ͽ� PostInfoList�� �ʱ�ȭ�ϴ� �ڷ�ƾ
    private IEnumerator LoadPostInfoFromJson()
    {
        string jsonFilePath = "file:///C:/Users/Admin/Documents/GitHub/new_unity/Assets/KJS/UserInfo/thumbnail.json";

        UnityWebRequest request = UnityWebRequest.Get(jsonFilePath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            PostInfoList postInfoList = JsonUtility.FromJson<PostInfoList>(request.downloadHandler.text);

            // postData ����Ʈ�� ��ųʸ��� �����Ͽ� ������ ��ȸ�� �� �ֵ��� �غ�
            foreach (H_PostInfo post in postInfoList.postData)
            {
                postInfoDict[post.postid] = post;
            }

            // �ʱ� �κ��丮 ����
            UpdateInventorySlots();
        }
        else
        {
            Debug.LogError("JSON ������ �ҷ����� �� �����߽��ϴ�: " + request.error);
        }
    }

    // �κ��丮 ���� UI�� ������Ʈ�ϴ� �޼���
    private void UpdateInventorySlots()
    {
        // ���� ���� UI ����
        foreach (GameObject slot in inventorySlots)
        {
            Destroy(slot);
        }
        inventorySlots.Clear();

        // inventoryPostIds ����Ʈ�� �������� ���ο� ���� UI ����
        foreach (string postId in inventoryPostIds)
        {
            AddItemToInventory(postId);
        }
    }

    // Ư�� postId �������� �κ��丮�� �߰��ϴ� �޼���
    public void AddItemToInventory(string postId)
    {
        // �ش� postId�� ��ųʸ��� �ִ��� Ȯ��
        if (postInfoDict.TryGetValue(postId, out H_PostInfo postInfo))
        {
            // PhotonNetwork.Instantiate�� ��Ʈ��ũ�� ���� ������ ����
            GameObject newSlot = PhotonNetwork.Instantiate(inventorySlotPrefabPath, Vector3.zero, Quaternion.identity);
            if (newSlot == null)
            {
                Debug.LogError("Inventory Slot UI �������� Resources �������� ã�� �� �����ϴ�. ��θ� Ȯ���ϼ���: " + inventorySlotPrefabPath);
                return;
            }

            // ���� UI�� �κ��丮 �г� �Ʒ��� ��ġ
            newSlot.transform.SetParent(inventoryPanel, false);
            inventorySlots.Add(newSlot);

            // �ش� postInfo�� thumburl�� ����Ͽ� �̹����� �ε��Ͽ� ���Կ� ����
            StartCoroutine(LoadAndSetThumbnail(postInfo.thumburl, newSlot));
        }
        else
        {
            Debug.LogWarning("postId�� �ش��ϴ� �����͸� ã�� �� �����ϴ�: " + postId);
        }
    }

    // �̹��� ������ �ε��Ͽ� ���Կ� �����ϴ� �ڷ�ƾ
    private IEnumerator LoadAndSetThumbnail(string imagePath, GameObject slot)
    {
        string filePath = "file:///" + imagePath; // ���� ���� ���

        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(filePath);
        yield return textureRequest.SendWebRequest();

        if (textureRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(textureRequest);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            Image slotImage = slot.GetComponentInChildren<Image>();
            if (slotImage != null)
            {
                slotImage.sprite = sprite; // �̹��� ������Ʈ�� ��������Ʈ ����
            }
        }
        else
        {
            Debug.LogError("�̹��� �ε� ����: " + textureRequest.error);
        }
    }
}