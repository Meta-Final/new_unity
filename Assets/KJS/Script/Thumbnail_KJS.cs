using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Thumbnail_KJS : MonoBehaviour
{
    public Button spawnButton;  // ���� ��ư (Inspector���� ����)
    public Button saveButton;   // ���� ��ư (Inspector���� ����)
    public Transform uiParent;  // UI �г� (Ȱ��ȭ/��Ȱ��ȭ ��� ���)
    public Image targetImage;   // ������ �̹����� �Ҵ�� Image ������Ʈ (Inspector���� �Ҵ�)
    private string jsonFilePath = "C:/Users/Admin/Documents/GitHub/new_unity/Assets/KJS/UserInfo/thumbnail.json";  // JSON ���� ���� ���
    private string imageSavePath = "C:/Users/Admin/Documents/GitHub/new_unity/Assets/KJS/UserInfo";  // �̹��� ���� ���� ���

    private PostInfoList postInfoList = new PostInfoList();  // �� ����Ʈ�� �ʱ�ȭ

    void Start()
    {
        // ���� ��ư ����
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(ToggleUIVisibility);  // ���� ��ư Ŭ�� �� UI Ȱ��ȭ/��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("spawnButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // ���� ��ư ����
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveImageAndData);  // ���� ��ư Ŭ�� �� �̹����� �����͸� ����
        }
        else
        {
            Debug.LogError("saveButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    // UI�� Ȱ��ȭ/��Ȱ��ȭ�� ����ϴ� �޼���
    void ToggleUIVisibility()
    {
        if (uiParent != null)
        {
            uiParent.gameObject.SetActive(!uiParent.gameObject.activeSelf);  // UI �г� Ȱ��ȭ/��Ȱ��ȭ ���
            Debug.Log($"UI �г��� {(uiParent.gameObject.activeSelf ? "Ȱ��ȭ" : "��Ȱ��ȭ")}�Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("UI �θ� �������� �ʾҽ��ϴ�.");
        }
    }

    // �̹����� JSON �����͸� �����ϴ� �޼���
    void SaveImageAndData()
    {
        // targetImage �Ǵ� targetImage.sprite�� null���� Ȯ��
        if (targetImage == null)
        {
            Debug.LogError("������ Image ������Ʈ�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� targetImage�� �����ϼ���.");
            return;
        }

        if (targetImage.sprite == null)
        {
            Debug.LogError("targetImage�� �Ҵ�� Sprite�� �����ϴ�. �̹����� �����Ǿ����� Ȯ���ϼ���.");
            return;
        }

        if (targetImage.sprite.texture == null)
        {
            Debug.LogError("targetImage�� Sprite�� ������, Texture�� �����ϴ�.");
            return;
        }

        // ������ ������ ����
        if (!Directory.Exists(imageSavePath))
        {
            Directory.CreateDirectory(imageSavePath);
        }

        // �̹��� ���� �̸� ����
        string fileName = "UserImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Path.Combine(imageSavePath, fileName);

        // �̹��� ����
        SaveImageAsPNG(targetImage.sprite.texture, filePath);

        // postData�� �� �׸� �߰� �� ��� ����
        H_PostInfo newPost = new H_PostInfo
        {
            editorname = "DefaultName",  // �ʿ� �� �ٸ� �̸����� ���� ����
            thumburl = filePath  // ���� ��θ� thumburl�� ����
        };
        postInfoList.postData.Add(newPost);

        // JSON ������ ����
        SaveJsonData();
    }

    // Texture2D�� PNG ���Ϸ� �����ϴ� �޼���
    void SaveImageAsPNG(Texture2D texture, string filePath)
    {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
        {
            File.WriteAllBytes(filePath, pngData);
            Debug.Log($"�̹����� {filePath}�� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("�̹����� PNG �������� ���ڵ��ϴµ� �����߽��ϴ�.");
        }
    }

    // JSON ���Ͽ� �����͸� �����ϴ� �޼���
    void SaveJsonData()
    {
        try
        {
            // JSON �������� ����ȭ
            string json = JsonUtility.ToJson(postInfoList, true);  // �鿩���� ���� ����ȭ

            // JSON ���� ����
            File.WriteAllText(jsonFilePath, json);
            Debug.Log($"JSON �����Ͱ� {jsonFilePath}�� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON ���� �� ���� �߻�: {e.Message}");
        }
    }
}
