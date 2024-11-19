using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AiChatMgr_KJS : MonoBehaviour
{
    // �̱��� �ν��Ͻ� ����
    public static AiChatMgr_KJS Instance { get; private set; }

    public TMP_InputField userInputField; // ����ڰ� �Է��� TMP �ʵ�
    public TMP_Text chatResponseText;     // AI ������ ǥ���� TMP �ؽ�Ʈ
    public APIManager apiManager;         // APIManager �ν��Ͻ� ����
    public Button sendButton;             // ���� ��ư
    public GameObject extraUI;            // �߰����� UI (��ҿ��� ���� ����)

    private GameObject chatUI;            // MagazineView �ȿ� �ִ� Chat UI
    private GameObject toolUI;            // MagazineView �ȿ� �ִ� Tool UI

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // �̹� �ν��Ͻ��� �����ϸ� �ߺ��� ������Ʈ�� ����
        }
        else
        {
            Instance = this; // �ν��Ͻ� �Ҵ�
        }
    }

    void Start()
    {
        // "MagazineView" ������Ʈ �ȿ� �ִ� "Chat"�� "Tool" UI�� ã��
        GameObject magazineView = GameObject.Find("MagazineView");
        if (magazineView != null)
        {
            chatUI = magazineView.transform.Find("Chat")?.gameObject;
            toolUI = magazineView.transform.Find("Tool")?.gameObject;

            if (chatUI != null)
            {
                chatUI.SetActive(false); // Chat UI�� ��Ȱ��ȭ ���·� �ʱ�ȭ
            }
            else
            {
                Debug.LogError("Chat UI not found within MagazineView.");
            }

            if (toolUI != null)
            {
                toolUI.SetActive(false); // Tool UI�� ��Ȱ��ȭ ���·� �ʱ�ȭ
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

        // extraUI �ʱ� ��Ȱ��ȭ
        if (extraUI != null)
        {
            extraUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Extra UI�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        // ��ư Ŭ�� �� OnSendButtonClicked ȣ��
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    // ������Ʈ Ŭ�� �� Chat UI Ȱ��ȭ
    private void OnMouseDown()
    {
        if (chatUI != null)
        {
            chatUI.SetActive(true); // Chat UI Ȱ��ȭ
            Debug.Log("Chat UI�� Ȱ��ȭ�Ǿ����ϴ�.");
        }
    }

    void OnSendButtonClicked()
    {
        string userMessage = userInputField.text; // �Էµ� �ؽ�Ʈ ��������

        if (!string.IsNullOrEmpty(userMessage))
        {
            chatResponseText.text = ""; // ����ڰ� �Է��� �ؽ�Ʈ �ʱ�ȭ
            userInputField.text = "";  // �Է� �ʵ� �ʱ�ȭ

            // �Էµ� �ؽ�Ʈ�� ���� �ٸ� ���� ó��
            if (userMessage.Trim() == "ũ���������̹����������")
            {
                StartCoroutine(ProcessMessage("�̹����� �������. �߾�!", "ũ���������̹����������"));
                apiManager.Cover(); // APIManager�� Cover �޼��� ȣ��
            }
            else if (userMessage.Trim() == "�۾��ѱ�縦������Ʈ�θ������")
            {
                StartCoroutine(ProcessMessage("������Ʈ�� �������. �߾�!", "�۾�������Ʈ��縦������Ʈ�θ������"));
            }
            else if (userMessage.Trim() == "�۾���;�")
            {
                StartCoroutine(ProcessMessage("�˰ڴ� �߾�!", "�۾���;�"));
            }
            else if (userMessage.Trim() == "�����ֱٿ���ũ���ѱ�縦�˷���")
            {
                StartCoroutine(ProcessMessage("�ʰ� �ֱٿ� ��ũ���� ���� ��ȭ��, �����̴� �߾�!", "�����ֱٿ���ũ���ѱ�縦�˷���"));
            }
            else
            {
                StartCoroutine(ProcessMessage(userMessage, ""));
            }
        }
    }

    // �ڷ�ƾ: �޽��� ��� �� extraUI�� 1�� ���� Ȱ��ȭ
    private IEnumerator ProcessMessage(string responseText, string actionKey)
    {
        // �� ���ھ� �ؽ�Ʈ ���
        yield return StartCoroutine(TypeText(responseText));

        // �߰� UI 1�ʰ� Ȱ��ȭ
        yield return StartCoroutine(ShowExtraUIForOneSecond());

        // �߰� �۾� ���� (�ʿ��ϸ� actionKey�� ���� ó�� ����)
        if (actionKey == "�۾���;�" && toolUI != null)
        {
            toolUI.SetActive(true); // Tool UI Ȱ��ȭ
        }
    }

    // Coroutine: extraUI�� 1�ʰ� Ȱ��ȭ
    private IEnumerator ShowExtraUIForOneSecond()
    {
        if (extraUI != null)
        {
            extraUI.SetActive(true); // extraUI Ȱ��ȭ
            Debug.Log("Extra UI Ȱ��ȭ");
            yield return new WaitForSeconds(1f); // 1�� ���
            extraUI.SetActive(false); // extraUI ��Ȱ��ȭ
            Debug.Log("Extra UI ��Ȱ��ȭ");
        }
    }

    // APIManager���� ���� �ؽ�Ʈ�� �޾ƿ� �� ���ھ� ǥ���ϴ� �޼���
    private IEnumerator TypeText(string text)
    {
        chatResponseText.text = ""; // ���� �ؽ�Ʈ �ʱ�ȭ
        foreach (char c in text)
        {
            chatResponseText.text += c; // �� ���ھ� �߰�
            yield return new WaitForSeconds(0.05f); // ������ �߰� (0.05��)
        }
    }
}