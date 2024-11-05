using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicketUIManager : MonoBehaviour
{
    public GameObject player;

    public GameObject panelInventory;   // �κ��丮 UI
    public GameObject panelLinkNews;    // Picket�̶� ��� ��ũ ���� Panel
    public GameObject panelPicket;      // Picket UI
    public GameObject currentPicket;    // ��� ������ Picket Prefab

    public Button btn_Yes;   // ��ũ 'Yes'
    public Button btn_No;    // ��ũ 'No'

    public Button btn_X;

    public Texture newsTexture;   // ��� ��ũ����
    public RawImage img_News;     // ��� ��ũ������ �����ִ� �̹���

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
        // ����, ������ Ŭ���ߴٸ�
        if (Input.GetMouseButtonDown(0))
        {
            // ���Ͽ� ��簡 ��ũ�Ǿ� �ִٸ� 
            if (img_News.texture != null)
            {
                // ��� UI ȭ���� �����.
                ClickPicketShowUI();
            }
        }
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

    public void ClickPicketShowUI()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.gameObject.layer == 18)
            {
                panelPicket.SetActive(true);
            }
        }
    }

    // X ��ư�� ������ ��
    public void OnClickXBtn()
    {
        // Picket UI ��Ȱ��ȭ
        panelPicket.SetActive(false);
    }
}
