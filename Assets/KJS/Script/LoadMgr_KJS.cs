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
    public Scrollbar scrollbar;  // 스크롤바 참조

    public List<GameObject> textBoxes = new List<GameObject>();
    public List<GameObject> imageBoxes = new List<GameObject>();
    public List<GameObject> pages = new List<GameObject>();

    public GameObject magazineView;  // MagazineView 오브젝트 참조

    private string savePath = @"C:\Users\Admin\Documents\GitHub\new_unity\Assets\KJS\UserInfo\Magazine.json";  // 고정된 저장 경로
    private RootObject rootData;

    public void Initialize(RootObject data)
    {
        rootData = data;

        // MagazineView는 초기 상태에서 비활성화합니다.
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

            // 기존 오브젝트 삭제
            textBoxes.ForEach(Destroy);
            imageBoxes.ForEach(Destroy);
            pages.ForEach(Destroy);

            textBoxes.Clear();
            imageBoxes.Clear();
            pages.Clear();

            // 로드된 데이터로 오브젝트 생성
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

            // 스크롤바 스텝 설정
            scrollbar.numberOfSteps = pages.Count;

            // MagazineView 활성화
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