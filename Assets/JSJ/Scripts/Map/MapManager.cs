using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public Button btn_F1;

    void Start()
    {
        btn_F1.onClick.AddListener(GoToF1);
        
    }

    void Update()
    {
        
    }

    void GoToF1()
    {
        SceneManager.LoadScene("Meta_Town_Scene");
    }
}
