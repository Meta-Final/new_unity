using System.Collections;
using System.Collections.Generic;
using System.IO; // File.ReadAllBytes 사용을 위해 필요
using UnityEngine;
using UnityEngine.UI; // RawImage 및 Scroll View 사용
using TMPro; // TextMeshPro 사용

public class AiChatMgr_KJS : MonoBehaviour
{
    public TMP_InputField userInputField; // 사용자가 입력할 TMP 필드
    public TMP_Text chatResponseText;     // AI 응답을 표시할 TMP 텍스트
    public APIManager apiManager;         // APIManager 인스턴스 참조
    public Button sendButton;             // 전송 버튼
    public Transform scrollViewContent;   // Scroll View의 Content 참조

    void Start()
    {
        // 버튼 클릭 시 OnSendButtonClicked 호출
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    void OnSendButtonClicked()
    {
        string userMessage = userInputField.text; // 입력된 텍스트 가져오기

        if (!string.IsNullOrEmpty(userMessage))
        {
            chatResponseText.text = userMessage; // 사용자가 입력한 텍스트 표시
            userInputField.text = "";            // 입력 필드 초기화
            apiManager.CallLLM(userMessage);     // API 호출 (비동기 응답 대기)
        }
    }

    // APIManager에서 응답 텍스트를 받아와 표시하는 메서드
    public void UpdateChatResponse(string response)
    {
        Debug.Log($"UpdateChatResponse 호출됨: {response}"); // 디버그 로그 추가

        chatResponseText.text = response; // AI 응답 표시

        if (response == "text:/img.json")
        {
            LoadImageFromPath(@"C:\Users\Admin\Desktop\요리.jpg");
        }
    }

    // 로컬 이미지 파일을 로드하고 Scroll View에 표시하는 메서드
    private void LoadImageFromPath(string path)
    {
        Debug.Log($"LoadImageFromPath 호출됨: {path}"); // 디버그 로그 추가

        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2); // 임시로 2x2 텍스처 생성

            if (tex.LoadImage(fileData))
            {
                Debug.Log("이미지 로드 성공!");
                CreateImageGameObject(tex); // 이미지를 표시할 GameObject 생성
            }
            else
            {
                Debug.LogError("이미지 로드 실패!");
            }
        }
        else
        {
            Debug.LogError($"이미지 파일을 찾을 수 없습니다: {path}");
        }
    }

    // Scroll View의 Content에 Image GameObject를 생성하는 메서드
    private void CreateImageGameObject(Texture2D texture)
    {
        Debug.Log("CreateImageGameObject 호출됨"); // 디버그 로그 추가

        GameObject newImageObject = new GameObject("ChatImage", typeof(RectTransform), typeof(RawImage));
        newImageObject.transform.SetParent(scrollViewContent, false); // false로 설정해 로컬 위치 유지

        RectTransform rectTransform = newImageObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = new Vector2(300, 300);

        RawImage rawImage = newImageObject.GetComponent<RawImage>();
        rawImage.texture = texture;
    }
}