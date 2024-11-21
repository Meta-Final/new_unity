using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AiChatMgr_KJS : MonoBehaviour
{
    public static AiChatMgr_KJS Instance { get; private set; }

    public TMP_InputField userInputField; // 사용자 입력 TMP 필드
    public TMP_Text chatResponseText;     // AI 응답 텍스트 TMP
    public APIManager apiManager;         // APIManager 인스턴스
    public Button sendButton;             // 전송 버튼
    public GameObject extraUI;            // 추가적인 UI (일시적 표시)

    private GameObject chatUI;            // MagazineView 안의 Chat UI
    private GameObject toolUI;            // MagazineView 안의 Tool UI
    private bool wasToolUIActive = false;

    private AudioSource audioSource;      // 오디오 소스
    public AudioClip typingSound;         // 타이핑 효과음
    private float lastSoundPlayTime = 0f; // 마지막으로 재생된 시간
    private float typingSoundDelay = 0.5f; // 타이핑 사운드 재생 간격 (초)

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        GameObject magazineView = GameObject.Find("MagazineView");
        if (magazineView != null)
        {
            chatUI = magazineView.transform.Find("Chat")?.gameObject;
            toolUI = magazineView.transform.Find("Tool")?.gameObject;

            if (chatUI != null)
            {
                chatUI.SetActive(false);
            }
            else
            {
                Debug.LogError("Chat UI not found within MagazineView.");
            }

            if (toolUI != null)
            {
                toolUI.SetActive(false);
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

        if (extraUI != null)
        {
            extraUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Extra UI가 설정되지 않았습니다.");
        }

        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    private void OnMouseDown()
    {
        if (chatUI != null)
        {
            chatUI.SetActive(true);
            Debug.Log("Chat UI 활성화되었습니다.");
        }
    }

    void OnSendButtonClicked()
    {
        string userMessage = userInputField.text;

        if (!string.IsNullOrEmpty(userMessage))
        {
            chatResponseText.text = "";
            userInputField.text = "";

            // 입력된 텍스트에 따라 다른 작업 처리
            if (userMessage.Trim() == "글쓰고싶어")
            {
                StartCoroutine(ProcessMessage("알겠다 삐약", "글쓰고싶어"));
            }
            else if (userMessage.Trim() == "크리스마스타임!")
            {
                StartCoroutine(ProcessMessage("크리스마스 분위기 전환 완료! 삐약!", "크리스마스타임!"));
                apiManager.Cover();
            }
            else if (userMessage.Trim() == "작업상태확인")
            {
                StartCoroutine(ProcessMessage("작업상태는 양호합니다. 삐약!", "작업상태확인"));
            }
            else
            {
                StartCoroutine(ProcessMessage(userMessage, ""));
            }
        }
    }

    private IEnumerator ProcessMessage(string responseText, string actionKey)
    {
        if (extraUI != null)
        {
            extraUI.SetActive(true);
            Debug.Log("Extra UI 활성화");
        }

        yield return StartCoroutine(TypeText(responseText));

        yield return new WaitForSeconds(1f);

        if (extraUI != null)
        {
            extraUI.SetActive(false);
            Debug.Log("Extra UI 비활성화");
        }

        if (actionKey == "글쓰고싶어" && toolUI != null)
        {
            toolUI.SetActive(true);

            if (chatUI != null)
            {
                chatUI.SetActive(false);
                Debug.Log("Chat UI 비활성화");
            }

            PlayerMove playerMove = FindObjectOfType<PlayerMove>();
            if (playerMove != null)
            {
                playerMove.EnableMoving(false);
                Debug.Log("플레이어 이동 비활성화");
            }
        }
    }

    private IEnumerator TypeText(string text)
    {
        chatResponseText.text = "";
        foreach (char c in text)
        {
            chatResponseText.text += c;

            // 사운드 재생 간격을 확인
            if (typingSound != null && audioSource != null)
            {
                if (Time.time - lastSoundPlayTime >= typingSoundDelay)
                {
                    audioSource.PlayOneShot(typingSound);
                    lastSoundPlayTime = Time.time; // 마지막 재생 시간 업데이트
                }
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}