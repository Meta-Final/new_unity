using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // UI ���
using System.IO;  // ���� ����� ���
#if UNITY_EDITOR
using UnityEditor;  // ���� Ž���� ����� ���� ���ӽ����̽�
#endif

[System.Serializable]

public class Thumbnail_KJS : MonoBehaviour
{
    public Button saveButton;  // ���� ��ư (Inspector���� ����)
    public string savePath = "C:/Users/haqqm/Desktop/postData.json";  // JSON ���� ���� ���

    private PostInfoList postInfoList = new PostInfoList();  // �ν��Ͻ� ����� �ʱ�ȭ

    void Start()
    {
        if (saveButton == null)
        {
            Debug.LogError("saveButton�� �Ҵ���� �ʾҽ��ϴ�. Inspector���� �����ϼ���.");
            return;
        }

        // ��ư Ŭ�� �� OpenFileExplorer �޼��� ȣ��
        saveButton.onClick.AddListener(OpenFileExplorer);
    }

    void OpenFileExplorer()
    {
#if UNITY_EDITOR
        try
        {
            string path = EditorUtility.OpenFilePanel("Select Thumbnail", "", "png,jpg");

            if (!string.IsNullOrEmpty(path))
            {
                Debug.Log($"���õ� ���� ���: {path}");

                // ���õ� ���� ��θ� ������ �Խù� ���� �߰�
                H_PostInfo newPost = new H_PostInfo { editorname = "Alice", thumburl = path };
                postInfoList.postData.Add(newPost);  // �Խù� ��Ͽ� �߰�

                // JSON ������ ����
                SaveToJson();
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

    void SaveToJson()
    {
        try
        {
            // JSON �������� ����ȭ
            string json = JsonUtility.ToJson(postInfoList, true);  // true�� �鿩���� ����

            // ���� ����
            File.WriteAllText(savePath, json);

            Debug.Log($"JSON �����Ͱ� {savePath}�� ����Ǿ����ϴ�.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON ���� �� ���� �߻�: {e.Message}");
        }
    }
}
