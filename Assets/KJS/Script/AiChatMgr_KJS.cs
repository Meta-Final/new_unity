using System.Collections;
using UnityEngine;
using UnityEngine.UI; // RawImage ���
using TMPro; // TextMeshPro ���

public class AiChatMgr_KJS : MonoBehaviour
{
    // �̱��� �ν��Ͻ� ����
    public static AiChatMgr_KJS Instance { get; private set; }

    public TMP_InputField userInputField; // ����ڰ� �Է��� TMP �ʵ�
    public TMP_Text chatResponseText;     // AI ������ ǥ���� TMP �ؽ�Ʈ
    public APIManager apiManager;         // APIManager �ν��Ͻ� ����
    public Button sendButton;             // ���� ��ư
    public RawImage targetRawImage;       // Inspector���� �Ҵ�� RawImage ����
    public Texture2D inspectorTexture;    // Inspector���� �Ҵ�� �̹���(Texture2D)
    public Transform scrollViewContent;   // Scroll View�� Content ����
    public GameObject prefabObject;       // ������ ������ ���� (Inspector���� �Ҵ�)

    public float displayDuration = 3f; // �� �� ���� �̹����� ǥ������ ����

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
            if (userMessage.Trim() == "�丮 �̹��� �������")
            {
                UpdateChatResponse("�丮 �̹��� �������"); // ���� �޼��� ȣ��
            }
            else if (userMessage.Trim() == "�۾��� ����Ʈ ������ ������Ʈ�� �������")
            {
                UpdateChatResponse("�۾��� ����Ʈ ������ ������Ʈ�� �������"); // ���ο� ���� ó��
            }
            else
            {
                apiManager.CallLLM(userMessage); // API ȣ�� (�񵿱� ���� ���)
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
            if (response == "�丮 �̹��� �������")
            {
                chatResponseText.text = "�̹����� ��������ϴ�.";

                if (targetRawImage == null)
                {
                    Debug.LogError("targetRawImage�� Inspector�� ������� �ʾҽ��ϴ�.");
                    return;
                }

                if (inspectorTexture != null)
                {
                    Debug.Log("Inspector���� �Ҵ�� �̹����� ǥ���մϴ�.");

                    targetRawImage.texture = inspectorTexture; // RawImage�� �ؽ�ó ����
                    targetRawImage.gameObject.SetActive(true); // RawImage Ȱ��ȭ

                    StartCoroutine(HideImageAfterDelay(displayDuration)); // ���� �ð� �� ��Ȱ��ȭ �ڷ�ƾ ����
                }
                else
                {
                    Debug.LogError("inspectorTexture�� Inspector���� �Ҵ���� �ʾҽ��ϴ�.");
                }
            }
            // ���ο� ����: ������Ʈ ����
            else if (response == "�۾��� ����Ʈ ������ ������Ʈ�� �������")
            {
                chatResponseText.text = "������Ʈ�� ��������ϴ�.";
                Debug.Log("������Ʈ�� �����մϴ�.");

                // ������Ʈ�� (-41.9, 19.5, -27) ��ġ�� �����ϰ� �������� 25�� ����
                CreateObjectAtPositionAndScale(new Vector3(-41.9f, 19.5f, -27f), new Vector3(25f, 25f, 25f));
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

    // ������ ��ġ�� �����Ϸ� ������ ������Ʈ�� �����ϴ� �޼���
    private void CreateObjectAtPositionAndScale(Vector3 position, Vector3 scale)
    {
        if (prefabObject == null)
        {
            Debug.LogError("�������� Inspector�� ������� �ʾҽ��ϴ�.");
            return;
        }

        // ������ ����
        GameObject newObject = Instantiate(prefabObject, position, Quaternion.identity);

        // ������ ������Ʈ�� ������ ����
        newObject.transform.localScale = scale;

        Debug.Log($"������Ʈ�� {position} ��ġ�� ������ {scale}�� �����Ǿ����ϴ�.");
    }

    // ���� �ð� �� RawImage�� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    private IEnumerator HideImageAfterDelay(float delay)
    {
        Debug.Log($"HideImageAfterDelay �ڷ�ƾ ����, {delay}�� �� ��Ȱ��ȭ ����"); // ����� �α�

        yield return new WaitForSeconds(delay); // ������ �ð� ���� ���

        targetRawImage.gameObject.SetActive(false); // RawImage ��Ȱ��ȭ
        Debug.Log("RawImage�� ��Ȱ��ȭ�Ǿ����ϴ�.");
    }
}
