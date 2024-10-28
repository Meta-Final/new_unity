using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI 사용
using System.IO;  // 파일 입출력 사용
#if UNITY_EDITOR
using UnityEditor;  // 파일 탐색기 사용을 위한 네임스페이스
#endif

[System.Serializable]

public class Thumbnail_KJS : MonoBehaviour
{
    public Button saveButton;  // 저장 버튼 (Inspector에서 연결)
    public string savePath = "C:/Users/haqqm/Desktop/postData.json";  // JSON 파일 저장 경로

    private PostInfoList postInfoList = new PostInfoList();  // 인스턴스 명시적 초기화

    void Start()
    {
        if (saveButton == null)
        {
            Debug.LogError("saveButton이 할당되지 않았습니다. Inspector에서 연결하세요.");
            return;
        }

        // 버튼 클릭 시 OpenFileExplorer 메서드 호출
        saveButton.onClick.AddListener(OpenFileExplorer);
    }

    void OpenFileExplorer()
    {
#if UNITY_EDITOR
        try
        {
            string path = EditorUtility.OpenFilePanel("Select Thumbnail", "", "png,jpg");

            if (!string.IsNullOrEmpty(path))
            {
                Debug.Log($"선택된 파일 경로: {path}");

                // 선택된 파일 경로를 포함한 게시물 정보 추가
                H_PostInfo newPost = new H_PostInfo { editorname = "Alice", thumburl = path };
                postInfoList.postData.Add(newPost);  // 게시물 목록에 추가

                // JSON 데이터 저장
                SaveToJson();
            }
            else
            {
                Debug.LogWarning("파일이 선택되지 않았습니다.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"파일 탐색기 열기 중 오류 발생: {e.Message}");
        }
#else
    Debug.LogWarning("파일 탐색기는 Unity Editor에서만 사용할 수 있습니다.");
#endif
    }

    void SaveToJson()
    {
        try
        {
            // JSON 형식으로 직렬화
            string json = JsonUtility.ToJson(postInfoList, true);  // true로 들여쓰기 포함

            // 파일 저장
            File.WriteAllText(savePath, json);

            Debug.Log($"JSON 데이터가 {savePath}에 저장되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON 저장 중 오류 발생: {e.Message}");
        }
    }
}
