using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Element
{
    public string content;
    public enum ElementType { Text_Box, Image_Box }
    public ElementType type;
    public byte[] imageData; // string 대신 byte[]로 변경
    public Vector3 position;
    public Vector3 scale;

    public int fontSize;
    public string fontFace;
    public bool isUnderlined;
    public bool isStrikethrough;

    public Element(
        ElementType type, string content, byte[] imageData, Vector3 position, Vector3 scale,
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

    public static byte[] EncodeImageToBytes(Texture2D texture)
    {
        return texture != null ? texture.EncodeToPNG() : null;
    }

    public static Texture2D DecodeImageFromBytes(byte[] imageData)
    {
        if (imageData == null || imageData.Length == 0) return null;
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
        Debug.Log($"버튼 {button.name}이 추가되었습니다.");
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
        pages.Add(page); // pages 리스트에 페이지 추가
        totalPages = pages.Count; // totalPages를 pages.Count로 업데이트
        Debug.Log($"Page added. Current totalPages: {totalPages}");
    }

    public void RemovePage(GameObject page)
    {
        pages.Remove(page); // pages 리스트에서 페이지 제거
        totalPages = pages.Count; // totalPages를 pages.Count로 업데이트
        Debug.Log($"Page removed. Current totalPages: {totalPages}");
    }

    private void SaveObjectsToFile()
    {
        // 로그인 확인
        if (!MetaFireAuth.instance.IsLoggedIn)
        {
            Debug.LogError("Firebase에 로그인되어 있지 않습니다. 자동 로그인을 시도합니다.");
            MetaFireAuth.instance.SignInWithTemporaryAccount();
            return;
        }

        string targetPostId = inputPostIdField.text;

        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("유효한 postId를 입력하세요.");
            return;
        }

        try
        {
            // 동일한 postId가 이미 있는지 확인
            Post existingPost = rootData.posts.Find(post => post.postId == targetPostId);

            // 동일한 ID의 게시물이 있다면 덮어쓰고, 없으면 새로 생성
            Post targetPost = existingPost ?? new Post(targetPostId);
            if (existingPost == null)
            {
                rootData.posts.Add(targetPost);
            }
            else
            {
                // 기존 게시물 내용을 비웁니다.
                targetPost.pages.Clear();
            }

            // 유효한 textBox와 imageBox만 필터링
            textBoxes.RemoveAll(item => item == null);
            imageBoxes.RemoveAll(item => item == null);
            pages.RemoveAll(item => item == null);

            // 페이지 및 요소 생성
            for (int i = 0; i < pages.Count; i++)
            {
                Page newPage = new Page(i);

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

                foreach (var imageBox in imageBoxes)
                {
                    if (imageBox.transform.parent != pages[i].transform) continue;

                    Image imageComponent = imageBox.transform.GetChild(0).GetComponent<Image>();
                    byte[] imageData = null;

                    if (imageComponent != null && imageComponent.sprite != null)
                    {
                        Texture2D texture = imageComponent.sprite.texture;
                        imageData = Element.EncodeImageToBytes(texture);
                    }

                    newPage.elements.Add(new Element(
                        Element.ElementType.Image_Box,
                        "",
                        imageData,
                        imageBox.transform.localPosition,
                        imageBox.transform.localScale
                    ));
                }

                targetPost.pages.Add(newPage);
            }

            // JSON 데이터를 직렬화하고 로컬 파일에 저장
            string json = JsonUtility.ToJson(rootData, true);
            File.WriteAllText(savePath, json);

            Debug.Log($"Data saved locally at {savePath}. Post ID: {targetPost.postId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    public void LoadSpecificPostById()
    {
        string targetPostId = loadPostIdField.text;

        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("유효한 postId를 입력하세요.");
            return;
        }

        Post targetPost = rootData.posts.Find(post => post.postId == targetPostId);

        if (targetPost == null)
        {
            Debug.LogWarning($"postId '{targetPostId}'에 해당하는 게시물이 없습니다.");
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
                    if (imageComponent != null && element.imageData != null && element.imageData.Length > 0)
                    {
                        Texture2D texture = Element.DecodeImageFromBytes(element.imageData);
                        imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    }
                }
            }
        }

        totalPages = pages.Count;

        createMgr.pageCount = totalPages;
        createMgr.UpdateContentWidth();
        createMgr.UpdateScrollbarSteps();

        UpdateScrollbar();

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
                    Debug.LogError("EditorMgr_KJS가 할당되지 않았습니다.");
                }
            });
        }
        else
        {
            Debug.LogError("텍스트 박스 프리팹에 Button 컴포넌트가 없습니다.");
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
            Debug.LogError("ImageBox 프리팹에 Button 컴포넌트가 없습니다.");
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