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

    private GameObject chatUI;            // MagazineView 안에 있는 Chat UI
    private GameObject toolUI;            // MagazineView 안에 있는 Tool UI

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
        // "MagazineView" 오브젝트 안에 있는 "Chat"과 "Tool" UI를 찾기
        GameObject magazineView = GameObject.Find("MagazineView");
        if (magazineView != null)
        {
            chatUI = magazineView.transform.Find("Chat")?.gameObject;
            toolUI = magazineView.transform.Find("Tool")?.gameObject;

            if (chatUI != null)
            {
                chatUI.SetActive(false); // Chat UI를 비활성화 상태로 초기화
            }
            else
            {
                Debug.LogError("Chat UI not found within MagazineView.");
            }

            if (toolUI != null)
            {
                toolUI.SetActive(false); // Tool UI를 비활성화 상태로 초기화
            }
            else
            {
                Debug.LogError("Tool UI not found within MagazineView.");
            }
        }
        else
        {
            Debug.LogError("MagazineView object not found in the scene.");
        }

        // 버튼 클릭 시 OnSendButtonClicked 호출
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    // 오브젝트 클릭 시 Chat UI 활성화
    private void OnMouseDown()
    {
        if (chatUI != null)
        {
            chatUI.SetActive(true); // Chat UI 활성화
            Debug.Log("Chat UI가 활성화되었습니다.");
        }
    }

    void OnSendButtonClicked()
    {
        string userMessage = userInputField.text; // 입력된 텍스트 가져오기

        if (!string.IsNullOrEmpty(userMessage))
        {
            chatResponseText.text = userMessage; // 사용자가 입력한 텍스트 표시
            userInputField.text = ""; // 입력 필드 초기화

            // 입력된 텍스트에 따라 다른 응답 처리
            if (userMessage.Trim() == "크리스마스이미지만들어줘")
            {
                UpdateChatResponse("크리스마스이미지만들어줘"); // 기존 메서드 호출
                apiManager.Cover(); // APIManager의 Cover 메서드 호출
            }
            else if (userMessage.Trim() == "작업한기사를오브젝트로만들어줘")
            {
                UpdateChatResponse("작업한포스트기사를오브젝트로만들어줘"); // 새로운 조건 처리
            }
            else if (userMessage.Trim() == "글쓰고싶어")
            {
                UpdateChatResponse("글쓰고싶어");
            }
            else if (userMessage.Trim() == "내가최근에스크랩한기사를알려줘")
            {
                UpdateChatResponse("내가최근에스크랩한기사를알려줘");
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
            if (response == "크리스마스이미지만들어줘")
            {
                chatResponseText.text = "이미지를 만들었다. 삐약!";
                Debug.Log("요리 이미지 생성 요청에 대한 응답입니다.");
            }
            else if (response == "작업한기사를오브젝트로만들어줘")
            {
                chatResponseText.text = "오브젝트를 만들었다. 삐약!";
                Debug.Log("작업한 포스트 내용을 오브젝트로 만드는 요청에 대한 응답입니다.");
            }
            else if (response == "글쓰고싶어")
            {
                chatResponseText.text = "알겠다 삐약!";
                Debug.Log("글쓰기 요청에 대한 응답입니다.");

                // Tool UI를 활성화
                if (toolUI != null)
                {
                    toolUI.SetActive(true);
                }
                else
                {
                    Debug.LogError("Tool UI is not assigned.");
                }
            }
            else if (response == "내가최근에스크랩한기사를알려줘")
            {
                // 최근 스크랩한 기사 응답 처리
                chatResponseText.text = "너가 최근에 스크랩한 기사는 백화점, 연말이다 삐약!";
                Debug.Log("최근 스크랩한 기사 요청에 대한 응답입니다.");
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