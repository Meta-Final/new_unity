using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI 사용
using TMPro; // TextMeshPro 사용

public class AiChatMgr_KJS : MonoBehaviour
{
    // 싱글톤 인스턴스 선언
    public static AiChatMgr_KJS Instance { get; private set; }

    public TMP_InputField userInputField; // 사용자가 입력할 TMP 필드
    public TMP_Text chatResponseText;     // AI 응답을 표시할 TMP 텍스트
    public APIManager apiManager;         // APIManager 인스턴스 참조
    public Button sendButton;             // 전송 버튼

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 중복된 오브젝트를 제거
        }
        else
        {
            Instance = this; // 인스턴스 할당
        }
    }

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
            userInputField.text = ""; // 입력 필드 초기화

            // 입력된 텍스트에 따라 다른 응답 처리
            if (userMessage.Trim() == "이미지 만들어줘")
            {
                UpdateChatResponse("이미지 만들어줘"); // 기존 메서드 호출
                apiManager.Cover(); // APIManager의 Cover 메서드 호출
            }
            else if (userMessage.Trim() == "작업한 포스트 내용을 오브젝트로 만들어줘")
            {
                UpdateChatResponse("작업한 포스트 내용을 오브젝트로 만들어줘"); // 새로운 조건 처리
            }
            else
            {
                // apiManager.CallLLM(userMessage); // API 호출 (비동기 응답 대기)
            }
        }
    }

    // APIManager에서 응답 텍스트를 받아와 표시하는 메서드
    public void UpdateChatResponse(string response)
    {
        try
        {
            Debug.Log($"UpdateChatResponse 호출됨: {response}"); // 응답 로그

            // 요리 이미지 관련 처리
            if (response == "이미지 만들어줘")
            {
                chatResponseText.text = "이미지를 만들었습니다.";
                Debug.Log("요리 이미지 생성 요청에 대한 응답입니다.");
            }
            else if (response == "작업한 포스트 내용을 오브젝트로 만들어줘")
            {
                chatResponseText.text = "오브젝트를 만들었습니다.";
                Debug.Log("작업한 포스트 내용을 오브젝트로 만드는 요청에 대한 응답입니다.");
            }
            else
            {
                // 다른 응답의 경우, 전달된 응답을 그대로 표시
                chatResponseText.text = response;
                Debug.LogWarning("조건에 맞는 응답이 아닙니다.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"UpdateChatResponse 중 오류 발생: {e.Message}");
        }
    }
}
