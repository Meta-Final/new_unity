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

    public Element(ElementType type, string content, string imageData, Vector3 position, Vector3 scale)
    {
        this.type = type;
        this.content = content;
        this.imageData = imageData;
        this.position = position;
        this.scale = scale;
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
    public int pageId;  // ������ ID �߰�
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

    public Transform parent;                // �ؽ�Ʈ�� �̹��� �θ�
    public Transform pagesParentTransform;  // ������ �θ�

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();  // ������ ����

    public Button saveButton;
    public Button loadButton;

    private string saveDirectory = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo";
    private string saveFileName = "Magazine.json";
    private string savePath;

    private RootObject rootData = new RootObject();
    private int pageCounter = 0;  // ������ ID ī����

    private void Start()
    {
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);

        saveButton.onClick.AddListener(SaveObjectsToFile);
        loadButton.onClick.AddListener(LoadObjectsFromFile);

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
        pageCounter++;  // �� ������ ���� �� ID ����
    }

    private void SaveObjectsToFile()
    {
        try
        {
            rootData.posts.Clear();  // ���� ������ �ʱ�ȭ

            Post newPost = new Post();

            // ������ ������ ����
            for (int i = 0; i < pages.Count; i++)
            {
                Page newPage = new Page(i);

                // �ؽ�Ʈ �ڽ� ����
                textBoxes.RemoveAll(item => item == null);
                foreach (var textBox in textBoxes)
                {
                    if (textBox.transform.parent != pages[i].transform) continue; // �θ� ������ ��Ī

                    TMP_Text textComponent = textBox.GetComponentInChildren<TMP_Text>();
                    string content = textComponent != null ? textComponent.text : "";

                    newPage.elements.Add(new Element(
                        Element.ElementType.Text_Box,
                        content,
                        null,
                        textBox.transform.localPosition,  // ���� ��ǥ�� ����
                        textBox.transform.localScale
                    ));
                }

                // �̹��� �ڽ� ����
                imageBoxes.RemoveAll(item => item == null);
                foreach (var imageBox in imageBoxes)
                {
                    if (imageBox.transform.parent != pages[i].transform) continue; // �θ� ������ ��Ī

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
                        imageBox.transform.localPosition,  // ���� ��ǥ�� ����
                        imageBox.transform.localScale
                    ));
                }

                newPost.pages.Add(newPage);
            }

            rootData.posts.Add(newPost);

            // JSON ����ȭ �� ����
            string json = JsonUtility.ToJson(rootData, true);
            File.WriteAllText(savePath, json);

            Debug.Log("Data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }

    private void LoadObjectsFromFile()
    {
        try
        {
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("Save file not found.");
                return;
            }

            string json = File.ReadAllText(savePath);
            rootData = JsonUtility.FromJson<RootObject>(json);

            if (rootData.posts.Count == 0)
            {
                Debug.LogWarning("No posts found.");
                return;
            }

            Post post = rootData.posts[0];  // ù ��° ����Ʈ �ε�

            // ���� ������Ʈ ����
            textBoxes.ForEach(Destroy);
            imageBoxes.ForEach(Destroy);
            pages.ForEach(Destroy);

            textBoxes.Clear();
            imageBoxes.Clear();
            pages.Clear();

            // �������� ��� ����
            foreach (var page in post.pages)
            {
                GameObject newPageObj = Instantiate(pagePrefab, pagesParentTransform);
                AddPage(newPageObj);

                foreach (var element in page.elements)
                {
                    GameObject newObj = null;

                    if (element.type == Element.ElementType.Text_Box)
                    {
                        newObj = Instantiate(textBoxPrefab, newPageObj.transform);  // ������ �ڽ����� �߰�
                        TMP_Text textComponent = newObj.GetComponentInChildren<TMP_Text>();
                        if (textComponent != null)
                        {
                            textComponent.text = element.content;
                        }
                        AddTextBox(newObj);
                    }
                    else if (element.type == Element.ElementType.Image_Box)
                    {
                        newObj = Instantiate(imageBoxPrefab, newPageObj.transform);  // ������ �ڽ����� �߰�
                        Image imageComponent = newObj.transform.GetChild(0).GetComponent<Image>();

                        if (!string.IsNullOrEmpty(element.imageData))
                        {
                            Texture2D texture = Element.DecodeImageFromBase64(element.imageData);
                            imageComponent.sprite = Sprite.Create(
                                texture,
                                new Rect(0, 0, texture.width, texture.height),
                                new Vector2(0.5f, 0.5f)
                            );
                        }
                        AddImageBox(newObj);
                    }

                    if (newObj != null)
                    {
                        newObj.transform.localPosition = element.position;  // ���� ��ǥ�� ����
                        newObj.transform.localScale = element.scale;
                    }
                }
            }

            Debug.Log("Data loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }
}