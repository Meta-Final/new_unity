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
    public Button loadButton;   // �ε� ��ư (Inspector���� ����)
    public Button modifyButton; // ���� ��ư (Inspector���� ����)
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

        // �ε� ��ư ����
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(LoadImageAndData);  // �ε� ��ư Ŭ�� �� �̹����� �����͸� �ε�
        }
        else
        {
            Debug.LogError("loadButton�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // ���� ��ư ����
        if (modifyButton != null)
        {
            modifyButton.onClick.AddListener(ModifyAndSaveImage); // ���� ��ư Ŭ�� �� �̹����� �����ϰ� ����
        }
        else
        {
            Debug.LogError("modifyButton�� �Ҵ���� �ʾҽ��ϴ�.");
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
        if (targetImage == null || targetImage.sprite == null || targetImage.sprite.texture == null)
        {
            Debug.LogError("������ �̹����� �������� �ʾҰų� �ؽ�ó�� �����ϴ�.");
            return;
        }

        if (string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId�� �Է��ϼ���. postIdInputField�� ��� �ֽ��ϴ�.");
            return;
        }

        string fileName = "UserImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string imagePath = Path.Combine(saveDirectory, fileName);

        SaveImageToLocal(targetImage.sprite.texture, imagePath);

        H_PostInfo newPost = new H_PostInfo
        {
            postid = postIdInputField.text,
            thumburl = imagePath
        };

        postInfoList.postData.Add(newPost);
        SaveJsonToLocal();
    }

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

    private void SaveJsonToLocal()
    {
        try
        {
            string json = JsonUtility.ToJson(postInfoList, true);

            File.WriteAllText(jsonFilePath, json);
            Debug.Log($"JSON �����Ͱ� ���ÿ� {jsonFilePath} ��η� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"���ÿ� JSON ���� �� ���� �߻�: {e.Message}");
        }
    }

    // JSON ���Ͽ��� Ư�� postId�� �ش��ϴ� �����͸� �ε��ϰ� �̹����� ǥ��
    void LoadImageAndData()
    {
        if (string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId�� �Է��ϼ���. postIdInputField�� ��� �ֽ��ϴ�.");
            return;
        }

        // JSON ���� �б�
        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError($"JSON ������ {jsonFilePath} ��ο� �������� �ʽ��ϴ�.");
            return;
        }

        try
        {
            string json = File.ReadAllText(jsonFilePath);
            postInfoList = JsonUtility.FromJson<PostInfoList>(json);

            // �Էµ� postId�� ��ġ�ϴ� �׸� ã��
            H_PostInfo postInfo = postInfoList.postData.Find(post => post.postid == postIdInputField.text);
            if (postInfo == null)
            {
                Debug.LogError("�ش� postId�� ���� �׸��� ã�� �� �����ϴ�.");
                return;
            }

            // �ش� �̹��� �ε� �� ǥ��
            string imagePath = postInfo.thumburl;
            if (File.Exists(imagePath))
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(imageData))
                {
                    targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    Debug.Log($"�̹����� {imagePath} ��ο��� �ε�Ǿ����ϴ�.");
                }
                else
                {
                    Debug.LogError("�̹��� �����͸� �ε��ϴ� �� �����߽��ϴ�.");
                }
            }
            else
            {
                Debug.LogError("�ش� ��ο� �̹��� ������ �����ϴ�.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON ������ �ε��ϴ� ���� ������ �߻��߽��ϴ�: {e.Message}");
        }
    }

    // �̹����� �����ϰ� ���ÿ� �����ϴ� �޼���
    void ModifyAndSaveImage()
    {
        if (targetImage == null || targetImage.sprite == null || targetImage.sprite.texture == null)
        {
            Debug.LogError("������ �̹����� �������� �ʾҰų� �ؽ�ó�� �����ϴ�.");
            return;
        }

        // �ؽ�ó ���� �� ���� (���⼭�� ���� ������ ���÷� ��)
        Texture2D originalTexture = targetImage.sprite.texture;
        Texture2D modifiedTexture = new Texture2D(originalTexture.width, originalTexture.height);

        for (int y = 0; y < originalTexture.height; y++)
        {
            for (int x = 0; x < originalTexture.width; x++)
            {
                Color originalColor = originalTexture.GetPixel(x, y);
                Color modifiedColor = new Color(1 - originalColor.r, 1 - originalColor.g, 1 - originalColor.b); // ���� ����
                modifiedTexture.SetPixel(x, y, modifiedColor);
            }
        }
        modifiedTexture.Apply();

        // ������ �ؽ�ó�� �ٽ� ����
        string modifiedFileName = "ModifiedImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string modifiedImagePath = Path.Combine(saveDirectory, modifiedFileName);

        SaveImageToLocal(modifiedTexture, modifiedImagePath);

        Debug.Log($"������ �̹����� {modifiedImagePath} ��ο� ����Ǿ����ϴ�.");
    }
}