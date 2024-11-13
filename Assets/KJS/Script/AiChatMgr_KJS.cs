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
        // ��ư Ŭ�� �� OnSendButtonClicked ȣ��
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    void OnSendButtonClicked()
    {
        string userMessage = userInputField.text; // �Էµ� �ؽ�Ʈ ��������

        if (!string.IsNullOrEmpty(userMessage))
        {
            chatResponseText.text = userMessage; // ����ڰ� �Է��� �ؽ�Ʈ ǥ��
            userInputField.text = ""; // �Է� �ʵ� �ʱ�ȭ

            // �Էµ� �ؽ�Ʈ�� ���� �ٸ� ���� ó��
            if (userMessage.Trim() == "�̹��� �������")
            {
                UpdateChatResponse("�̹��� �������"); // ���� �޼��� ȣ��
                apiManager.Cover(); // APIManager�� Cover �޼��� ȣ��
            }
            else if (userMessage.Trim() == "�۾��� ����Ʈ ������ ������Ʈ�� �������")
            {
                UpdateChatResponse("�۾��� ����Ʈ ������ ������Ʈ�� �������"); // ���ο� ���� ó��
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
            if (response == "�̹��� �������")
            {
                chatResponseText.text = "�̹����� ��������ϴ�.";
                Debug.Log("�丮 �̹��� ���� ��û�� ���� �����Դϴ�.");
            }
            else if (response == "�۾��� ����Ʈ ������ ������Ʈ�� �������")
            {
                chatResponseText.text = "������Ʈ�� ��������ϴ�.";
                Debug.Log("�۾��� ����Ʈ ������ ������Ʈ�� ����� ��û�� ���� �����Դϴ�.");
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
