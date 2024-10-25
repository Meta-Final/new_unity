using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�

public class UIManager : MonoBehaviour
{

    public TMP_InputField userInputField; // ����ڰ� �Է��� TMP �ʵ�
    public TMP_Text chatResponseText;     // AI ������ ǥ���� TMP �ؽ�Ʈ
    //public APIManager apiManager;         // APIManager �ν��Ͻ� ����

    public UnityEngine.UI.Button sendButton; // ���� ��ư (Button�� TMP ������ �ƴ�)

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
            chatResponseText.text = "Thinking..."; // ���� ��� �� �޽���
            //apiManager.CallLLM(userMessage); // API ȣ��
        }
    }

    // APIManager�� ȣ���� ������ ǥ���ϴ� �޼���
    public void UpdateChatResponse(string response)
    {
        chatResponseText.text = response; // AI ���� ǥ��
    }
}
