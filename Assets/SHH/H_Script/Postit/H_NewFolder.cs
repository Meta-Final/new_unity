using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class H_NewFolder : MonoBehaviour
{
    public List<Texture> texs = new List<Texture>();
    public GameObject contentsView;
    public GameObject newFolder;
    public Button Btn_close_mergeview;

    void Start()
    {
       
        Btn_close_mergeview.onClick.AddListener(OnClickCloseContentview);

    }

    void Update()
    {
        
    }
    private void OnClickContentView()
    {
        contentsView.SetActive(true);
    }
    private void OnClickCloseContentview()
    {
        contentsView.SetActive(false);
    }
}

