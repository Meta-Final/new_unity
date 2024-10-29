using System.Collections;
using UnityEngine;
using UnityEngine.UI; // RawImage 사용
using TMPro; // TextMeshPro 사용

public class AiChatMgr_KJS : MonoBehaviour
{
    // 싱글톤 인스턴스 선언
    public static AiChatMgr_KJS Instance { get; private set; }

    public TMP_InputField userInputField; // 사용자가 입력할 TMP 필드
    public TMP_Text chatResponseText;     // AI 응답을 표시할 TMP 텍스트
    public APIManager apiManager;         // APIManager 인스턴스 참조
    public Button sendButton;             // 전송 버튼
    public RawImage targetRawImage;       // Inspector에서 할당된 RawImage 참조
    public Texture2D inspectorTexture;    // Inspector에서 할당된 이미지(Texture2D)
    public Transform scrollViewContent;   // Scroll View의 Content 참조
    public GameObject prefabObject;       // 생성할 프리팹 참조 (Inspector에서 할당)

    public float displayDuration = 3f; // 몇 초 동안 이미지를 표시할지 설정

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
            if (userMessage.Trim() == "요리 이미지 만들어줘")
            {
                UpdateChatResponse("요리 이미지 만들어줘"); // 기존 메서드 호출
            }
            else if (userMessage.Trim() == "작업한 포스트 내용을 오브젝트로 만들어줘")
            {
                UpdateChatResponse("작업한 포스트 내용을 오브젝트로 만들어줘"); // 새로운 조건 처리
            }
            else
            {
                apiManager.CallLLM(userMessage); // API 호출 (비동기 응답 대기)
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
            if (response == "요리 이미지 만들어줘")
            {
                chatResponseText.text = "이미지를 만들었습니다.";

                if (targetRawImage == null)
                {
                    Debug.LogError("targetRawImage가 Inspector에 연결되지 않았습니다.");
                    return;
                }

                if (inspectorTexture != null)
                {
                    Debug.Log("Inspector에서 할당된 이미지를 표시합니다.");

                    targetRawImage.texture = inspectorTexture; // RawImage에 텍스처 적용
                    targetRawImage.gameObject.SetActive(true); // RawImage 활성화

                    StartCoroutine(HideImageAfterDelay(displayDuration)); // 일정 시간 후 비활성화 코루틴 시작
                }
                else
                {
                    Debug.LogError("inspectorTexture가 Inspector에서 할당되지 않았습니다.");
                }
            }
            // 새로운 조건: 오브젝트 생성
            else if (response == "작업한 포스트 내용을 오브젝트로 만들어줘")
            {
                chatResponseText.text = "오브젝트를 만들었습니다.";
                Debug.Log("오브젝트를 생성합니다.");

                // 오브젝트를 (-41.9, 19.5, -27) 위치에 생성하고 스케일을 25로 설정
                CreateObjectAtPositionAndScale(new Vector3(-41.9f, 19.5f, -27f), new Vector3(25f, 25f, 25f));
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

    // 지정된 위치와 스케일로 프리팹 오브젝트를 생성하는 메서드
    private void CreateObjectAtPositionAndScale(Vector3 position, Vector3 scale)
    {
        if (prefabObject == null)
        {
            Debug.LogError("프리팹이 Inspector에 연결되지 않았습니다.");
            return;
        }

        // 프리팹 생성
        GameObject newObject = Instantiate(prefabObject, position, Quaternion.identity);

        // 생성된 오브젝트의 스케일 설정
        newObject.transform.localScale = scale;

        Debug.Log($"오브젝트가 {position} 위치에 스케일 {scale}로 생성되었습니다.");
    }

    // 일정 시간 후 RawImage를 비활성화하는 코루틴
    private IEnumerator HideImageAfterDelay(float delay)
    {
        Debug.Log($"HideImageAfterDelay 코루틴 시작, {delay}초 후 비활성화 예정"); // 디버깅 로그

        yield return new WaitForSeconds(delay); // 설정된 시간 동안 대기

        targetRawImage.gameObject.SetActive(false); // RawImage 비활성화
        Debug.Log("RawImage가 비활성화되었습니다.");
    }
}
