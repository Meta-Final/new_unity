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
    public RawImage img_Preview;        // 스크린샷 미리보기 화면
    public Button btn_SlotItemPrefab;   // 스크린샷 프리팹
    public Button btn_Delete;           // 스크린샷 삭제 버튼

    [Header("포스트잇")]
    public GameObject postItPrefab;     // 포스트잇 프리팹
    public Transform noticePos;         // 액자 위치
    public Button btn_PostIt;           // 포스트잇 생성 버튼

    int selectIndex = -1;
    Texture2D selectScreenshot;

    private void Start()
    {
        // 지금 'Meta_Studio_Scene'이라면, 액자를 찾아라.
        if (SceneManager.GetActiveScene().name == "Meta_Studio_Scene")
        {
            noticePos = GameObject.Find("NoticeFrame").transform;
        }
        else
        {
            noticePos = null;
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

    // 스크린샷 클릭하면 발동하는 함수
    public void OnSlotClick(int index)
    {
        selectIndex = index;

        InventoryManager.instance.ShowScreenshot(selectIndex);

        btn_Delete.interactable = true;
        btn_PostIt.interactable = true;

        btn_Delete.onClick.AddListener(() => OnSlotDeleteClick(selectIndex));
    }

    // 스크린샷 삭제 버튼 기능
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

    // 포스트잇 생성 버튼 기능
    public void OnPostitButtionClick()
    {
        if (selectScreenshot != null)
        {
            GameObject newPostIt = Instantiate(postItPrefab, noticePos);

            RawImage postItImage = newPostIt.GetComponentInChildren<RawImage>();

            if (postItImage != null)
            {
                postItImage.texture = selectScreenshot;

                print("1");
                float originWidth = selectScreenshot.width;
                float originHeight = selectScreenshot.height;
                float aspectRatio = originWidth / originHeight;
            }
        }

        btn_Delete.interactable = false;
        btn_PostIt.interactable = false;
    }
}                
