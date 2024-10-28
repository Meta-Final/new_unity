using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadMgr_KJS : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject imageBoxPrefab;
    public GameObject pagePrefab;

    public Transform pagesParentTransform;  // 페이지 부모
    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    private string saveDirectory;
    private string saveFileName = "Magazine.json";
    private string savePath;

    private RootObject rootData = new RootObject();

    private void Awake()
    {
        saveDirectory = Application.dataPath + "/KJS/UserInfo";
        savePath = Path.Combine(saveDirectory, saveFileName);
    }

    public void LoadObjectsFromFile()
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

            Post post = rootData.posts[0];  // 첫 번째 포스트 로드

            // 기존 오브젝트 삭제
            ClearExistingObjects();

            // 페이지와 요소 생성
            foreach (var page in post.pages)
            {
                GameObject newPageObj = Instantiate(pagePrefab, pagesParentTransform);
                pages.Add(newPageObj);

                foreach (var element in page.elements)
                {
                    GameObject newObj = CreateElement(element, newPageObj.transform);

                    if (newObj != null)
                    {
                        newObj.transform.localPosition = element.position;  // 로컬 좌표 복원
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

    private void ClearExistingObjects()
    {
        textBoxes.ForEach(Destroy);
        imageBoxes.ForEach(Destroy);
        pages.ForEach(Destroy);

        textBoxes.Clear();
        imageBoxes.Clear();
        pages.Clear();
    }

    private GameObject CreateElement(Element element, Transform parent)
    {
        GameObject newObj = null;

        if (element.type == Element.ElementType.Text_Box)
        {
            newObj = Instantiate(textBoxPrefab, parent);
            TMP_Text textComponent = newObj.GetComponentInChildren<TMP_Text>();

            if (textComponent != null)
            {
                textComponent.text = element.content;
                textComponent.fontSize = (float)element.fontSize;

                // 폰트 설정 (Resources 폴더에서 폰트 로드)
                TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>($"Fonts/{element.fontFace}");
                if (fontAsset != null)
                {
                    textComponent.font = fontAsset;
                }

                // 밑줄과 취소선 적용
                textComponent.fontStyle = FontStyles.Normal;
                if (element.isUnderlined) textComponent.fontStyle |= FontStyles.Underline;
                if (element.isStrikethrough) textComponent.fontStyle |= FontStyles.Strikethrough;
            }
            textBoxes.Add(newObj);
        }
        else if (element.type == Element.ElementType.Image_Box)
        {
            newObj = Instantiate(imageBoxPrefab, parent);
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
            imageBoxes.Add(newObj);
        }

        return newObj;
    }
}