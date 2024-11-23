using UnityEngine;
using TMPro;
using System.Collections;

public class Btn_TextChanger_KJS : MonoBehaviour
{
    public TMP_Text targetText; // TMP_Text ������Ʈ ����

    // ��ư 1, 2, 3, 4�� ����� �ؽ�Ʈ�� ���� ����
    public string textForButton1;
    public string textForButton2;
    public string textForButton3;
    public string textForButton4; // ��ư 4�� �ؽ�Ʈ

    public GameObject uiToEnableForButton1; // ��ư 1���� Ȱ��ȭ�� UI
    public GameObject uiToEnableForButton2; // ��ư 2���� Ȱ��ȭ�� UI
    public GameObject uiToEnableAfterButton3; // ��ư 3�� �ڷ�ƾ�� ���� �� Ȱ��ȭ�� UI
    public GameObject uiToEnableForButton4; // ��ư 4���� Ȱ��ȭ�� UI

    public float typingSpeed = 0.1f; // �ؽ�Ʈ�� ��µǴ� �ӵ� (��)
    public float delayAfterButton3 = 0.5f; // ��ư 3 ���� UI Ȱ��ȭ ������ (��)

    public AudioClip bgmClip; // �ڷ�ƾ ���� ����� ����� Ŭ��
    private GameObject bgmObject; // PlayClipAtPoint�� ������ �ӽ� ����� ��ü

    private Coroutine typingCoroutine; // ���� ���� ���� �ڷ�ƾ�� ����

    // ���� ��� �ؽ�Ʈ ���
    private readonly string[] randomTexts = {
        "�������̾� �߾�!",
        "�ɽ��� �߾�!",
        "���� ��縦 �����? �߾�!",
        "���� ��縦 ���� ��ȯ���� �߾�!"
    };

    private void OnEnable()
    {
        StartTyping(GetRandomText(), false); // Ȱ��ȭ�� �� ���� �ؽ�Ʈ�� �� ���ھ� ���
    }

    // �������� �ؽ�Ʈ ��ȯ
    private string GetRandomText()
    {
        int randomIndex = Random.Range(0, randomTexts.Length); // ���� �ε��� ����
        return randomTexts[randomIndex]; // ���� �ؽ�Ʈ ��ȯ
    }

    // ��ư 1���� ȣ��� �޼���
    public void ChangeTextToButton1Text()
    {
        StartTyping(textForButton1, false); // ��ư 1�� UI�� ��Ȱ��ȭ���� ����
        if (uiToEnableForButton1 != null)
        {
            uiToEnableForButton1.SetActive(true); // ��ư 1�� UI Ȱ��ȭ
        }
    }

    // ��ư 2���� ȣ��� �޼���
    public void ChangeTextToButton2Text()
    {
        StartTyping(textForButton2, false); // ��ư 2�� UI�� ��Ȱ��ȭ���� ����
        if (uiToEnableForButton2 != null)
        {
            uiToEnableForButton2.SetActive(true); // ��ư 2�� UI Ȱ��ȭ
        }
    }

    // ��ư 3���� ȣ��� �޼���
    public void ChangeTextToButton3Text()
    {
        StartTyping(textForButton3, true); // ��ư 3�� UI�� ��Ȱ��ȭ��
    }

    // ��ư 4���� ȣ��� �޼���
    public void ChangeTextToButton4Text()
    {
        StartTyping(textForButton4, false); // ��ư 4�� UI�� ��Ȱ��ȭ���� ����
        if (uiToEnableForButton4 != null)
        {
            uiToEnableForButton4.SetActive(true); // ��ư 4�� UI Ȱ��ȭ
        }
    }

    // �ؽ�Ʈ ��� ���� (���� �ڷ�ƾ �ߴ� �� ���ο� �ڷ�ƾ ����)
    private void StartTyping(string textToDisplay, bool deactivateUIAfterTyping)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // ���� �ڷ�ƾ�� ������ ����
        }

        typingCoroutine = StartCoroutine(TypeTextCoroutine(textToDisplay, deactivateUIAfterTyping));
    }

    // �ڷ�ƾ���� �ؽ�Ʈ�� �� ���ھ� ���
    private IEnumerator TypeTextCoroutine(string textToDisplay, bool deactivateUIAfterTyping)
    {
        // BGM ��� ���� (bgmClip�� �����Ǿ� ������)
        if (bgmClip != null)
        {
            bgmObject = PlayBGM(bgmClip);
        }

        targetText.text = ""; // ���� �ؽ�Ʈ �ʱ�ȭ

        foreach (char letter in textToDisplay)
        {
            targetText.text += letter; // �� ���ھ� �߰�
            yield return new WaitForSeconds(typingSpeed); // ������ �ӵ��� ���
        }

        typingCoroutine = null; // �ڷ�ƾ �Ϸ� �� �ʱ�ȭ

        // BGM ����
        StopBGM();

        // ��ư 3�� ���� ȣ���̰�, Ư�� UI�� �����Ǿ� �ִٸ� Ȱ��ȭ (0.5�� ������ �߰�)
        if (deactivateUIAfterTyping)
        {
            if (uiToEnableAfterButton3 != null)
            {
                yield return new WaitForSeconds(delayAfterButton3); // 0.5�� ���
                uiToEnableAfterButton3.SetActive(true); // Ư�� UI Ȱ��ȭ
            }

            // �� ��ũ��Ʈ�� ���Ե� UI ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
    }

    // Play BGM using PlayClipAtPoint
    private GameObject PlayBGM(AudioClip clip)
    {
        GameObject audioObject = new GameObject("BGM_Audio"); // �ӽ� ����� ��ü ����
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.loop = true; // BGM�� �ݺ��ǵ��� ����
        audioSource.playOnAwake = false;
        audioSource.Play();

        return audioObject; // ����� ��ü ��ȯ
    }

    // Stop BGM and destroy the audio object
    private void StopBGM()
    {
        if (bgmObject != null)
        {
            Destroy(bgmObject); // �ӽ� ����� ��ü ����
        }
    }
}