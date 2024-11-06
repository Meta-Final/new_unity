using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Element
{
    public string content;
    public enum ElementType { Text_Box, Image_Box }
    public ElementType type;
    public string imageData;
    public Vector3 position;
    public Vector3 scale;

    public int fontSize;
    public string fontFace;
    public bool isUnderlined;
    public bool isStrikethrough;

    public Element(
        ElementType type, string content, string imageData, Vector3 position, Vector3 scale,
        int fontSize = 14, string fontFace = "Arial", bool isUnderlined = false, bool isStrikethrough = false)
    {
        this.type = type;
        this.content = content;
        this.imageData = imageData;
        this.position = position;
        this.scale = scale;
        this.fontSize = fontSize;
        this.fontFace = fontFace;
        this.isUnderlined = isUnderlined;
        this.isStrikethrough = isStrikethrough;
    }

    public static string EncodeImageToBase64(Texture2D texture)
    {
        if (texture == null) return null;
        byte[] imageData = texture.EncodeToPNG();
        return Convert.ToBase64String(imageData);
    }

    public static Texture2D DecodeImageFromBase64(string base64String)
    {
        if (string.IsNullOrEmpty(base64String)) return null;
        byte[] imageData = Convert.FromBase64String(base64String);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageData);
        return texture;
    }
}

[System.Serializable]
public class Page
{
    public int pageId;
    public List<Element> elements = new List<Element>();

    public Page(int id)
    {
        pageId = id;
    }
}

[System.Serializable]
public class Post
{
    public string postId;
    public List<Page> pages = new List<Page>();

    public Post(string id)
    {
        postId = id;
    }
}

[System.Serializable]
public class RootObject
{
    public List<Post> posts = new List<Post>();
}

public class SaveMgr_KJS : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject imageBoxPrefab;
    public GameObject pagePrefab;
    public Scrollbar pageScrollbar;
    public ToolMgr_KJS toolManager;
    public EditorMgr_KJS editorMgr;
    public ImageMgr_KJS imageMgr;

    public int totalPages = 1;

    public Transform parent;
    public Transform pagesParentTransform;

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    public Button saveButton;
    public List<Button> loadButtons = new List<Button>();

    private string saveDirectory = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo";
    private string saveFileName = "Magazine.json";
    private string savePath;

    private RootObject rootData = new RootObject();
    public CreateMgr_KJS createMgr;

    public TMP_InputField inputPostIdField;
    public TMP_InputField loadPostIdField;

    private void Start()
    {
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        saveButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(SaveObjectsToFile);

        EnsureDirectoryExists();

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            rootData = JsonUtility.FromJson<RootObject>(json);
        }
        else
        {
            Directory.CreateDirectory(saveDirectory);
        }
    }

    public void SetLoadButton(Button button)
    {
        loadButtons.Add(button);
        Debug.Log($"��ư {button.name}�� �߰��Ǿ����ϴ�.");

        //button.onClick.AddListener(() => CreateObjectsFromFile());
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
            Debug.Log($"Directory created at: {saveDirectory}");
        }
    }

    public void AddTextBox(GameObject textBox) => textBoxes.Add(textBox);
    public void AddImageBox(GameObject imageBox) => imageBoxes.Add(imageBox);
    public void AddPage(GameObject page)
    {
        pages.Add(page); // pages ����Ʈ�� ������ �߰�
        totalPages = pages.Count; // totalPages�� pages.Count�� ������Ʈ
        Debug.Log($"Page added. Current totalPages: {totalPages}");
    }

    public void RemovePage(GameObject page)
    {
        pages.Remove(page); // pages ����Ʈ���� ������ ����
        totalPages = pages.Count; // totalPages�� pages.Count�� ������Ʈ
        Debug.Log($"Page removed. Current totalPages: {totalPages}");
    }

    private void SaveObjectsToFile()
    {
        string targetPostId = inputPostIdField.text;

        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("��ȿ�� postId�� �Է��ϼ���.");
            return;
        }

        try
        {
            Post existingPost = rootData.posts.Find(post => post.postId == targetPostId);
            if (existingPost != null)
            {
                Debug.LogError($"postId '{targetPostId}'�� �ش��ϴ� �Խù��� �̹� �����մϴ�. �ٸ� ID�� ����ϼ���.");
                return;
            }

            Post newPost = new Post(targetPostId);

            for (int i = 0; i < pages.Count; i++)
            {
                Page newPage = new Page(i);

                textBoxes.RemoveAll(item => item == null);
                foreach (var textBox in textBoxes)
                {
                    if (textBox.transform.parent != pages[i].transform) continue;

                    TMP_Text textComponent = textBox.GetComponentInChildren<TMP_Text>();
                    if (textComponent == null) continue;

                    string content = textComponent.text;
                    int fontSize = (int)textComponent.fontSize;
                    string fontFace = textComponent.font.name;
                    bool isUnderlined = textComponent.fontStyle.HasFlag(FontStyles.Underline);
                    bool isStrikethrough = textComponent.fontStyle.HasFlag(FontStyles.Strikethrough);

                    newPage.elements.Add(new Element(
                        Element.ElementType.Text_Box,
                        content,
                        null,
                        textBox.transform.localPosition,
                        textBox.transform.localScale,
                        fontSize,
                        fontFace,
                        isUnderlined,
                        isStrikethrough
                    ));
                }

                imageBoxes.RemoveAll(item => item == null);
                foreach (var imageBox in imageBoxes)
                {
                    if (imageBox.transform.parent != pages[i].transform) continue;

                    Image imageComponent = imageBox.transform.GetChild(0).GetComponent<Image>();
                    string imageData = null;

                    if (imageComponent != null && imageComponent.sprite != null)
                    {
                        Texture2D texture = imageComponent.sprite.texture;
                        imageData = Element.EncodeImageToBase64(texture);
                    }

                    newPage.elements.Add(new Element(
                        Element.ElementType.Image_Box,
                        "",
                        imageData,
                        imageBox.transform.localPosition,
                        imageBox.transform.localScale
                    ));
                }

                newPost.pages.Add(newPage);
            }

            rootData.posts.Add(newPost);

            string json = JsonUtility.ToJson(rootData, true);
            File.WriteAllText(savePath, json);

            Debug.Log($"Data saved successfully. Post ID: {newPost.postId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    //public void CreateObjectsFromFile()
    //{
    //    try
    //    {
    //        Debug.Log("LoadObjectsFromFile() called.");

    //        if (!File.Exists(savePath))
    //        {
    //            Debug.LogWarning("Save file not found.");
    //            return;
    //        }

    //        string json = File.ReadAllText(savePath);
    //        rootData = JsonUtility.FromJson<RootObject>(json);

    //        if (rootData.posts.Count == 0) return;

    //        Post post = rootData.posts[0];

    //        textBoxes.ForEach(Destroy);
    //        imageBoxes.ForEach(Destroy);
    //        pages.ForEach(Destroy);

    //        textBoxes.Clear();
    //        imageBoxes.Clear();
    //        pages.Clear();

    //        foreach (var page in post.pages)
    //        {
    //            GameObject newPage = Instantiate(pagePrefab, pagesParentTransform);
    //            InitializePage(newPage);

    //            foreach (var element in page.elements)
    //            {
    //                if (element.type == Element.ElementType.Text_Box)
    //                {
    //                    GameObject newTextBox = Instantiate(textBoxPrefab, newPage.transform);
    //                    InitializeTextBox(newTextBox);

    //                    newTextBox.transform.localPosition = element.position;
    //                    newTextBox.transform.localScale = element.scale;

    //                    TMP_Text textComponent = newTextBox.GetComponentInChildren<TMP_Text>();
    //                    if (textComponent != null)
    //                    {
    //                        textComponent.text = element.content;
    //                        textComponent.fontSize = element.fontSize;

    //                        TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>($"Fonts/{element.fontFace}");
    //                        if (fontAsset != null) textComponent.font = fontAsset;

    //                        textComponent.fontStyle = FontStyles.Normal;
    //                        if (element.isUnderlined) textComponent.fontStyle |= FontStyles.Underline;
    //                        if (element.isStrikethrough) textComponent.fontStyle |= FontStyles.Strikethrough;
    //                    }
    //                }
    //                else if (element.type == Element.ElementType.Image_Box)
    //                {
    //                    GameObject newImageBox = Instantiate(imageBoxPrefab, newPage.transform);
    //                    InitializeImageBox(newImageBox);

    //                    newImageBox.transform.localPosition = element.position;
    //                    newImageBox.transform.localScale = element.scale;

    //                    Image imageComponent = newImageBox.transform.GetChild(0).GetComponent<Image>();
    //                    if (imageComponent != null && !string.IsNullOrEmpty(element.imageData))
    //                    {
    //                        Texture2D texture = Element.DecodeImageFromBase64(element.imageData);
    //                        imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //                    }
    //                }
    //            }
    //        }

    //        totalPages = pages.Count;
    //        UpdateScrollbar();

    //        // ��ũ�ѹ��� value�� 0���� �ʱ�ȭ
    //        pageScrollbar.value = 0f;

    //        Debug.Log("Data loaded successfully.");
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError($"Load failed: {e.Message}");
    //    }
    //}

    public void LoadSpecificPostById()
    {
        string targetPostId = loadPostIdField.text;

        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("��ȿ�� postId�� �Է��ϼ���.");
            return;
        }

        Post targetPost = rootData.posts.Find(post => post.postId == targetPostId);

        if (targetPost == null)
        {
            Debug.LogWarning($"postId '{targetPostId}'�� �ش��ϴ� �Խù��� �����ϴ�.");
            return;
        }

        textBoxes.ForEach(Destroy);
        imageBoxes.ForEach(Destroy);
        pages.ForEach(Destroy);

        textBoxes.Clear();
        imageBoxes.Clear();
        pages.Clear();

        foreach (var page in targetPost.pages)
        {
            GameObject newPage = Instantiate(pagePrefab, pagesParentTransform);
            InitializePage(newPage);

            foreach (var element in page.elements)
            {
                if (element.type == Element.ElementType.Text_Box)
                {
                    GameObject newTextBox = Instantiate(textBoxPrefab, newPage.transform);
                    InitializeTextBox(newTextBox);

                    newTextBox.transform.localPosition = element.position;
                    newTextBox.transform.localScale = element.scale;

                    TMP_Text textComponent = newTextBox.GetComponentInChildren<TMP_Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = element.content;
                        textComponent.fontSize = element.fontSize;

                        TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>($"Fonts/{element.fontFace}");
                        if (fontAsset != null) textComponent.font = fontAsset;

                        textComponent.fontStyle = FontStyles.Normal;
                        if (element.isUnderlined) textComponent.fontStyle |= FontStyles.Underline;
                        if (element.isStrikethrough) textComponent.fontStyle |= FontStyles.Strikethrough;
                    }
                }
                else if (element.type == Element.ElementType.Image_Box)
                {
                    GameObject newImageBox = Instantiate(imageBoxPrefab, newPage.transform);
                    InitializeImageBox(newImageBox);

                    newImageBox.transform.localPosition = element.position;
                    newImageBox.transform.localScale = element.scale;

                    Image imageComponent = newImageBox.transform.GetChild(0).GetComponent<Image>();
                    if (imageComponent != null && !string.IsNullOrEmpty(element.imageData))
                    {
                        Texture2D texture = Element.DecodeImageFromBase64(element.imageData);
                        imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    }
                }
            }
        }

        totalPages = pages.Count;

        // pageCount�� totalPages�� �����ϰ� ����
        createMgr.pageCount = totalPages; // createMgr�� CreateMgr_KJS Ŭ������ �ν��Ͻ� ����
        createMgr.UpdateContentWidth();
        createMgr.UpdateScrollbarSteps();

        UpdateScrollbar();

        // ��ũ�ѹ��� value�� 0���� �ʱ�ȭ
        pageScrollbar.value = 0f;

        Debug.Log($"Data loaded successfully for postId '{targetPostId}'.");
    }

    private void InitializePage(GameObject page)
    {
        page.name = $"Page_{System.Guid.NewGuid()}";
        pages.Add(page);

        Button btn_TextBox = page.transform.Find("btn_TextBox")?.GetComponent<Button>();
        if (btn_TextBox != null)
        {
            btn_TextBox.onClick.AddListener(() =>
            {
                GameObject newTextBox = Instantiate(textBoxPrefab, page.transform);
                InitializeTextBox(newTextBox);
            });
        }

        Button btn_ImageBox = page.transform.Find("btn_Image")?.GetComponent<Button>();
        if (btn_ImageBox != null)
        {
            btn_ImageBox.onClick.AddListener(() =>
            {
                GameObject newImageBox = Instantiate(imageBoxPrefab, page.transform);
                InitializeImageBox(newImageBox);
            });
        }

        Button deleteButton = page.transform.Find("btn_Delete")?.GetComponent<Button>();
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(() => RemovePage(page));
        }
    }

    private void InitializeTextBox(GameObject textBox)
    {
        textBox.name = $"TextBox_{System.Guid.NewGuid()}";
        textBoxes.Add(textBox);

        Button buttonContent = textBox.GetComponentInChildren<Button>();
        if (buttonContent != null)
        {
            buttonContent.name = $"{textBox.name}_Button";
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

    private void InitializeImageBox(GameObject imageBox)
    {
        imageBox.name = $"ImageBox_{System.Guid.NewGuid()}";
        imageBoxes.Add(imageBox);

        Button buttonContent = imageBox.GetComponentInChildren<Button>();
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

    private void UpdateScrollbar()
    {
        if (totalPages <= 1)
        {
            pageScrollbar.size = 1f;
            pageScrollbar.value = 1f;
            pageScrollbar.interactable = false;
        }
        else
        {
            pageScrollbar.size = 1f / totalPages;
            pageScrollbar.value = 1f;
            pageScrollbar.interactable = true;

            pageScrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        }
    }

    private void OnScrollbarValueChanged(float value)
    {
        float step = 1f / (totalPages - 1);
        int currentPage = Mathf.RoundToInt(value / step);
        float targetValue = currentPage * step;

        pageScrollbar.value = targetValue;

        Debug.Log($"Current Page: {currentPage + 1}/{totalPages}");
    }
}