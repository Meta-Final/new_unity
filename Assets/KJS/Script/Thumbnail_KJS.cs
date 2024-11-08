using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Text;

public class Thumbnail_KJS : MonoBehaviour
{
    public Button saveButton;   // 저장 버튼 (Inspector에서 연결)
    public Button loadButton;   // 로드 버튼 (Inspector에서 연결)
    public Button modifyButton; // 수정 버튼 (Inspector에서 연결)
    public Image targetImage;   // 저장할 이미지가 할당된 Image 컴포넌트 (Inspector에서 할당)
    public TMP_InputField postIdInputField; // 사용자 입력을 통해 postId를 설정할 InputField (Inspector에서 할당)

    private PostInfoList postInfoList = new PostInfoList();  // 빈 리스트로 초기화
    private string saveDirectory = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo";
    private string jsonFilePath = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo\thumbnail.json";

    void Start()
    {
        // 저장 버튼 설정
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveImageAndData);  // 저장 버튼 클릭 시 이미지와 데이터를 저장
        }
        else
        {
            Debug.LogError("saveButton이 할당되지 않았습니다.");
        }

        // 로드 버튼 설정
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(LoadImageAndData);  // 로드 버튼 클릭 시 이미지와 데이터를 로드
        }
        else
        {
            Debug.LogError("loadButton이 할당되지 않았습니다.");
        }

        // 수정 버튼 설정
        if (modifyButton != null)
        {
            modifyButton.onClick.AddListener(ModifyAndSaveImage); // 수정 버튼 클릭 시 이미지를 수정하고 저장
        }
        else
        {
            Debug.LogError("modifyButton이 할당되지 않았습니다.");
        }

        if (postIdInputField == null)
        {
            Debug.LogError("postIdInputField가 할당되지 않았습니다. Inspector에서 postIdInputField를 설정하세요.");
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

    // 이미지와 JSON 데이터를 저장하는 메서드
    void SaveImageAndData()
    {
        if (targetImage == null || targetImage.sprite == null || targetImage.sprite.texture == null)
        {
            Debug.LogError("저장할 이미지가 설정되지 않았거나 텍스처가 없습니다.");
            return;
        }

        if (string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId를 입력하세요. postIdInputField가 비어 있습니다.");
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
            Debug.LogError("이미지를 PNG 형식으로 인코딩하는데 실패했습니다.");
            return;
        }

        try
        {
            File.WriteAllBytes(path, pngData);
            Debug.Log($"이미지가 로컬에 {path} 경로로 저장되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"로컬에 이미지 저장 중 오류 발생: {e.Message}");
        }
    }

    private void SaveJsonToLocal()
    {
        try
        {
            string json = JsonUtility.ToJson(postInfoList, true);

            File.WriteAllText(jsonFilePath, json);
            Debug.Log($"JSON 데이터가 로컬에 {jsonFilePath} 경로로 저장되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"로컬에 JSON 저장 중 오류 발생: {e.Message}");
        }
    }

    // JSON 파일에서 특정 postId에 해당하는 데이터를 로드하고 이미지를 표시
    void LoadImageAndData()
    {
        if (string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId를 입력하세요. postIdInputField가 비어 있습니다.");
            return;
        }

        // JSON 파일 읽기
        if (!File.Exists(jsonFilePath))
        {
            Debug.LogError($"JSON 파일이 {jsonFilePath} 경로에 존재하지 않습니다.");
            return;
        }

        try
        {
            string json = File.ReadAllText(jsonFilePath);
            postInfoList = JsonUtility.FromJson<PostInfoList>(json);

            // 입력된 postId와 일치하는 항목 찾기
            H_PostInfo postInfo = postInfoList.postData.Find(post => post.postid == postIdInputField.text);
            if (postInfo == null)
            {
                Debug.LogError("해당 postId를 가진 항목을 찾을 수 없습니다.");
                return;
            }

            // 해당 이미지 로드 및 표시
            string imagePath = postInfo.thumburl;
            if (File.Exists(imagePath))
            {
                byte[] imageData = File.ReadAllBytes(imagePath);
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(imageData))
                {
                    targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    Debug.Log($"이미지가 {imagePath} 경로에서 로드되었습니다.");
                }
                else
                {
                    Debug.LogError("이미지 데이터를 로드하는 데 실패했습니다.");
                }
            }
            else
            {
                Debug.LogError("해당 경로에 이미지 파일이 없습니다.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON 파일을 로드하는 동안 오류가 발생했습니다: {e.Message}");
        }
    }

    // 이미지를 수정하고 로컬에 저장하는 메서드
    void ModifyAndSaveImage()
    {
        if (targetImage == null || targetImage.sprite == null || targetImage.sprite.texture == null)
        {
            Debug.LogError("수정할 이미지가 설정되지 않았거나 텍스처가 없습니다.");
            return;
        }

        // 텍스처 복사 및 수정 (여기서는 색상 반전을 예시로 함)
        Texture2D originalTexture = targetImage.sprite.texture;
        Texture2D modifiedTexture = new Texture2D(originalTexture.width, originalTexture.height);

        for (int y = 0; y < originalTexture.height; y++)
        {
            for (int x = 0; x < originalTexture.width; x++)
            {
                Color originalColor = originalTexture.GetPixel(x, y);
                Color modifiedColor = new Color(1 - originalColor.r, 1 - originalColor.g, 1 - originalColor.b); // 색상 반전
                modifiedTexture.SetPixel(x, y, modifiedColor);
            }
        }
        modifiedTexture.Apply();

        // 수정된 텍스처를 다시 저장
        string modifiedFileName = "ModifiedImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string modifiedImagePath = Path.Combine(saveDirectory, modifiedFileName);

        SaveImageToLocal(modifiedTexture, modifiedImagePath);

        Debug.Log($"수정된 이미지가 {modifiedImagePath} 경로에 저장되었습니다.");
    }
}