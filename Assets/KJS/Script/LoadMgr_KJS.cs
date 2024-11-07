using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LoadMgr_KJS : MonoBehaviour
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

    private string saveDirectory = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo";
    private string saveFileName = "Magazine.dat"; // 확장자를 .json에서 .dat로 변경
    private string savePath;

    private RootObject rootData = new RootObject();

    private void Start()
    {
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        if (File.Exists(savePath))
        {
            LoadDataFromBinaryFile();
        }
    }

    // 특정 postId에 맞는 데이터를 로드하는 메서드
    public void LoadObjectsFromFile(string postId)
    {
        try
        {
            Debug.Log($"LoadObjectsFromFile() called for postId: {postId}");

            if (!File.Exists(savePath))
            {
                Debug.LogWarning("Save file not found.");
                return;
            }

            LoadDataFromBinaryFile();

            Post post = rootData.posts.Find(p => p.postId == postId);
            if (post == null)
            {
                Debug.LogWarning($"postId '{postId}'에 해당하는 게시물이 없습니다.");
                return;
            }

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

                        if (element.imageData != null && element.imageData.Length > 0)
                        {
                            Texture2D texture = Element.DecodeImageFromBytes(element.imageData);
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

            pageScrollbar.value = 0f;

            Debug.Log("Data loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }

    private void AddTextBox(GameObject textBox) => textBoxes.Add(textBox);
    private void AddImageBox(GameObject imageBox) => imageBoxes.Add(imageBox);
    private void AddPage(GameObject page) => pages.Add(page);

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

        pageScrollbar.value = 0f;
    }

    private void OnScrollbarValueChanged(float value)
    {
        float step = 1f / (totalPages - 1);
        int currentPage = Mathf.RoundToInt(value / step);
        float targetValue = currentPage * step;

        pageScrollbar.value = targetValue;

        Debug.Log($"Current Page: {currentPage + 1}/{totalPages}");
    }

    // 바이너리 파일로부터 데이터를 로드하는 메서드
    private void LoadDataFromBinaryFile()
    {
        try
        {
            using (FileStream fs = new FileStream(savePath, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    int postCount = reader.ReadInt32();
                    rootData.posts = new List<Post>();

                    for (int i = 0; i < postCount; i++)
                    {
                        string postId = reader.ReadString();
                        Post post = new Post(postId);

                        int pageCount = reader.ReadInt32();
                        for (int j = 0; j < pageCount; j++)
                        {
                            Page page = new Page(j);
                            int elementCount = reader.ReadInt32();

                            for (int k = 0; k < elementCount; k++)
                            {
                                Element.ElementType type = (Element.ElementType)reader.ReadInt32();
                                string content = type == Element.ElementType.Text_Box ? reader.ReadString() : null;

                                int imageDataLength = reader.ReadInt32();
                                byte[] imageData = imageDataLength > 0 ? reader.ReadBytes(imageDataLength) : null;

                                Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                                Vector3 scale = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                                int fontSize = reader.ReadInt32();
                                string fontFace = reader.ReadString();
                                bool isUnderlined = reader.ReadBoolean();
                                bool isStrikethrough = reader.ReadBoolean();

                                Element element = new Element(type, content, imageData, position, scale, fontSize, fontFace, isUnderlined, isStrikethrough);
                                page.elements.Add(element);
                            }
                            post.pages.Add(page);
                        }
                        rootData.posts.Add(post);
                    }
                }
            }
            Debug.Log("Data loaded from binary file successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data from binary file: {e.Message}");
        }
    }
}
