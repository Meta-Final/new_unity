using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

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
    public List<Page> pages = new List<Page>();
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

    private void Start()
    {
        postMgr = FindObjectOfType<PostMgr>();

        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        saveButton.onClick.AddListener(SaveObjectsToFile);

        // 저장된 모든 Load 버튼에 이벤트 리스너 추가
        foreach (var button in loadButtons)
        {
            button.onClick.AddListener(() => LoadObjectsFromFile());
            button.onClick.AddListener(() => postMgr.ThumStart());
        }

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
        button.onClick.AddListener(() => LoadObjectsFromFile());
        button.onClick.AddListener(() => postMgr.ThumStart());
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
        try
        {
            rootData.posts.Clear();  // 기존 데이터 초기화

            Post newPost = new Post();  // 새 게시물 생성

            // 페이지별로 데이터 저장
            for (int i = 0; i < pages.Count; i++)
            {
                Page newPage = new Page(i);  // 새 페이지 생성

                // 텍스트 박스 처리
                textBoxes.RemoveAll(item => item == null);  // Null 오브젝트 제거
                foreach (var textBox in textBoxes)
                {
                    // 올바른 부모-자식 관계인지 확인
                    if (textBox.transform.parent != pages[i].transform) continue;

                    TMP_Text textComponent = textBox.GetComponentInChildren<TMP_Text>();
                    if (textComponent == null) continue;  // 텍스트 컴포넌트가 없으면 스킵

                    string content = textComponent.text;
                    int fontSize = (int)textComponent.fontSize;
                    string fontFace = textComponent.font.name;
                    bool isUnderlined = textComponent.fontStyle.HasFlag(FontStyles.Underline);
                    bool isStrikethrough = textComponent.fontStyle.HasFlag(FontStyles.Strikethrough);

                    // 텍스트 요소 추가
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

                    Debug.Log($"TextBox 저장됨: {content}");
                }

                // 이미지 박스 처리
                imageBoxes.RemoveAll(item => item == null);  // Null 오브젝트 제거
                foreach (var imageBox in imageBoxes)
                {
                    // 올바른 부모-자식 관계인지 확인
                    if (imageBox.transform.parent != pages[i].transform) continue;

                    Image imageComponent = imageBox.transform.GetChild(0).GetComponent<Image>();
                    string imageData = null;

                    if (imageComponent != null && imageComponent.sprite != null)
                    {
                        Texture2D texture = imageComponent.sprite.texture;
                        imageData = Element.EncodeImageToBase64(texture);
                    }

                    // 이미지 요소 추가
                    newPage.elements.Add(new Element(
                        Element.ElementType.Image_Box,
                        "",
                        imageData,
                        imageBox.transform.localPosition,
                        imageBox.transform.localScale
                    ));

                    Debug.Log($"ImageBox 저장됨: {imageData != null}");
                }

                // 페이지를 게시물에 추가
                newPost.pages.Add(newPage);
            }

            // 게시물을 루트 데이터에 추가
            rootData.posts.Add(newPost);


            // JSON 직렬화 및 저장
            string json = JsonUtility.ToJson(rootData, true);

            print(json);

            File.WriteAllText(savePath, json);

            Debug.Log("Data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }


    public void LoadObjectsFromFile()
    {
        print("!!!!!");
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

            textBoxes.ForEach(Destroy);
            imageBoxes.ForEach(Destroy);
            pages.ForEach(Destroy);

            textBoxes.Clear();
            imageBoxes.Clear();
            pages.Clear();

            foreach (var page in post.pages)
            {
                GameObject newPageObj = Instantiate(pagePrefab, pagesParentTransform);
                AddPage(newPageObj);

                foreach (var element in page.elements)
                {
                    GameObject newObj = null;

                    if (element.type == Element.ElementType.Text_Box)
                    {
                        newObj = Instantiate(textBoxPrefab, newPageObj.transform);
                        TMP_Text textComponent = newObj.GetComponentInChildren<TMP_Text>();
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
                        AddTextBox(newObj);
                    }
                    else if (element.type == Element.ElementType.Image_Box)
                    {
                        newObj = Instantiate(imageBoxPrefab, newPageObj.transform);
                        Image imageComponent = newObj.transform.GetChild(0).GetComponent<Image>();

                        if (!string.IsNullOrEmpty(element.imageData))
                        {
                            Texture2D texture = Element.DecodeImageFromBase64(element.imageData);
                            imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        }
                        AddImageBox(newObj);
                    }

                    if (newObj != null)
                    {
                        newObj.transform.localPosition = element.position;
                        newObj.transform.localScale = element.scale;
                    }
                }
            }

            totalPages = pages.Count;
            UpdateScrollbar();

            Debug.Log("Data loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
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