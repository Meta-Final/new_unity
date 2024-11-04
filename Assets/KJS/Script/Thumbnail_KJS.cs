using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;  // ���� �б�/����
using UnityEngine.Networking;  // �̹��� �ε� �� ���

public class Thumbnail_KJS : MonoBehaviour
{
    public Button spawnButton;  // ���� ��ư (Inspector���� ����)
    public Button saveButton;   // ���� ��ư (Inspector���� ����)
    public GameObject prefabToSpawn;  // ������ ������ (Inspector���� �Ҵ�)
    public Transform uiParent;  // ������ ������Ʈ�� �θ� (UI Panel ��)
    public string jsonFilePath = "C:/Users/haqqm/Desktop/postData.json";  // JSON ���� ���

    private PostInfoList postInfoList = new PostInfoList();  // ������ �����͸� ������ ����Ʈ

    void Start()
    {
        // ���� ��ư ����
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(SpawnObject);  // ���� ��ư Ŭ�� �� ������Ʈ ����
        }
        else
        {
            Debug.LogError("spawnButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // ���� ��ư ����
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveJsonData);  // ���� ��ư Ŭ�� �� JSON ����
        }
        else
        {
            Debug.LogError("saveButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    // �������� UI�� �ڽ����� �����ϰ� �����͸� �����ϴ� �޼���
    void SpawnObject()
    {
        if (prefabToSpawn != null && uiParent != null)
        {
            // UI�� �ڽ����� ������ �ν��Ͻ� ����
            GameObject spawnedObject = Instantiate(prefabToSpawn, uiParent);

            // RectTransform ������ �����Ͽ� �����տ� ����� ��ġ�� ũ�⸦ ����
            // spawnedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // ����
            // spawnedObject.GetComponent<RectTransform>().localScale = Vector3.one;       // ����

            // ������ ������Ʈ�� ������ ���� (�ؽ�Ʈ�� �̹���)
            Text editorNameText = spawnedObject.transform.Find("EditorName").GetComponent<Text>();
            Image thumbnailImage = spawnedObject.transform.Find("Thumbnail").GetComponent<Image>();

            // ���� ������ ���� (���Ƿ� �Ҵ�)
            string editorName = "�� ������";
            string imagePath = "file:///C:/Users/haqqm/Desktop/post/tiramisu.png";  // �ӽ� �̹��� ���

            if (editorNameText != null)
            {
                editorNameText.text = editorName;  // ������ �̸� ����
            }

            if (thumbnailImage != null)
            {
                StartCoroutine(LoadImage(imagePath, thumbnailImage));  // �̹��� �ε� �� ����
            }

            // ������ ������ ���� ����Ʈ�� �߰�
            H_PostInfo newPost = new H_PostInfo { editorname = editorName, thumburl = imagePath };
            postInfoList.postData.Add(newPost);  // H_PostInfo�� ���ϵ� ����Ʈ�� �߰�

            Debug.Log($"{editorName}�� ������Ʈ�� �����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("������ ������ �Ǵ� UI �θ� �������� �ʾҽ��ϴ�.");
        }
    }

    // URL ��ο��� �̹����� �ε��Ͽ� Image ������Ʈ�� �����ϴ� �ڷ�ƾ
    IEnumerator LoadImage(string url, Image image)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                image.sprite = sprite;
                Debug.Log("�̹����� ���������� �ε�Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogError($"�̹��� �ε� ����: {request.error}");
            }
        }
    }

    // ���� �Խù� �����͸� JSON ���Ϸ� �����ϴ� �޼���
    void SaveJsonData()
    {
        try
        {
            // JSON �������� ����ȭ
            string json = JsonUtility.ToJson(postInfoList, true);  // �鿩���� ���� ����ȭ

            // ���� ����
            File.WriteAllText(jsonFilePath, json);
            Debug.Log($"JSON �����Ͱ� {jsonFilePath}�� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON ���� �� ���� �߻�: {e.Message}");
        }
    }
}