using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class InventoryText_KJS : MonoBehaviour
{
    public List<H_PostInfo> allPost = new List<H_PostInfo>(); // ��� post ������ ����
    public GameObject prefabfactory;                         // ��ư ������
    public Transform inventoryPanel;                         // ��ư�� ��ġ�� �θ�
    public GameObject MagCanvas;                             // �� ������ ǥ���� ĵ����
    public Button btn_Exit;                                  // �ݱ� ��ư

    private LoadMgr_KJS loadManager;                         // �ܺο��� �����͸� �ε��� �Ŵ���

    private string baseDirectory = Application.dataPath;     // �⺻ ���� ���
    private List<Button> btns = new List<Button>();          // ������ ��ư ����Ʈ

    void Start()
    {
        // EditorManager���� LoadMgr_KJS ������Ʈ�� ã���ϴ�.
        GameObject editorManagerObj = GameObject.Find("EditorManager");
        if (editorManagerObj != null)
        {
            loadManager = editorManagerObj.GetComponent<LoadMgr_KJS>();
            if (loadManager == null)
            {
                Debug.LogError("EditorManager ������Ʈ�� LoadMgr_KJS ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("EditorManager ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // �ʱ�ȭ �۾�
        ThumStart();
    }

    public void ThumStart()
    {
        // postId���� ����� �������� JSON ������ �н��ϴ�.
        if (Directory.Exists(baseDirectory))
        {
            string[] postDirectories = Directory.GetDirectories(baseDirectory);

            foreach (string postDirectory in postDirectories)
            {
                string jsonFilePath = Path.Combine(postDirectory, "thumbnail.json");

                if (File.Exists(jsonFilePath))
                {
                    try
                    {
                        string json = File.ReadAllText(jsonFilePath);
                        PostInfoList postInfoList = JsonUtility.FromJson<PostInfoList>(json);
                        allPost.AddRange(postInfoList.postData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"JSON ������ �ε��ϴ� ���� ������ �߻��߽��ϴ�: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"���� {postDirectory}�� thumbnail.json ������ �����ϴ�.");
                }
            }
        }
        else
        {
            Debug.LogError($"�⺻ ���丮�� �������� �ʽ��ϴ�: {baseDirectory}");
        }

        // �ε��� �����͸� �������� UI ����
        CreatePostThumbnails();
    }

    private void CreatePostThumbnails()
    {
        // �о�� ��� post ������ ������� ��ư�� ����
        for (int i = 0; i < allPost.Count; i++)
        {
            // ��ư ������ ����
            GameObject go = Instantiate(prefabfactory, inventoryPanel);

            // ��ư ������Ʈ�� ������
            Button button = go.GetComponent<Button>();
            btns.Add(button);

            string postId = allPost[i].postid;

            // ��ư Ŭ�� �̺�Ʈ �߰�
            btns[i].onClick.AddListener(() => OnClickMagContent(postId));

            // ��ư�� ����� �̹����� ����
            StartCoroutine(SetButtonImage(go, allPost[i].thumburl));
        }

        // MagCanvas�� ������ ��Ȱ��ȭ
        if (MagCanvas)
        {
            MagCanvas.SetActive(false);
        }
    }

    private IEnumerator SetButtonImage(GameObject buttonObject, string imageUrl)
    {
        // URL���� �̹����� �ٿ�ε�
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + imageUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // �ؽ�ó�� Sprite�� ��ȯ
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            // ��ư�� Image ������Ʈ�� ������ ����
            Image buttonImage = buttonObject.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("��ư�� Image ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError($"�̹��� �ٿ�ε� ����: {imageUrl}, {request.error}");
        }
    }

    public void OnClickMagContent(string postId)
    {
        // �� ���� UI Ȱ��ȭ
        MagCanvas.SetActive(true);

        // LoadMgr_KJS �Ŵ����� ���� �ش� postId�� �����͸� �ε�
        if (loadManager != null)
        {
            loadManager.LoadObjectsFromFile(postId);
        }
        else
        {
            Debug.LogError("loadManager�� null�Դϴ�. LoadMgr_KJS�� ã�� �� �����ϴ�.");
        }
    }
}