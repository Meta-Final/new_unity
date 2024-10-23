using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveMgr_KJS : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject imageBoxPrefab;
    public GameObject pagePrefab;

    public Transform parent;  // 기존 텍스트와 이미지 박스의 부모 트랜스폼
    public Transform pagesParentTransform;  // 페이지 프리팹 생성 위치 지정

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    public List<string> userIds;
    public int selectedUserIndex = 0;

    public Button saveButton;
    public Button loadButton;

    private string saveDirectory = @"C:\Users\Admin\Documents\GitHub\unity\Assets\KJS\UserInfo";
    private string saveFileName = "Magazine.json";
    private string savePath;

    private Dictionary<string, UserPosts> userData = new Dictionary<string, UserPosts>();

    private void Start()
    {
        savePath = Path.Combine(saveDirectory, saveFileName);

        saveButton.onClick.AddListener(SaveObjectsToFile);
        loadButton.onClick.AddListener(LoadObjectsFromFile);

        EnsureDirectoryExists();

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            userData = JsonUtility.FromJson<SerializableDictionary>(json).ToDictionary();
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

    private string GetSelectedUserId()
    {
        if (userIds == null || userIds.Count == 0)
        {
            Debug.LogWarning("User ID list is empty.");
            return null;
        }

        if (selectedUserIndex < 0 || selectedUserIndex >= userIds.Count)
        {
            Debug.LogWarning("Invalid user index selected.");
            return null;
        }

        return userIds[selectedUserIndex];
    }

    public void AddTextBox(GameObject textBox) => textBoxes.Add(textBox);
    public void AddImageBox(GameObject imageBox) => imageBoxes.Add(imageBox);
    public void AddPage(GameObject page) => pages.Add(page);
    public void RemovePage(GameObject page) => pages.Remove(page);

    private void SaveObjectsToFile()
    {
        string userId = GetSelectedUserId();
        if (userId == null) return;

        if (!userData.ContainsKey(userId))
        {
            userData[userId] = new UserPosts();
        }

        UserPosts posts = userData[userId];
        posts.posts.Clear();

        // 유효한 텍스트 박스만 저장
        textBoxes.RemoveAll(item => item == null);  // null 오브젝트 제거
        foreach (var textBox in textBoxes)
        {
            TMP_Text textComponent = textBox.GetComponentInChildren<TMP_Text>();
            string content = textComponent != null ? textComponent.text : "";

            posts.posts.Add(new PostInfo(
                "TextBox",
                content,
                null,
                textBox.transform.position,
                textBox.transform.localScale
            ));
        }

        // 유효한 이미지 박스만 저장
        imageBoxes.RemoveAll(item => item == null);  // null 오브젝트 제거
        foreach (var imageBox in imageBoxes)
        {
            Image imageComponent = imageBox.transform.GetChild(0).GetComponent<Image>();
            byte[] imageData = null;

            if (imageComponent != null && imageComponent.sprite != null)
            {
                Texture2D texture = imageComponent.sprite.texture;
                imageData = texture.EncodeToPNG();
            }

            posts.posts.Add(new PostInfo(
                "ImageBox",
                "",
                imageData,
                imageBox.transform.position,
                imageBox.transform.localScale
            ));
        }

        // 유효한 페이지만 저장
        pages.RemoveAll(item => item == null);  // null 오브젝트 제거
        foreach (var page in pages)
        {
            posts.posts.Add(new PostInfo(
                "Page",
                "",
                null,
                page.transform.position,
                page.transform.localScale
            ));
        }

        // JSON으로 변환 후 파일에 저장
        string json = JsonUtility.ToJson(new SerializableDictionary(userData), true);
        File.WriteAllText(savePath, json);

        Debug.Log($"Data saved for User ID: {userId}");
    }

    private void LoadObjectsFromFile()
    {
        string userId = GetSelectedUserId();
        if (userId == null) return;

        if (!userData.ContainsKey(userId))
        {
            Debug.LogWarning($"No data found for User ID: {userId}");
            return;
        }

        UserPosts posts = userData[userId];

        // 기존 오브젝트 제거
        textBoxes.ForEach(Destroy);
        imageBoxes.ForEach(Destroy);
        pages.ForEach(Destroy);

        textBoxes.Clear();
        imageBoxes.Clear();
        pages.Clear();

        // 저장된 데이터로부터 객체 생성
        foreach (var post in posts.posts)
        {
            GameObject newObj = null;

            if (post.type == "TextBox")
            {
                newObj = Instantiate(textBoxPrefab, parent);
                TMP_Text textComponent = newObj.GetComponentInChildren<TMP_Text>();
                if (textComponent != null)
                {
                    textComponent.text = post.content;
                }
                AddTextBox(newObj);
            }
            else if (post.type == "ImageBox")
            {
                newObj = Instantiate(imageBoxPrefab, parent);
                Image imageComponent = newObj.transform.GetChild(0).GetComponent<Image>();

                byte[] imageData = post.GetImageData();

                if (imageData != null && imageData.Length > 0)
                {
                    Texture2D texture = new Texture2D(2, 2);
                    texture.LoadImage(imageData);
                    imageComponent.sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f)
                    );
                }
                AddImageBox(newObj);
            }
            else if (post.type == "Page")
            {
                // 페이지 프리팹 생성 시 지정된 pagesParentTransform을 부모로 사용
                newObj = Instantiate(pagePrefab, pagesParentTransform);
                AddPage(newObj);
            }

            if (newObj != null)
            {
                newObj.transform.position = post.position;
                newObj.transform.localScale = post.scale;
            }
        }

        Debug.Log($"Data loaded for User ID: {userId}");
    }
}

[System.Serializable]
public class PostInfo
{
    public string type;
    public string content;
    public string imageData; // Base64 문자열로 이미지 데이터 저장
    public Vector3 position;
    public Vector3 scale;

    public PostInfo(string type, string content, byte[] imageData, Vector3 position, Vector3 scale)
    {
        this.type = type;
        this.content = content;
        this.imageData = imageData != null ? Convert.ToBase64String(imageData) : null;
        this.position = position;
        this.scale = scale;
    }

    public byte[] GetImageData()
    {
        return string.IsNullOrEmpty(imageData) ? null : Convert.FromBase64String(imageData);
    }
}

[System.Serializable]
public class UserPosts
{
    public List<PostInfo> posts = new List<PostInfo>();
}

[System.Serializable]
public class SerializableDictionary
{
    public List<string> keys = new List<string>();
    public List<UserPosts> values = new List<UserPosts>();

    public SerializableDictionary(Dictionary<string, UserPosts> dictionary)
    {
        foreach (var kvp in dictionary)
        {
            keys.Add(kvp.Key);
            values.Add(kvp.Value);
        }
    }

    public Dictionary<string, UserPosts> ToDictionary()
    {
        var dict = new Dictionary<string, UserPosts>();
        for (int i = 0; i < keys.Count; i++)
        {
            dict[keys[i]] = values[i];
        }
        return dict;
    }
}