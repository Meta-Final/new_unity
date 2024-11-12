using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class magazieview2toggle_KJS : MonoBehaviour
{
    [SerializeField] private GameObject targetUI; // 토글할 UI 오브젝트
    private bool isUIToggled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isUIToggled = !isUIToggled;
            targetUI.SetActive(isUIToggled);
        }
    }
}
