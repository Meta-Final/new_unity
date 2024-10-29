using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI ���
using System.IO;  // ���� ����� ���
#if UNITY_EDITOR
using UnityEditor;  // ���� Ž���� ����� ���� ���ӽ����̽�
#endif

public class ImageBox_Thumbnail : MonoBehaviour
{
    private Button button;  // ��ư ������Ʈ ����
    public Image targetImage;  // ���õ� �̹����� ǥ���� Image ������Ʈ (Inspector���� �Ҵ�)

    void Start()
    {
        // ��ư ������Ʈ ��������
        button = GetComponent<Button>();

        if (button != null)
        {
            // ��ư Ŭ�� �� OpenFileExplorer �޼��� ȣ��
            button.onClick.AddListener(OpenFileExplorer);
        }
        else
        {
            Debug.LogError("Button ������Ʈ�� ã�� �� �����ϴ�.");
        }

        if (targetImage == null)
        {
            Debug.LogError("Target Image�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� Image ������Ʈ�� �����ϼ���.");
        }
    }

    // ���� Ž���⸦ ���� �޼���
    void OpenFileExplorer()
    {
#if UNITY_EDITOR
        try
        {
            // ���� Ž���� ����: �̹��� ���� ����
            string path = EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

            if (!string.IsNullOrEmpty(path))
            {
                Debug.Log($"���õ� ���� ���: {path}");

                // ���õ� ���� ����� �̹����� �ε��Ͽ� Image ������Ʈ�� ǥ��
                StartCoroutine(LoadImage(path));
            }
            else
            {
                Debug.LogWarning("������ ���õ��� �ʾҽ��ϴ�.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"���� Ž���� ���� �� ���� �߻�: {e.Message}");
        }
#else
        Debug.LogWarning("���� Ž����� Unity Editor������ ����� �� �ֽ��ϴ�.");
#endif
    }

    // ���õ� �̹��� ������ �ε��Ͽ� Image ������Ʈ�� �����ϴ� �ڷ�ƾ
    IEnumerator LoadImage(string path)
    {
        // ���� ��ο��� �̹��� �����͸� �о����
        byte[] imageData = File.ReadAllBytes(path);

        // Texture2D ����
        Texture2D texture = new Texture2D(2, 2);  // �ӽ� ũ�� (�ε� �� �ڵ����� ������)
        bool isLoaded = texture.LoadImage(imageData);  // �̹��� ������ �ε�

        if (isLoaded)
        {
            // Texture2D�� Sprite�� ��ȯ
            Sprite newSprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            // Image ������Ʈ�� �� Sprite ����
            targetImage.sprite = newSprite;
            Debug.Log("�̹����� ���������� �ε�Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("�̹��� �ε忡 �����߽��ϴ�.");
        }

        yield return null;
    }
}
