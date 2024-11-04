using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateMgr_KJS : MonoBehaviour
{
    public SaveMgr_KJS saveMgr;  // SaveMgr_KJS ����

    public GameObject panelPage;   // ������ ������
    public GameObject textBoxPrefab;     // �ؽ�Ʈ �ڽ� ������
    public GameObject imageBoxPrefab;    // �̹��� �ڽ� ������

    public Transform content;  // ������ ������Ʈ�� �θ� Ʈ������
    public Scrollbar horizontalScrollbar;
    public ScrollRect scrollRect;

    public Button loadButton;  // �����͸� �ε��ϴ� ��ư

    private int pageCount = 1;
    private float pageWidth;

    public ToolMgr_KJS toolManager;
    public EditorMgr_KJS editorMgr;
    public ImageMgr_KJS imageMgr;  // ImageMgr_KJS ����

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    private string saveDirectory = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo";
    private string saveFileName = "Magazine.json";
    private string savePath;

    private RootObject rootData = new RootObject();

    void Start()
    {
        if (panelPage == null || content == null) return;
        pageWidth = panelPage.GetComponent<RectTransform>().rect.width;

        // ���� ��� ����
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        UpdateScrollbarSteps();

        // �ε� ��ư�� �����Ǿ� �ִٸ�, ��ư Ŭ�� �� CreateObjectsFromFile�� ȣ���ϵ��� ����
        if (loadButton != null)
        {
            loadButton.onClick.AddListener(CreateObjectsFromFile);
        }
    }

    public void CreateObjectsFromFile()
    {
        try
        {
            Debug.Log("LoadObjectsFromFile() called.");

            if (!File.Exists(savePath))
            {
                Debug.LogWarning("Save file not found.");
                return;
            }

            string json = File.ReadAllText(savePath);
            rootData = JsonUtility.FromJson<RootObject>(json);

            if (rootData.posts.Count == 0) return;

            // ���� ������Ʈ ����
            ClearExistingObjects();

            Post post = rootData.posts[0];

            // JSON ������ ������� �������� ��� ����
            foreach (var page in post.pages)
            {
                GameObject newPageObj = CreatePage(page.pageId);

                foreach (var element in page.elements)
                {
                    if (element.type == Element.ElementType.Text_Box)
                    {
                        CreateTextBox(newPageObj.transform, element.content, element.fontSize, element.fontFace, element.position, element.scale, element.isUnderlined, element.isStrikethrough);
                    }
                    else if (element.type == Element.ElementType.Image_Box)
                    {
                        CreateImageBox(newPageObj.transform, element.imageData, element.position, element.scale);
                    }
                }
            }

            pageCount = pages.Count;
            UpdateScrollbarSteps();

            Debug.Log("Data loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }

    private void ClearExistingObjects()
    {
        // ���� ������Ʈ ����
        textBoxes.ForEach(Destroy);
        imageBoxes.ForEach(Destroy);
        pages.ForEach(Destroy);

        textBoxes.Clear();
        imageBoxes.Clear();
        pages.Clear();
    }

    private GameObject CreatePage(int pageId = -1)
    {
        GameObject newPage = Instantiate(panelPage, content);
        newPage.name = pageId >= 0 ? $"Page_{pageId}" : $"Page_{System.Guid.NewGuid()}";
        pageCount++;

        saveMgr.AddPage(newPage);
        pages.Add(newPage);  // ����Ʈ�� �߰�

        Button btn_TextBox = newPage.transform.Find("btn_TextBox")?.GetComponent<Button>();
        if (btn_TextBox != null)
        {
            btn_TextBox.onClick.AddListener(() => CreateTextBox(newPage.transform));
        }

        Button btn_ImageBox = newPage.transform.Find("btn_Image")?.GetComponent<Button>();
        if (btn_ImageBox != null)
        {
            btn_ImageBox.onClick.AddListener(() => CreateImageBox(newPage.transform));
        }

        Button deleteButton = newPage.transform.Find("btn_Delete")?.GetComponent<Button>();
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(() => RemovePage(newPage));
        }

        UpdateContentWidth();
        UpdateScrollbarSteps();

        return newPage;
    }

    public void CreateTextBox(Transform parent, string content = "", int fontSize = 14, string fontFace = "Arial", Vector3 position = default, Vector3 scale = default, bool isUnderlined = false, bool isStrikethrough = false)
    {
        GameObject newTextBox = Instantiate(textBoxPrefab, parent);
        newTextBox.name = $"TextBox_{System.Guid.NewGuid()}";

        TMP_Text textComponent = newTextBox.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.text = content;
            textComponent.fontSize = fontSize;

            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>($"Fonts/{fontFace}");
            if (fontAsset != null) textComponent.font = fontAsset;

            textComponent.fontStyle = FontStyles.Normal;
            if (isUnderlined) textComponent.fontStyle |= FontStyles.Underline;
            if (isStrikethrough) textComponent.fontStyle |= FontStyles.Strikethrough;
        }

        newTextBox.transform.localPosition = position;
        newTextBox.transform.localScale = scale == default ? Vector3.one : scale;

        saveMgr.AddTextBox(newTextBox);
        textBoxes.Add(newTextBox);  // ����Ʈ�� �߰�

        Button buttonContent = newTextBox.GetComponentInChildren<Button>();
        if (buttonContent != null)
        {
            buttonContent.name = $"{newTextBox.name}_Button";
            buttonContent.onClick.AddListener(toolManager.OnClickTogglePanel);
            buttonContent.onClick.AddListener(() =>
            {
                if (editorMgr != null)
                {
                    editorMgr.SetInputFieldTextFromButton(buttonContent);
                }
                else
                {
                    Debug.LogError("EditorMgr_KJS�� �Ҵ���� �ʾҽ��ϴ�.");
                }
            });
        }
        else
        {
            Debug.LogError("�ؽ�Ʈ �ڽ� �����տ� Button ������Ʈ�� �����ϴ�.");
        }
    }

    public void CreateImageBox(Transform parent, string imageData = "", Vector3 position = default, Vector3 scale = default)
    {
        GameObject newImageBox = Instantiate(imageBoxPrefab, parent);
        newImageBox.name = $"ImageBox_{System.Guid.NewGuid()}";

        Image imageComponent = newImageBox.transform.GetChild(0).GetComponent<Image>();
        if (imageComponent != null && !string.IsNullOrEmpty(imageData))
        {
            Texture2D texture = Element.DecodeImageFromBase64(imageData);
            imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        newImageBox.transform.localPosition = position;
        newImageBox.transform.localScale = scale == default ? Vector3.one : scale;

        saveMgr.AddImageBox(newImageBox);
        imageBoxes.Add(newImageBox);  // ����Ʈ�� �߰�

        Button buttonContent = newImageBox.GetComponentInChildren<Button>();
        if (buttonContent != null)
        {
            imageMgr.AddButton(buttonContent);
            Debug.Log($"ImageBox button {buttonContent.name} added to ImageMgr_KJS.");
        }
        else
        {
            Debug.LogError("ImageBox �����տ� Button ������Ʈ�� �����ϴ�.");
        }
    }

    public void RemovePage(GameObject page)
    {
        saveMgr.RemovePage(page);
        pages.Remove(page);
        Destroy(page);
        pageCount = Mathf.Max(1, pageCount - 1);

        UpdateContentWidth();
        UpdateScrollbarSteps();
    }

    private void UpdateContentWidth()
    {
        if (content == null) return;

        RectTransform contentRect = content.GetComponent<RectTransform>();
        float newWidth = pageWidth * pageCount;
        contentRect.sizeDelta = new Vector2(newWidth, contentRect.sizeDelta.y);
    }

    private void UpdateScrollbarSteps()
    {
        if (horizontalScrollbar == null || pageCount <= 1) return;

        horizontalScrollbar.numberOfSteps = pageCount;
    }
}