using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PicketSceneManager : MonoBehaviour
{
    public Button btn_GoToTown;

    void Start()
    {
        btn_GoToTown.onClick.AddListener(GoToTown);
    }

    void Update()
    {
        
    }

    public void GoToTown()
    {
        SceneManager.LoadScene("Meta_Town_Scene");
    }
}
