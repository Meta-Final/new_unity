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
    public Button saveButton;   // 저장 버튼 (Inspector에서 연결)
    public Image targetImage;   // 저장할 이미지가 할당된 Image 컴포넌트 (Inspector에서 할당)
    public TMP_InputField postIdInputField; // 사용자 입력을 통해 postId를 설정할 InputField (Inspector에서 할당)

    private FirebaseAuth auth;
    private FirebaseStorage storage;

    private PostInfoList postInfoList = new PostInfoList();  // 빈 리스트로 초기화

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        storage = FirebaseStorage.DefaultInstance;

        // 저장 버튼 설정
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveImageAndData);  // 저장 버튼 클릭 시 이미지와 데이터를 저장
        }
        else
        {
            Debug.LogError("saveButton이 할당되지 않았습니다.");
        }

        if (postIdInputField == null)
        {
            Debug.LogError("postIdInputField가 할당되지 않았습니다. Inspector에서 postIdInputField를 설정하세요.");
        }
    }

    // 이미지와 JSON 데이터를 저장하는 메서드
    async void SaveImageAndData()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("Firebase에 로그인되지 않았습니다.");
            return;
        }

        // targetImage 또는 targetImage.sprite가 null인지 확인
        if (targetImage == null || targetImage.sprite == null || targetImage.sprite.texture == null)
        {
            Debug.LogError("저장할 이미지가 설정되지 않았거나 텍스처가 없습니다.");
            return;
        }

        // postIdInputField가 비어 있는지 확인
        if (string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId를 입력하세요. postIdInputField가 비어 있습니다.");
            return;
        }

        string userId = auth.CurrentUser.UserId;

        // 이미지 파일 이름 생성
        string fileName = "UserImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";

        // Firebase Storage 경로
        string imagePath = $"POST/{userId}/{fileName}";
        string jsonPath = $"POST/{userId}/Thumbnail/thumbnail.json";

        // 이미지 업로드
        await SaveImageToFirebase(targetImage.sprite.texture, imagePath);

        // postData에 새 항목 추가 및 이미지 URL 설정
        H_PostInfo newPost = new H_PostInfo
        {
            postid = postIdInputField.text,  // InputField에 입력된 텍스트를 postId로 사용
            thumburl = imagePath             // Firebase Storage 경로를 thumburl에 저장
        };
        postInfoList.postData.Add(newPost);

        // JSON 데이터 저장
        SaveJsonToFirebase(jsonPath);
    }

    // Firebase Storage에 이미지 업로드
    private async Task SaveImageToFirebase(Texture2D texture, string path)
    {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData == null)
        {
            Debug.LogError("이미지를 PNG 형식으로 인코딩하는데 실패했습니다.");
            return;
        }

        // Firebase Storage에 이미지 업로드
        StorageReference storageRef = storage.GetReference(path);
        try
        {
            await storageRef.PutBytesAsync(pngData);
            Debug.Log($"이미지가 Firebase Storage에 {path} 경로로 저장되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Firebase Storage에 이미지 저장 중 오류 발생: {e.Message}");
        }
    }

    // Firebase Storage에 JSON 데이터 업로드
    private async void SaveJsonToFirebase(string path)
    {
        try
        {
            // JSON 형식으로 직렬화
            string json = JsonUtility.ToJson(postInfoList, true);  // 들여쓰기 포함 직렬화
            byte[] jsonData = Encoding.UTF8.GetBytes(json);

            // Firebase Storage에 JSON 업로드
            StorageReference storageRef = storage.GetReference(path);
            await storageRef.PutBytesAsync(jsonData);
            Debug.Log($"JSON 데이터가 Firebase Storage에 {path} 경로로 저장되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Firebase Storage에 JSON 저장 중 오류 발생: {e.Message}");
        }
    }
}
