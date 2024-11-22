using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PicketUIManager : MonoBehaviour
{
    public GameObject player;
    public GameObject currentPicket; // ��� ������ Picket Prefab

    [Header("Picket UI")]
    public GameObject panelInventory; // �κ��丮 UI
    public GameObject panelLinkNews; // Picket�̶� ��� ��ũ ���� Panel
    public GameObject panelPicket; // Picket UI

    [Header("Button")]
    public Button btn_Yes; // ��ũ 'Yes'
    public Button btn_No; // ��ũ 'No'
    public Button btn_X;

    [Header("Screenshot")]
    public RawImage img_News; // ��� ��ũ������ �����ִ� �̹���

    public bool isShowPicketUI = false;

    public DrawWithMouse drawWithMouse;

    [Header("Inventory Data")]
    public List<string> screenshotList; // InventoryManager���� ������ ��ũ���� ��� ����Ʈ

    void Start()
    {
        player = FindObjectOfType<GameManager>().player;

        if (player != null)
        {
            panelInventory = player.transform.Find("Canvas_Inventory/Panel_Inventory")?.gameObject;
        }

        // InventoryManager���� ��ũ���� ������ ��������
        if (InventoryManager.instance != null)
        {
            screenshotList = InventoryManager.instance.GetScreenshotList();
        }

        // UI�� ��ũ���� ������ ����
        if (screenshotList != null && screenshotList.Count > 0)
        {
            Debug.Log("��ũ���� ������ �ε� �Ϸ�!");
            foreach (string screenshot in screenshotList)
            {
                Debug.Log("��ũ���� ���: " + screenshot);
            }
        }
        else
        {
            Debug.LogWarning("��ũ���� �����Ͱ� �����ϴ�.");
        }

        // ��ư �̺�Ʈ ���
        btn_Yes.onClick.AddListener(OnClickYesBtn);
        btn_No.onClick.AddListener(OnClickNoBtn);
        btn_X.onClick.AddListener(OnClickXBtn);

        panelInventory.SetActive(false);
        panelLinkNews.SetActive(false);
        panelPicket.SetActive(false);
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
                if (hitInfo.collider.gameObject.layer == 18 && currentPicket != null)
                {
                    ShowPicketUI();
                }
            }
        }
    }

    public void ShowPicketUI()
    {
        isShowPicketUI = true;

        // Picket UI Ȱ��ȭ
        panelPicket.SetActive(true);

        // ���� ������Ʈ Ȱ��ȭ
        drawWithMouse.lineParent.SetActive(true);
        // ��ƼĿ ������Ʈ Ȱ��ȭ
        drawWithMouse.stickerParent.SetActive(true);

        // Picket�� ����� ��ũ���� ��η� newsTexture ����
        LoadNewsTextureFromPicket();
    }

    public void LoadNewsTextureFromPicket()
    {
        if (currentPicket != null)
        {
            // PicketId_KJS ������Ʈ���� ��ũ���� ��� ��������
            PicketId_KJS picketIdKJS = currentPicket.GetComponent<PicketId_KJS>();
            if (picketIdKJS != null)
            {
                string screenshotPath = picketIdKJS.GetScreenshotPath();

                if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
                {
                    byte[] fileData = File.ReadAllBytes(screenshotPath);
                    Texture2D texture = new Texture2D(2, 2);
                    if (texture.LoadImage(fileData))
                    {
                        img_News.texture = texture;
                        Debug.Log("��ũ������ ���������� �ε�Ǿ����ϴ�: " + screenshotPath);
                    }
                    else
                    {
                        Debug.LogError("�ؽ�ó �ε� ����: " + screenshotPath);
                    }
                }
                else
                {
                    Debug.LogError("��ũ���� ��ΰ� ��ȿ���� �ʰų� ������ �������� �ʽ��ϴ�: " + screenshotPath);
                }
            }
            else
            {
                Debug.LogError("currentPicket�� PicketId_KJS ������Ʈ�� �����ϴ�.");
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

    public void SetURL(int index)
    {
        currentPicket.GetComponent<PicketId_KJS>().SetScreenshotPath(screenshotList[index]);
    }
}