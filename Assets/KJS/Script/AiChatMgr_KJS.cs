using System.Collections;
using System.Collections.Generic;
using System.IO; // File.ReadAllBytes ����� ���� �ʿ�
using UnityEngine;
using UnityEngine.UI; // RawImage �� Scroll View ���
using TMPro; // TextMeshPro ���

public class AiChatMgr_KJS : MonoBehaviour
{
    public TMP_InputField userInputField; // ����ڰ� �Է��� TMP �ʵ�
    public TMP_Text chatResponseText;     // AI ������ ǥ���� TMP �ؽ�Ʈ
    public APIManager apiManager;         // APIManager �ν��Ͻ� ����
    public Button sendButton;             // ���� ��ư
    public Transform scrollViewContent;   // Scroll View�� Content ����

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
            userInputField.text = "";            // �Է� �ʵ� �ʱ�ȭ
            apiManager.CallLLM(userMessage);     // API ȣ�� (�񵿱� ���� ���)
        }
    }

    // APIManager���� ���� �ؽ�Ʈ�� �޾ƿ� ǥ���ϴ� �޼���
    public void UpdateChatResponse(string response)
    {
        Debug.Log($"UpdateChatResponse ȣ���: {response}"); // ����� �α� �߰�

        chatResponseText.text = response; // AI ���� ǥ��

        if (response == "text:/img.json")
        {
            LoadImageFromPath(@"C:\Users\Admin\Desktop\�丮.jpg");
        }
    }

    // ���� �̹��� ������ �ε��ϰ� Scroll View�� ǥ���ϴ� �޼���
    private void LoadImageFromPath(string path)
    {
        Debug.Log($"LoadImageFromPath ȣ���: {path}"); // ����� �α� �߰�

        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2); // �ӽ÷� 2x2 �ؽ�ó ����

            if (tex.LoadImage(fileData))
            {
                Debug.Log("�̹��� �ε� ����!");
                CreateImageGameObject(tex); // �̹����� ǥ���� GameObject ����
            }
            else
            {
                Debug.LogError("�̹��� �ε� ����!");
            }
        }
        else
        {
            Debug.LogError($"�̹��� ������ ã�� �� �����ϴ�: {path}");
        }
    }

    // Scroll View�� Content�� Image GameObject�� �����ϴ� �޼���
    private void CreateImageGameObject(Texture2D texture)
    {
        Debug.Log("CreateImageGameObject ȣ���"); // ����� �α� �߰�

        GameObject newImageObject = new GameObject("ChatImage", typeof(RectTransform), typeof(RawImage));
        newImageObject.transform.SetParent(scrollViewContent, false); // false�� ������ ���� ��ġ ����

        RectTransform rectTransform = newImageObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = new Vector2(300, 300);

        RawImage rawImage = newImageObject.GetComponent<RawImage>();
        rawImage.texture = texture;
    }
}