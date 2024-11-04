using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;  // 파일 읽기/쓰기
using UnityEngine.Networking;  // 이미지 로드 시 사용

public class Thumbnail_KJS : MonoBehaviour
{
    public Button spawnButton;  // 스폰 버튼 (Inspector에서 연결)
    public Button saveButton;   // 저장 버튼 (Inspector에서 연결)
    public GameObject prefabToSpawn;  // 생성할 프리팹 (Inspector에서 할당)
    public Transform uiParent;  // 생성된 오브젝트의 부모 (UI Panel 등)
    public string jsonFilePath = "C:/Users/haqqm/Desktop/postData.json";  // JSON 파일 경로

    private PostInfoList postInfoList = new PostInfoList();  // 생성된 데이터를 저장할 리스트

    void Start()
    {
        // 스폰 버튼 설정
        if (spawnButton != null)
        {
            spawnButton.onClick.AddListener(SpawnObject);  // 스폰 버튼 클릭 시 오브젝트 생성
        }
        else
        {
            Debug.LogError("spawnButton이 할당되지 않았습니다.");
        }

        // 저장 버튼 설정
        if (saveButton != null)
        {
            saveButton.onClick.AddListener(SaveJsonData);  // 저장 버튼 클릭 시 JSON 저장
        }
        else
        {
            Debug.LogError("saveButton이 할당되지 않았습니다.");
        }
    }

    // 프리팹을 UI의 자식으로 생성하고 데이터를 적용하는 메서드
    void SpawnObject()
    {
        if (prefabToSpawn != null && uiParent != null)
        {
            // UI의 자식으로 프리팹 인스턴스 생성
            GameObject spawnedObject = Instantiate(prefabToSpawn, uiParent);

            // RectTransform 설정을 제거하여 프리팹에 저장된 위치와 크기를 유지
            // spawnedObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // 제거
            // spawnedObject.GetComponent<RectTransform>().localScale = Vector3.one;       // 제거

            // 생성된 오브젝트에 데이터 적용 (텍스트와 이미지)
            Text editorNameText = spawnedObject.transform.Find("EditorName").GetComponent<Text>();
            Image thumbnailImage = spawnedObject.transform.Find("Thumbnail").GetComponent<Image>();

            // 예시 데이터 생성 (임의로 할당)
            string editorName = "새 에디터";
            string imagePath = "file:///C:/Users/haqqm/Desktop/post/tiramisu.png";  // 임시 이미지 경로

            if (editorNameText != null)
            {
                editorNameText.text = editorName;  // 에디터 이름 설정
            }

            if (thumbnailImage != null)
            {
                StartCoroutine(LoadImage(imagePath, thumbnailImage));  // 이미지 로드 및 적용
            }

            // 데이터 저장을 위한 리스트에 추가
            H_PostInfo newPost = new H_PostInfo { editorname = editorName, thumburl = imagePath };
            postInfoList.postData.Add(newPost);  // H_PostInfo로 통일된 리스트에 추가

            Debug.Log($"{editorName}의 오브젝트가 생성되었습니다.");
        }
        else
        {
            Debug.LogWarning("생성할 프리팹 또는 UI 부모가 설정되지 않았습니다.");
        }
    }

    // URL 경로에서 이미지를 로드하여 Image 컴포넌트에 적용하는 코루틴
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
                Debug.Log("이미지가 성공적으로 로드되었습니다.");
            }
            else
            {
                Debug.LogError($"이미지 로드 실패: {request.error}");
            }
        }
    }

    // 현재 게시물 데이터를 JSON 파일로 저장하는 메서드
    void SaveJsonData()
    {
        try
        {
            // JSON 형식으로 직렬화
            string json = JsonUtility.ToJson(postInfoList, true);  // 들여쓰기 포함 직렬화

            // 파일 저장
            File.WriteAllText(jsonFilePath, json);
            Debug.Log($"JSON 데이터가 {jsonFilePath}에 저장되었습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON 저장 중 오류 발생: {e.Message}");
        }
    }
}