using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicketUIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject currentPicket;    // ��� ������ Picket Prefab

    public GameObject panelInventory;   // �κ��丮 UI
    public GameObject panelLinkNews;    // Picket�̶� ��� ��ũ ���� Panel
    public GameObject panelPicket;      // Picket UI
    

    public Button btn_Yes;   // ��ũ 'Yes'
    public Button btn_No;    // ��ũ 'No'

    public Button btn_X;

    public Texture newsTexture;   // ��� ��ũ����
    public RawImage img_News;     // ��� ��ũ������ �����ִ� �̹���

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
                // ����, ��簡 ��ũ�� ������ Ŭ���ߴٸ�
                if (hitInfo.collider.gameObject.layer == 18 && img_News.texture != null)
                {
                    ShowPicketUI();
                }
            }
        }
    }

    public void ShowPicketUI()
    {
        // Picket UI Ȱ��ȭ
        panelPicket.SetActive(true);

        // ���� ������Ʈ Ȱ��ȭ
        drawWithMouse.lineParent.SetActive(true);
        // ��ƼĿ ������Ʈ Ȱ��ȭ
        drawWithMouse.stickerParent.SetActive(true);
    }

   
    // ������ 'Yes'��ư�� ������ ��
    public void OnClickYesBtn()
    {
        if (panelLinkNews.activeSelf)
        {
            // ���� UI ��Ȱ��ȭ
            panelLinkNews.SetActive(false);

            // �κ��丮 UI Ȱ��ȭ
            panelInventory.SetActive(true);
        }
    }

    // ������ 'No'��ư�� ������ ��
    public void OnClickNoBtn()
    {
        if (panelLinkNews.activeSelf)
        {
            // ���� UI ��Ȱ��ȭ
            panelLinkNews.SetActive(false);

            // ��� ������ Picket ����
            Destroy(currentPicket);
        }
    }

    // X ��ư�� ������ ��
    public void OnClickXBtn()
    {
        // Picket UI ��Ȱ��ȭ
        panelPicket.SetActive(false);

        // ���� ������Ʈ ��Ȱ��ȭ
        drawWithMouse.lineParent.SetActive(false);
        // ��ƼĿ ������Ʈ ��Ȱ��ȭ
        drawWithMouse.stickerParent.SetActive(false);

        drawWithMouse.isDrawing = false;
        drawWithMouse.isAttaching = false;
    }
}
