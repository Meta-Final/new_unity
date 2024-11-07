using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Firebase.Auth;
using Firebase.Storage;
using System.Text;
using System.Threading.Tasks;

public class Thumbnail_KJS : MonoBehaviour
{
    public Button saveButton;   // ���� ��ư (Inspector���� ����)
    public Image targetImage;   // ������ �̹����� �Ҵ�� Image ������Ʈ (Inspector���� �Ҵ�)
    public TMP_InputField postIdInputField; // ����� �Է��� ���� postId�� ������ InputField (Inspector���� �Ҵ�)

    private FirebaseAuth auth;
    private FirebaseStorage storage;

    private PostInfoList postInfoList = new PostInfoList();  // �� ����Ʈ�� �ʱ�ȭ

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        storage = FirebaseStorage.DefaultInstance;

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
    }

    // �̹����� JSON �����͸� �����ϴ� �޼���
    async void SaveImageAndData()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("Firebase�� �α��ε��� �ʾҽ��ϴ�.");
            return;
        }

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

        string userId = auth.CurrentUser.UserId;

        // �̹��� ���� �̸� ����
        string fileName = "UserImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        // Firebase Storage ���
        string imagePath = $"POST/{userId}/{fileName}";
        string jsonPath = $"POST/{userId}/Thumbnail/thumbnail.json";

        // �̹��� ���ε�
        await SaveImageToFirebase(targetImage.sprite.texture, imagePath);

        // postData�� �� �׸� �߰� �� �̹��� URL ����
        H_PostInfo newPost = new H_PostInfo
        {
            postid = postIdInputField.text,  // InputField�� �Էµ� �ؽ�Ʈ�� postId�� ���
            thumburl = imagePath             // Firebase Storage ��θ� thumburl�� ����
        };
        postInfoList.postData.Add(newPost);

        // JSON ������ ����
        SaveJsonToFirebase(jsonPath);
    }

    // Firebase Storage�� �̹��� ���ε�
    private async Task SaveImageToFirebase(Texture2D texture, string path)
    {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData == null)
        {
            Debug.LogError("�̹����� PNG �������� ���ڵ��ϴµ� �����߽��ϴ�.");
            return;
        }

        // Firebase Storage�� �̹��� ���ε�
        StorageReference storageRef = storage.GetReference(path);
        try
        {
            await storageRef.PutBytesAsync(pngData);
            Debug.Log($"�̹����� Firebase Storage�� {path} ��η� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Firebase Storage�� �̹��� ���� �� ���� �߻�: {e.Message}");
        }
    }

    // Firebase Storage�� JSON ������ ���ε�
    private async void SaveJsonToFirebase(string path)
    {
        try
        {
            // JSON �������� ����ȭ
            string json = JsonUtility.ToJson(postInfoList, true);  // �鿩���� ���� ����ȭ
            byte[] jsonData = Encoding.UTF8.GetBytes(json);

            // Firebase Storage�� JSON ���ε�
            StorageReference storageRef = storage.GetReference(path);
            await storageRef.PutBytesAsync(jsonData);
            Debug.Log($"JSON �����Ͱ� Firebase Storage�� {path} ��η� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Firebase Storage�� JSON ���� �� ���� �߻�: {e.Message}");
        }
    }
}
