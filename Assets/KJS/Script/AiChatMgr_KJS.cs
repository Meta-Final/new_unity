using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AiChatMgr_KJS : MonoBehaviour
{
    // 싱글톤 인스턴스 선언
    public static AiChatMgr_KJS Instance { get; private set; }

    public TMP_InputField userInputField; // 사용자가 입력할 TMP 필드
    public TMP_Text chatResponseText;     // AI 응답을 표시할 TMP 텍스트
    public APIManager apiManager;         // APIManager 인스턴스 참조
    public Button sendButton;             // 전송 버튼
    public GameObject extraUI;            // 추가적인 UI (평소에는 꺼져 있음)

    private GameObject chatUI;            // MagazineView 안에 있는 Chat UI
    private GameObject toolUI;            // MagazineView 안에 있는 Tool UI
    private bool wasToolUIActive = false;

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

        // extraUI 초기 비활성화
        if (extraUI != null)
        {
            extraUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Extra UI가 할당되지 않았습니다.");
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
            chatResponseText.text = ""; // 텍스트 초기화
            userInputField.text = "";  // 입력 필드 초기화

            // 입력된 텍스트에 따라 다른 응답 처리
            if (userMessage.Trim() == "크리스마스이미지만들어줘")
            {
                StartCoroutine(ProcessMessage("이미지를 만들었다. 삐약!", "크리스마스이미지만들어줘"));
                apiManager.Cover(); // APIManager의 Cover 메서드 호출
            }
            else if (userMessage.Trim() == "작업한기사를오브젝트로만들어줘")
            {
                StartCoroutine(ProcessMessage("오브젝트를 만들었다. 삐약!", "작업한포스트기사를오브젝트로만들어줘"));
            }
            else if (userMessage.Trim() == "글쓰고싶어")
            {
                StartCoroutine(ProcessMessage("알겠다 삐약!", "글쓰고싶어"));
            }
            else if (userMessage.Trim() == "내가최근에스크랩한기사를알려줘")
            {
                StartCoroutine(ProcessMessage("너가 최근에 스크랩한 기사는 백화점, 연말이다 삐약!", "내가최근에스크랩한기사를알려줘"));
            }
            else
            {
                StartCoroutine(ProcessMessage(userMessage, ""));
            }
        }
    }

    // 코루틴: UI 활성화 -> 메시지 출력 -> 1초 대기 후 UI 비활성화
    private IEnumerator ProcessMessage(string responseText, string actionKey)
    {
        // 1. extraUI를 먼저 활성화
        if (extraUI != null)
        {
            extraUI.SetActive(true);
            Debug.Log("Extra UI 활성화");
        }

        // 2. 한 글자씩 텍스트 출력
        yield return StartCoroutine(TypeText(responseText));

        // 3. 텍스트 출력이 완료된 후 1초 대기
        yield return new WaitForSeconds(1f);

        // 4. extraUI 비활성화
        if (extraUI != null)
        {
            extraUI.SetActive(false);
            Debug.Log("Extra UI 비활성화");
        }

        if (actionKey == "글쓰고싶어" && toolUI != null)
        {
            toolUI.SetActive(true); // Tool UI 활성화

            // chatUI 비활성화
            if (chatUI != null)
            {
                chatUI.SetActive(false);
                Debug.Log("Chat UI 비활성화");
            }

            // 캐릭터 이동 비활성화
            PlayerMove playerMove = FindObjectOfType<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.EnableMoving(false); // 캐릭터 이동 비활성화
                Debug.Log("캐릭터 이동 비활성화");
            }
        }
    }

    // 텍스트를 한 글자씩 출력하는 Coroutine
    private IEnumerator TypeText(string text)
    {
        chatResponseText.text = ""; // 기존 텍스트 초기화
        foreach (char c in text)
        {
            chatResponseText.text += c; // 한 글자씩 추가
            yield return new WaitForSeconds(0.05f); // 딜레이 추가 (0.05초)
        }
    }
}