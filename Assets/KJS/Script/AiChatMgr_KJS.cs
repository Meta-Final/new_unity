using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ReqRes;
using UnityEngine.Networking;

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
    private Coroutine typeTextCoroutine;

    // 하드코딩된 사용자 ID
    private const string userId = "user12345";

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

            if (userMessage == "오브젝트 만들어줘")
            {
                if (typeTextCoroutine != null)
                {
                    StopCoroutine(typeTextCoroutine);
                }

                string responseMessage = "알겠다 삐약! 지금부터 만들겠다 삐약!";
                typeTextCoroutine = StartCoroutine(TypeText(responseMessage, () =>
                {
                    StartCoroutine(ShowExtraUIWithDelay(1f, 9f));
                    CreateNewGameObject();
                }));
            }
            else
            {
                // AI API 호출
                apiManager.LLM(userMessage);
            }
        }
    }

    private IEnumerator ShowExtraUIWithDelay(float delayBeforeShow, float duration)
    {
        yield return new WaitForSeconds(delayBeforeShow); // 출력 후 1초 대기

        if (extraUI != null)
        {
            extraUI.SetActive(true);
            yield return new WaitForSeconds(duration);
            extraUI.SetActive(false);
        }
    }

    private void CreateNewGameObject()
    {
        // 새로운 게임 오브젝트 생성
        GameObject newObject = new GameObject("NewGameObject");
        newObject.transform.position = Vector3.zero;
        Debug.Log("새로운 오브젝트가 생성되었습니다: " + newObject.name);
    }

    private IEnumerator TypeText(string text, System.Action onComplete)
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

        // 모든 텍스트 출력이 완료된 후 콜백 실행
        onComplete?.Invoke();
    }
}
