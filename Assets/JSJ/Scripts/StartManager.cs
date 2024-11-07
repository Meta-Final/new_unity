using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public Button btn_Start;

    void Start()
    {
        btn_Start.onClick.AddListener(OnClickStart);
        
    }

    void Update()
    {
        
    }

    void OnClickStart()
    {
        SceneManager.LoadScene("Meta_Login_Scene");

    }
}
