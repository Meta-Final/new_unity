using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using Photon.Pun;

public class InventoryText_KJS : MonoBehaviour
{
    public List<string> inventoryPostIds = new List<string>(); // �ν����Ϳ��� �Ҵ��� postId ����Ʈ
    public string inventorySlotPrefabPath = "Prefabs/InventorySlotUI"; // Resources ���� �� ������ ���
    public Transform inventoryPanel;

    private List<GameObject> inventorySlots = new List<GameObject>(); // �κ��丮 ���� UI ���
    private Dictionary<string, H_PostInfo> postInfoDict = new Dictionary<string, H_PostInfo>(); // postId���� ������ �����͸� ������ ��ųʸ�

    private int previousItemCount = 0; // ���� inventoryPostIds ������ �����Ͽ� ��ȭ ����
    private string baseDirectory = Application.dataPath; // �⺻ ���� ���

    void Start()
    {
        StartCoroutine(LoadPostInfoFromFolders());
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

    // �� postId�� �������� JSON �����͸� �ε��Ͽ� PostInfoList�� �ʱ�ȭ�ϴ� �ڷ�ƾ
    private IEnumerator LoadPostInfoFromFolders()
    {
        if (Directory.Exists(baseDirectory))
        {
            string[] postDirectories = Directory.GetDirectories(baseDirectory);

            foreach (string postDirectory in postDirectories)
            {
                string jsonFilePath = Path.Combine(postDirectory, "thumbnail.json");

                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);
                    PostInfoList postInfoList = JsonUtility.FromJson<PostInfoList>(jsonContent);

                    // postData ����Ʈ�� ��ųʸ��� �����Ͽ� ������ ��ȸ�� �� �ֵ��� �غ�
                    foreach (H_PostInfo post in postInfoList.postData)
                    {
                        postInfoDict[post.postid] = post;
                    }
                }
            }

            // �ʱ� �κ��丮 ����
            UpdateInventorySlots();
        }
        yield return null;
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
            // Resources �������� ������ �ε� (PhotonNetwork.Instantiate������ ��ο� Resources�� �������� �ʽ��ϴ�)
            if (Resources.Load<GameObject>(inventorySlotPrefabPath) == null)
            {
                Debug.LogError("Inventory Slot UI �������� Resources �������� ã�� �� �����ϴ�. ��θ� Ȯ���ϼ���: " + inventorySlotPrefabPath);
                return;
            }

            // ���ο� ���� UI ���� (PhotonNetwork.Instantiate ���)
            GameObject newSlot = PhotonNetwork.Instantiate(inventorySlotPrefabPath, inventoryPanel.position, Quaternion.identity);
            newSlot.transform.SetParent(inventoryPanel, false); // inventoryPanel�� �ڽ����� ����
            inventorySlots.Add(newSlot);

            // �ش� postInfo�� thumburl�� ����Ͽ� �̹����� �ε��Ͽ� ���Կ� ����
            StartCoroutine(LoadAndSetThumbnail(postInfo, newSlot));
        }
        else
        {
            Debug.LogWarning("postId�� �ش��ϴ� �����͸� ã�� �� �����ϴ�: " + postId);
        }
    }

    // �̹��� ������ �ε��Ͽ� ���Կ� �����ϴ� �ڷ�ƾ
    private IEnumerator LoadAndSetThumbnail(H_PostInfo postInfo, GameObject slot)
    {
        string filePath = "file:///" + postInfo.thumburl; // �� postId�� ���� �� �̹��� ���� ���
        Debug.Log("baseDirectory: " + baseDirectory);
        Debug.Log("filePath: " + filePath);

        UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(filePath);
        Debug.LogError("�̹��� �ε� ����: " + textureRequest.error);
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