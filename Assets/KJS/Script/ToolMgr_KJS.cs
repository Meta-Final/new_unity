using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ToolMgr_KJS : MonoBehaviour
{
    public GameObject editorPanel;  // 활성화/비활성화할 패널

    // 패널의 활성화 상태를 토글하는 함수
    public void OnClickTogglePanel()
    {
        bool isActive = editorPanel.activeSelf;
        editorPanel.SetActive(!isActive);
    }
}
