using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Thumbnail_KJS : MonoBehaviour
{
    public Button saveButton;   // 저장 버튼 (Inspector에서 연결)
    public Image targetImage;   // 저장할 이미지가 할당된 Image 컴포넌트 (Inspector에서 할당)
    public TMP_InputField postIdInputField; // 사용자 입력을 통해 postId를 설정할 InputField (Inspector에서 할당)

    private string jsonFilePath = "C:/Users/Admin/Documents/GitHub/new_unity/Assets/KJS/UserInfo/thumbnail.json";  // JSON 파일 저장 경로
    private string imageSavePath = "C:/Users/Admin/Documents/GitHub/new_unity/Assets/KJS/UserInfo";  // 이미지 저장 폴더 경로

    private PostInfoList postInfoList = new PostInfoList();  // 빈 리스트로 초기화

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

        if (postIdInputField == null)
        {
            Debug.LogError("postIdInputField가 할당되지 않았습니다. Inspector에서 postIdInputField를 설정하세요.");
        }
    }

    // 스폰 버튼 클릭 시 호출되는 메서드 (현재 빈 메서드로 유지)
    void OnSpawnButtonClicked()
    {
        // 필요 시 스폰 버튼 클릭 시 수행할 동작을 추가할 수 있습니다.
        Debug.Log("스폰 버튼이 클릭되었습니다.");
    }

    // 이미지와 JSON 데이터를 저장하는 메서드
    void SaveImageAndData()
    {
        // targetImage 또는 targetImage.sprite가 null인지 확인
        if (targetImage == null)
        {
            Debug.LogError("저장할 Image 컴포넌트가 할당되지 않았습니다. Inspector에서 targetImage를 설정하세요.");
            return;
        }

        if (targetImage.sprite == null)
        {
            Debug.LogError("targetImage에 할당된 Sprite가 없습니다. 이미지가 지정되었는지 확인하세요.");
            return;
        }

        if (targetImage.sprite.texture == null)
        {
            Debug.LogError("targetImage에 Sprite가 있지만, Texture가 없습니다.");
            return;
        }

        // postIdInputField가 비어 있는지 확인
        if (postIdInputField == null || string.IsNullOrWhiteSpace(postIdInputField.text))
        {
            Debug.LogError("postId를 입력하세요. postIdInputField가 비어 있거나 할당되지 않았습니다.");
            return;
        }

        // 폴더가 없으면 생성
        if (!Directory.Exists(imageSavePath))
        {
            Directory.CreateDirectory(imageSavePath);
        }

        // 이미지 파일 이름 생성
        string fileName = "UserImage_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = Path.Combine(imageSavePath, fileName);

        // 이미지 저장
        SaveImageAsPNG(targetImage.sprite.texture, filePath);

        // postData에 새 항목 추가 및 경로 설정
        H_PostInfo newPost = new H_PostInfo
        {
            postid = postIdInputField.text,  // InputField에 입력된 텍스트를 postId로 사용
            thumburl = filePath  // 파일 경로를 thumburl에 저장
        };
        postInfoList.postData.Add(newPost);

        // JSON 데이터 저장
        SaveJsonData();
    }

    // Texture2D를 PNG 파일로 저장하는 메서드
    void SaveImageAsPNG(Texture2D texture, string filePath)
    {
        byte[] pngData = texture.EncodeToPNG();
        if (pngData != null)
        {
            File.WriteAllBytes(filePath, pngData);
            Debug.Log($"이미지가 {filePath}에 저장되었습니다.");
        }
        else
        {
            Debug.LogError("이미지를 PNG 형식으로 인코딩하는데 실패했습니다.");
        }
    }

    // JSON 파일에 데이터를 저장하는 메서드
    void SaveJsonData()
    {
        try
        {
            // JSON 형식으로 직렬화
            string json = JsonUtility.ToJson(postInfoList, true);  // 들여쓰기 포함 직렬화

            // JSON 파일 저장
            File.WriteAllText(jsonFilePath, json);
            Debug.Log($"JSON 데이터가 {jsonFilePath}에 저장되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON 저장 중 오류 발생: {e.Message}");
        }
    }
}
