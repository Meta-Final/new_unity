using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI 사용
using System.IO;  // 파일 입출력 사용
#if UNITY_EDITOR
using UnityEditor;  // 파일 탐색기 사용을 위한 네임스페이스
#endif

public class ImageBox_Thumbnail : MonoBehaviour
{
    private Button button;  // 버튼 컴포넌트 참조
    public Image targetImage;  // 선택된 이미지를 표시할 Image 컴포넌트 (Inspector에서 할당)

    void Start()
    {
        // 버튼 컴포넌트 가져오기
        button = GetComponent<Button>();

        if (button != null)
        {
            // 버튼 클릭 시 OpenFileExplorer 메서드 호출
            button.onClick.AddListener(OpenFileExplorer);
        }
        else
        {
            Debug.LogError("Button 컴포넌트를 찾을 수 없습니다.");
        }

        if (targetImage == null)
        {
            Debug.LogError("Target Image가 할당되지 않았습니다. Inspector에서 Image 컴포넌트를 연결하세요.");
        }
    }

    // 파일 탐색기를 여는 메서드
    void OpenFileExplorer()
    {
#if UNITY_EDITOR
        try
        {
            // 파일 탐색기 열기: 이미지 파일 선택
            string path = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

            if (!string.IsNullOrEmpty(path))
            {
                Debug.Log($"선택된 파일 경로: {path}");

                // 선택된 파일 경로의 이미지를 로드하여 Image 컴포넌트에 표시
                StartCoroutine(LoadImage(path));
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

    // 선택된 이미지 파일을 로드하여 Image 컴포넌트에 적용하는 코루틴
    IEnumerator LoadImage(string path)
    {
        // 파일 경로에서 이미지 데이터를 읽어오기
        byte[] imageData = File.ReadAllBytes(path);

        // Texture2D 생성
        Texture2D texture = new Texture2D(2, 2);  // 임시 크기 (로드 시 자동으로 조정됨)
        bool isLoaded = texture.LoadImage(imageData);  // 이미지 데이터 로드

        if (isLoaded)
        {
            // Texture2D를 Sprite로 변환
            Sprite newSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            // Image 컴포넌트에 새 Sprite 적용
            targetImage.sprite = newSprite;
            Debug.Log("이미지가 성공적으로 로드되었습니다.");
        }
        else
        {
            Debug.LogError("이미지 로드에 실패했습니다.");
        }

        yield return null;
    }
}
