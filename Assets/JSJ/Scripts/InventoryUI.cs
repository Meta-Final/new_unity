using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("인벤토리")]
    public Transform slot;
    public RawImage img_Preview;
    public Button btn_SlotItemPrefab;
    public Button btn_Delete;

    [Header("포스트잇")]
    public GameObject postItPrefab;
    public Transform framePos;
    public Button btn_PostIt;

    int selectIndex = -1;
    Texture2D selectScreenshot;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "PostitTest_Scene")
        {
            framePos = GameObject.Find("FramePos").transform;
        }
        else
        {
            framePos = null;
        }

        btn_Delete.interactable = false;
        btn_PostIt.interactable = false;

        btn_PostIt.onClick.AddListener(() => OnPostitButtionClick());
    }

    // 인벤토리 UI 업데이트
    public void UpdateInventoryUI()
    {
        foreach (Transform child in slot)
        {
            // Slot 의 모든 자식 오브젝트를 제거
            Destroy(child.gameObject);
        }

        for (int i = 0; i < InventoryManager.instance.ScreenshotCount(); i++)
        {
            Button newButton = Instantiate(btn_SlotItemPrefab, slot);
            int index = i;

            newButton.onClick.AddListener(() => OnSlotClick(index));

        }
    }

    // 클릭하면 발동하는 함수
    public void OnSlotClick(int index)
    {
        selectIndex = index;

        InventoryManager.instance.ShowScreenshot(selectIndex);

        btn_Delete.interactable = true;
        btn_PostIt.interactable = true;

        btn_Delete.onClick.AddListener(() => OnSlotDeleteClick(selectIndex));
    }

    // 삭제 기능
    public void OnSlotDeleteClick(int index)
    {
        InventoryManager.instance.DeleteScreenshot(index);

        img_Preview.texture = null;

        btn_Delete.interactable = false;
        btn_PostIt.interactable = false;

        selectIndex = -1;

        UpdateInventoryUI();
    }

    // 이미지 로드
    public void DisplayScreenshot(string path)
    {
        StartCoroutine(LoadImage(path));
    }

    public IEnumerator LoadImage(string path)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("file:///" + path);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            img_Preview.texture = texture;
            selectScreenshot = texture;
        }
        else
        {
            Debug.LogError("이미지 로드 실패: " + request.error);
        }
    }

    // 포스트잇 기능
    public void OnPostitButtionClick()
    {
        if (selectScreenshot != null)
        {
            GameObject newPostIt = Instantiate(postItPrefab, framePos);

            RawImage postItImage = newPostIt.GetComponentInChildren<RawImage>();

            if (postItImage != null)
            {
                postItImage.texture = selectScreenshot;

                print("1");
                float originWidth = selectScreenshot.width;
                float originHeight = selectScreenshot.height;
                float aspectRatio = originWidth / originHeight;

                //RectTransform postItRectTransform = newPostIt.GetComponent<RectTransform>();

                //if (postItRectTransform != null)
                //{
                //    print("22222");
                //    float baseWidth = 0.8f;
                //    float baseHeight = baseWidth / aspectRatio;


                //    postItRectTransform.sizeDelta = new Vector2(baseWidth, baseHeight);

                //    RectTransform imageRectTransform = postItImage.GetComponent<RectTransform>();
                //    if (imageRectTransform != null)
                //    {
                //        print("33333");
                //        imageRectTransform.sizeDelta = new Vector2(baseWidth, baseHeight);
                //    }
                //}

                // Scale
                
            }
        }

        btn_Delete.interactable = false;
        btn_PostIt.interactable = false;
    }
}                
