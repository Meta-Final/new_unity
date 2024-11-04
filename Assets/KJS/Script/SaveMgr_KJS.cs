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

    public int fontSize;
    public string fontFace;
    public bool isUnderlined;
    public bool isStrikethrough;

    public Element(ElementType type, string content, string imageData, Vector3 position, Vector3 scale,
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
    private string saveDirectory;
    private string saveFileName = "Magazine.json";
    private string savePath;

    public Transform parent;
    public Transform pagesParentTransform;

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    public Button saveButton;
    public List<Button> loadButtons = new List<Button>();

    private RootObject rootData = new RootObject();
    private int pageCounter = 0;

    private void Start()
    {
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);
        EnsureDirectoryExists();
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

    public void SaveObjectsToFile()
    {
        try
        {
            rootData.posts.Clear();
            Post newPost = new Post();

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

            Debug.Log("Data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }
}