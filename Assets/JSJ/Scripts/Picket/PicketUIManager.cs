using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PicketUIManager : MonoBehaviour
{
    public GameObject panelPicket;
    public Button btn_X;
    

    void Start()
    {
        btn_X.onClick.AddListener(OnClickXButton);
        
    }

    void Update()
    {
        
    }

    void OnClickXButton()
    {
        panelPicket.SetActive(false);
    }
}
