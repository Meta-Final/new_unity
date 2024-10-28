using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Loader : MonoBehaviour
{
    public GameObject textBoxPrefab;
    public GameObject imageBoxPrefab;
    public GameObject pagePrefab;

    public Transform pagesParentTransform;
    public Scrollbar scrollbar;  // ��ũ�ѹ� ����

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    public GameObject magazineView;  // MagazineView ������Ʈ ����

    private string savePath = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo\Magazine.json";  // ������ ���� ���
    private RootObject rootData;

    public void Initialize(RootObject data)
    {
        rootData = data;

        // MagazineView�� �ʱ� ���¿��� ��Ȱ��ȭ�մϴ�.
        if (magazineView != null)
            magazineView.SetActive(false);
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

            Post post = rootData.posts[0];

            // ���� ������Ʈ ����
            textBoxes.ForEach(Destroy);
            imageBoxes.ForEach(Destroy);
            pages.ForEach(Destroy);

            textBoxes.Clear();
            imageBoxes.Clear();
            pages.Clear();

            // �ε�� �����ͷ� ������Ʈ ����
            foreach (var page in post.pages)
            {
                GameObject newPageObj = Instantiate(pagePrefab, pagesParentTransform);
                pages.Add(newPageObj);

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
                            textComponent.fontSize = (float)element.fontSize;

                            TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>($"Fonts/{element.fontFace}");
                            if (fontAsset != null) textComponent.font = fontAsset;

                            textComponent.fontStyle = FontStyles.Normal;
                            if (element.isUnderlined) textComponent.fontStyle |= FontStyles.Underline;
                            if (element.isStrikethrough) textComponent.fontStyle |= FontStyles.Strikethrough;
                        }
                        textBoxes.Add(newObj);
                    }
                    else if (element.type == Element.ElementType.Image_Box)
                    {
                        newObj = Instantiate(imageBoxPrefab, newPageObj.transform);
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

                    if (newObj != null)
                    {
                        newObj.transform.localPosition = element.position;
                        newObj.transform.localScale = element.scale;
                    }
                }
            }

            // ��ũ�ѹ� ���� ����
            scrollbar.numberOfSteps = pages.Count;

            // MagazineView Ȱ��ȭ
            if (magazineView != null)
            {
                magazineView.SetActive(true);
                Debug.Log("MagazineView activated.");
            }

            Debug.Log("Data loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Load failed: {e.Message}");
        }
    }
}