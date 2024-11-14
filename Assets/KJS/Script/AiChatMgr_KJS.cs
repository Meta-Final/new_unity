using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI ���
using TMPro; // TextMeshPro ���

public class AiChatMgr_KJS : MonoBehaviour
{
    // �̱��� �ν��Ͻ� ����
    public static AiChatMgr_KJS Instance { get; private set; }

    public TMP_InputField userInputField; // ����ڰ� �Է��� TMP �ʵ�
    public TMP_Text chatResponseText;     // AI ������ ǥ���� TMP �ؽ�Ʈ
    public APIManager apiManager;         // APIManager �ν��Ͻ� ����
    public Button sendButton;             // ���� ��ư

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
            chatResponseText.text = userMessage; // ����ڰ� �Է��� �ؽ�Ʈ ǥ��
            userInputField.text = ""; // �Է� �ʵ� �ʱ�ȭ

            // �Էµ� �ؽ�Ʈ�� ���� �ٸ� ���� ó��
            if (userMessage.Trim() == "ũ���������̹����������")
            {
                UpdateChatResponse("ũ���������̹����������"); // ���� �޼��� ȣ��
                apiManager.Cover(); // APIManager�� Cover �޼��� ȣ��
            }
            else if (userMessage.Trim() == "�۾��ѱ�縦������Ʈ�θ������")
            {
                UpdateChatResponse("�۾�������Ʈ��縦������Ʈ�θ������"); // ���ο� ���� ó��
            }
            else if (userMessage.Trim() == "�۾���;�")
            {
                UpdateChatResponse("�۾���;�");
            }
            else if (userMessage.Trim() == "�����ֱٿ���ũ���ѱ�縦�˷���")
            {
                UpdateChatResponse("�����ֱٿ���ũ���ѱ�縦�˷���");
            }
            else
            {
                // apiManager.CallLLM(userMessage); // API ȣ�� (�񵿱� ���� ���)
            }
        }
    }

    // APIManager���� ���� �ؽ�Ʈ�� �޾ƿ� ǥ���ϴ� �޼���
    public void UpdateChatResponse(string response)
    {
        try
        {
            Debug.Log($"UpdateChatResponse ȣ���: {response}"); // ���� �α�

            // �丮 �̹��� ���� ó��
            if (response == "ũ���������̹����������")
            {
                chatResponseText.text = "�̹����� �������. �߾�!";
                Debug.Log("�丮 �̹��� ���� ��û�� ���� �����Դϴ�.");
            }
            else if (response == "�۾��ѱ�縦������Ʈ�θ������")
            {
                chatResponseText.text = "������Ʈ�� �������. �߾�!";
                Debug.Log("�۾��� ����Ʈ ������ ������Ʈ�� ����� ��û�� ���� �����Դϴ�.");
            }
            else if (response == "�۾���;�")
            {
                chatResponseText.text = "�˰ڴ� �߾�!";
                Debug.Log("�۾��� ��û�� ���� �����Դϴ�.");

                // Tool UI�� Ȱ��ȭ
                if (toolUI != null)
                {
                    toolUI.SetActive(true);
                }
                else
                {
                    Debug.LogError("Tool UI is not assigned.");
                }
            }
            else if (response == "�����ֱٿ���ũ���ѱ�縦�˷���")
            {
                // �ֱ� ��ũ���� ��� ���� ó��
                chatResponseText.text = "�ʰ� �ֱٿ� ��ũ���� ���� ��ȭ��, �����̴� �߾�!";
                Debug.Log("�ֱ� ��ũ���� ��� ��û�� ���� �����Դϴ�.");
            }
            else
            {
                // �ٸ� ������ ���, ���޵� ������ �״�� ǥ��
                chatResponseText.text = response;
                Debug.LogWarning("���ǿ� �´� ������ �ƴմϴ�.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"UpdateChatResponse �� ���� �߻�: {e.Message}");
        }
    }
}