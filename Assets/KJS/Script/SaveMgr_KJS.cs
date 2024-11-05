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

    // 추가된 필드들
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
    public int pageId;  // 페이지 ID 추가
    public List<Element> elements = new List<Element>();

    public Page(int id)
    {
        pageId = id;
    }
}

[System.Serializable]
public class Post
{
    public string postId;  // postId를 string으로 변경
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

    private int totalPages;

    public Transform parent;
    public Transform pagesParentTransform;

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    public Button saveButton;
    public List<Button> loadButtons = new List<Button>();  // Load 버튼을 List로 관리

    private string saveDirectory = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo";
    private string saveFileName = "Magazine.json";
    private string savePath;

    private RootObject rootData = new RootObject();
    private int pageCounter = 0;
    private PostMgr postMgr;

    private int postCounter = 0;  // 추가된 필드: 저장될 때마다 증가할 Post ID
    public TMP_InputField inputPostIdField;

    private void Start()
    {

        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        // 중복된 이벤트 리스너 방지를 위해 기존 리스너 모두 제거
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

    // 버튼을 List에 추가하고 이벤트 리스너를 연결하는 메서드
    public void SetLoadButton(Button button)
    {
        loadButtons.Add(button);  // List<Button>에 추가
        Debug.Log($"버튼 {button.name}이 추가되었습니다.");

        // 동적으로 추가된 버튼에 클릭 이벤트 등록
        button.onClick.AddListener(() => CreateObjectsFromFile());
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
    public void AddPage(GameObject page) => pages.Add(page);
    public void RemovePage(GameObject page) => pages.Remove(page);

    public void CreateNewPage()
    {
        GameObject newPageObj = Instantiate(pagePrefab, pagesParentTransform);
        AddPage(newPageObj);
        pageCounter++;
    }

    private void SaveObjectsToFile()
    {
        string targetPostId = inputPostIdField.text;

        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("유효한 postId를 입력하세요.");
            return;
        }

        try
        {
            // 기존 데이터에서 동일한 postId의 게시물이 있는지 확인
            Post existingPost = rootData.posts.Find(post => post.postId == targetPostId);
            if (existingPost != null)
            {
                Debug.LogError($"postId '{targetPostId}'에 해당하는 게시물이 이미 존재합니다. 다른 ID를 사용하세요.");
                return;
            }

            // 새 게시물 생성 및 입력된 postId 사용
            Post newPost = new Post(targetPostId);

            // 페이지별로 데이터 저장
            for (int i = 0; i < pages.Count; i++)
            {
                Page newPage = new Page(i);  // 새 페이지 생성

                // 텍스트 박스 처리
                textBoxes.RemoveAll(item => item == null);  // Null 오브젝트 제거
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

                // 이미지 박스 처리
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

                newPost.pages.Add(newPage);  // 페이지를 게시물에 추가
            }

            rootData.posts.Add(newPost);  // 새 게시물을 기존 데이터에 추가

            // JSON 직렬화 및 저장
            string json = JsonUtility.ToJson(rootData, true);
            File.WriteAllText(savePath, json);

            Debug.Log($"Data saved successfully. Post ID: {newPost.postId}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
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

        Post post = rootData.posts[0];

        // 기존에 생성된 오브젝트들 제거
        textBoxes.ForEach(Destroy);
        imageBoxes.ForEach(Destroy);
        pages.ForEach(Destroy);

        textBoxes.Clear();
        imageBoxes.Clear();
        pages.Clear();

        foreach (var page in post.pages)
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

        // 페이지 수 업데이트 및 스크롤바 설정
        totalPages = pages.Count;
        UpdateScrollbar();

        Debug.Log("Data loaded successfully.");
    }
    catch (Exception e)
    {
        Debug.LogError($"Load failed: {e.Message}");
    }
}
    public void LoadSpecificPostById()
    {
        string targetPostId = inputPostIdField.text;

        if (string.IsNullOrWhiteSpace(targetPostId))
        {
            Debug.LogError("유효한 postId를 입력하세요.");
            return;
        }

        // 해당 postId의 게시물을 찾습니다.
        Post targetPost = rootData.posts.Find(post => post.postId == targetPostId);

        if (targetPost == null)
        {
            Debug.LogWarning($"postId '{targetPostId}'에 해당하는 게시물이 없습니다.");
            return;
        }

        // 기존에 생성된 오브젝트들 제거
        textBoxes.ForEach(Destroy);
        imageBoxes.ForEach(Destroy);
        pages.ForEach(Destroy);

        textBoxes.Clear();
        imageBoxes.Clear();
        pages.Clear();

        // 찾은 게시물에 맞는 오브젝트 생성
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

        // 페이지 수 업데이트 및 스크롤바 설정
        totalPages = pages.Count;
        UpdateScrollbar();

        Debug.Log($"Data loaded successfully for postId '{targetPostId}'.");
    }

    private void InitializePage(GameObject page)
    {
        page.name = $"Page_{System.Guid.NewGuid()}";
        pages.Add(page);

        // 페이지 내의 버튼 연결
        Button btn_TextBox = page.transform.Find("btn_TextBox")?.GetComponent<Button>();
        if (btn_TextBox != null)
        {
            btn_TextBox.onClick.AddListener(() =>
            {
                GameObject newTextBox = Instantiate(textBoxPrefab, page.transform);
                InitializeTextBox(newTextBox); // 텍스트 박스 초기화
            });
        }

        Button btn_ImageBox = page.transform.Find("btn_Image")?.GetComponent<Button>();
        if (btn_ImageBox != null)
        {
            btn_ImageBox.onClick.AddListener(() =>
            {
                GameObject newImageBox = Instantiate(imageBoxPrefab, page.transform);
                InitializeImageBox(newImageBox); // 이미지 박스 초기화
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