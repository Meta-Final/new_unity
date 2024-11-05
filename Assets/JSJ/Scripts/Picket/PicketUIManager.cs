using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicketUIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject currentPicket;    // 방금 생성된 Picket Prefab

    public GameObject panelInventory;   // 인벤토리 UI
    public GameObject panelLinkNews;    // Picket이랑 기사 링크 여부 Panel
    public GameObject panelPicket;      // Picket UI
    

    public Button btn_Yes;   // 링크 'Yes'
    public Button btn_No;    // 링크 'No'

    public Button btn_X;

    public Texture newsTexture;   // 기사 스크린샷
    public RawImage img_News;     // 기사 스크린샷을 보여주는 이미지

    public DrawWithMouse drawWithMouse;

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;

        if (player != null)
        {
            panelInventory = player.transform.Find("Canvas_Inventory/Panel_Inventory")?.gameObject;
        }

        btn_Yes.onClick.AddListener(OnClickYesBtn);
        btn_No.onClick.AddListener(OnClickNoBtn);
        btn_X.onClick.AddListener(OnClickXBtn);

        panelInventory.SetActive(false);
        panelLinkNews.SetActive(false);
        panelPicket.SetActive(false);

        if (newsTexture != null)
        {
            img_News.texture = newsTexture;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                // 만일, 기사가 링크된 피켓을 클릭했다면
                if (hitInfo.collider.gameObject.layer == 18 && img_News.texture != null)
                {
                    ShowPicketUI();
                }
            }
        }
    }

    public void ShowPicketUI()
    {
        // Picket UI 활성화
        panelPicket.SetActive(true);

        // 라인 오브젝트 활성화
        drawWithMouse.lineParent.SetActive(true);
        // 스티커 오브젝트 활성화
        drawWithMouse.stickerParent.SetActive(true);
    }

   
    // 질문에 'Yes'버튼을 눌렀을 때
    public void OnClickYesBtn()
    {
        if (panelLinkNews.activeSelf)
        {
            // 질문 UI 비활성화
            panelLinkNews.SetActive(false);

            // 인벤토리 UI 활성화
            panelInventory.SetActive(true);
        }
    }

    // 질문에 'No'버튼을 눌렀을 때
    public void OnClickNoBtn()
    {
        if (panelLinkNews.activeSelf)
        {
            // 질문 UI 비활성화
            panelLinkNews.SetActive(false);

            // 방금 생성된 Picket 제거
            Destroy(currentPicket);
        }
    }

    // X 버튼을 눌럿을 때
    public void OnClickXBtn()
    {
        // Picket UI 비활성화
        panelPicket.SetActive(false);

        // 라인 오브젝트 비활성화
        drawWithMouse.lineParent.SetActive(false);
        // 스티커 오브젝트 비활성화
        drawWithMouse.stickerParent.SetActive(false);

        drawWithMouse.isDrawing = false;
        drawWithMouse.isAttaching = false;
    }
}
