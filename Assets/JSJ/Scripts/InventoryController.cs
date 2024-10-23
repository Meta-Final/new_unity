using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    

    public Button btn_ScreenshotInven;
    public Button btn_TextInven;

    public GameObject screenshotBG;
    public GameObject textBG;

    void Start()
    {
        btn_ScreenshotInven.onClick.AddListener(() => OnClickScreenshotInven());
        btn_TextInven.onClick.AddListener(() => OnClickTextInven());
        
    }

    void Update()
    {
        // 인벤토리 UI Toggle
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    public void OnClickScreenshotInven()
    {
        screenshotBG.SetActive(true);
        textBG.SetActive(false);
    }

    public void OnClickTextInven()
    {
        screenshotBG.SetActive(false);
        textBG.SetActive(true);
    }


}
