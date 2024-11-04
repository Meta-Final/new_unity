using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;   // 인벤토리 창
    
    public Button btn_ScreenshotInven;  // 스크린샷 인벤토리 버튼
    public Button btn_TextInven;        // 기사 인벤토리 버튼

    public GameObject screenshotBG;     // 스크린샷 인벤토리 화면
    public GameObject textBG;           // 기사 인벤토리 화면

    void Start()
    {
        // 버튼 기능 추가
        btn_ScreenshotInven.onClick.AddListener(() => OnClickScreenshotInven());
        btn_TextInven.onClick.AddListener(() => OnClickTextInven());
    }

    void Update()
    {
        // 인벤토리 UI Toggle (키보드 I 누를때마다 인벤토리 UI 활성화/비활성화)
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    // 스크린샷 인벤토리 버튼 기능
    public void OnClickScreenshotInven()
    {
        // 스크린샷 인벤토리 화면 활성화
        screenshotBG.SetActive(true);
        // 기사 인벤토리 화면 비활성화
        textBG.SetActive(false);
    }

    // 기사 인벤토리 버튼 기능
    public void OnClickTextInven()
    {
        // 스크린샷 인벤토리 화면 비활성화
        screenshotBG.SetActive(false);
        // 기사 인벤토리 화면 활성화
        textBG.SetActive(true);
    }
}
