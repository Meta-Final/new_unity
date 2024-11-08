using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Text;

public class Thumbnail_KJS : MonoBehaviour
{
    public Button saveButton;   // ���� ��ư (Inspector���� ����)
    public Image targetImage;   // ������ �̹����� �Ҵ�� Image ������Ʈ (Inspector���� �Ҵ�)
    public TMP_InputField postIdInputField; // ����� �Է��� ���� postId�� ������ InputField (Inspector���� �Ҵ�)

    private PostInfoList postInfoList = new PostInfoList();  // �� ����Ʈ�� �ʱ�ȭ
    private string saveDirectory = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo";
    private string jsonFilePath = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo\thumbnail.json";

    void Start()
    {
        // ���� ��ư ����
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveImageAndData);  // ���� ��ư Ŭ�� �� �̹����� �����͸� ����
        }
        else
        {
            Debug.LogError("saveButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (postIdInputField == null)
        {
            Debug.LogError("postIdInputField�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� postIdInputField�� �����ϼ���.");
        }

        EnsureDirectoryExists();
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
            Debug.Log($"Directory created at: {saveDirectory}");
        }
    }

    // �̹����� JSON �����͸� �����ϴ� �޼���
    void SaveImageAndData()
    {
        // targetImage �Ǵ� targetImage.sprite�� null���� Ȯ��
        if (targetImage == null || targetImage.sprite == null || targetImage.sprite.texture == null)
        {
            Debug.LogError("������ �̹����� �������� �ʾҰų� �ؽ�ó�� �����ϴ�.");
            return;
        }

        // postIdInputField�� ��� �ִ��� Ȯ��
        if (string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId�� �Է��ϼ���. postIdInputField�� ��� �ֽ��ϴ�.");
            return;
        }

        // �̹��� ���� �̸� ����
        string fileName = "UserImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string imagePath = Path.Combine(saveDirectory, fileName);

        // �̹��� ������ ���� ��ο� ����
        SaveImageToLocal(targetImage.sprite.texture, imagePath);

        // ���ο� postId ������ �߰�
        H_PostInfo newPost = new H_PostInfo
        {
            postid = postIdInputField.text,  // InputField�� �Էµ� �ؽ�Ʈ�� postId�� ���
            thumburl = imagePath             // ���� �̹��� ��θ� thumburl�� ����
        };

        // postData�� ���ο� �׸� �߰�
        postInfoList.postData.Add(newPost);

        // JSON ������ ����
        SaveJsonToLocal();
    }

    // ���� ���� �ý��ۿ� �̹��� ����
    private void SaveImageToLocal(Texture2D texture, string path)
    {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData == null)
        {
            Debug.LogError("�̹����� PNG �������� ���ڵ��ϴµ� �����߽��ϴ�.");
            return;
        }

        try
        {
            File.WriteAllBytes(path, pngData);
            Debug.Log($"�̹����� ���ÿ� {path} ��η� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"���ÿ� �̹��� ���� �� ���� �߻�: {e.Message}");
        }
    }

    // ���� ���� �ý��ۿ� JSON ������ ����
    private void SaveJsonToLocal()
    {
        try
        {
            // JSON �������� ����ȭ
            string json = JsonUtility.ToJson(postInfoList, true);  // �鿩���� ���� ����ȭ

            // ���� ��ο� JSON ���� ����
            File.WriteAllText(jsonFilePath, json);
            Debug.Log($"JSON �����Ͱ� ���ÿ� {jsonFilePath} ��η� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"���ÿ� JSON ���� �� ���� �߻�: {e.Message}");
        }
    }
}