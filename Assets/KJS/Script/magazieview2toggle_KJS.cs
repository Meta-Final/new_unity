using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class magazieview2toggle_KJS : MonoBehaviour
{
    [SerializeField] private GameObject targetUI; // ����� UI ������Ʈ
    private bool isUIToggled = false;

    public GameObject roombtn;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            isUIToggled = !isUIToggled;
            targetUI.SetActive(isUIToggled);
            roombtn.SetActive(false);
        }
        
        
    }
}
