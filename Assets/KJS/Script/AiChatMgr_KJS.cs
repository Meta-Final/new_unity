using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가

public class UIManager : MonoBehaviour
{

    public TMP_InputField userInputField; // 사용자가 입력할 TMP 필드
    public TMP_Text chatResponseText;     // AI 응답을 표시할 TMP 텍스트
    //public APIManager apiManager;         // APIManager 인스턴스 참조

    public UnityEngine.UI.Button sendButton; // 전송 버튼 (Button은 TMP 전용이 아님)

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
            chatResponseText.text = "Thinking..."; // 응답 대기 중 메시지
            //apiManager.CallLLM(userMessage); // API 호출
        }
    }

    // APIManager가 호출해 응답을 표시하는 메서드
    public void UpdateChatResponse(string response)
    {
        chatResponseText.text = response; // AI 응답 표시
    }
}
