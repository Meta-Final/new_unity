using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class H_PostInfo
{
    public string editorname;
    public string thumburl;
}
[System.Serializable]
public class PostInfoList
{
    public List<H_PostInfo> postData;
}

public class PostMgr : MonoBehaviour
{
    public List<H_PostInfo> allPost = new List<H_PostInfo>();
    public GameObject prefabfactory;
    public GameObject content;

    public GameObject MagCanvas;
    public GameObject Channelcanvas;

    List<Button> btns = new List<Button>();

    public Button btn_Exit;

    // Start is called before the first frame update
    void Start()
    {
        MagCanvas = GameObject.Find("CanvasMag");
        Channelcanvas = GameObject.Find("ChannelCanvas");

        HttpInfo info = new HttpInfo();
        info.url = "file:///C:\\Users\\Admin\\Desktop\\post\\postinfolist.txt";
        info.onComplete = OncompletePostInfo;

        StartCoroutine(HttpManager.GetInstance().Get(info));

        btn_Exit.onClick.AddListener(OnClickExit);

    }

    public void OncompletePostInfo(DownloadHandler downloadhandler)
    {
        print(downloadhandler.text);
        PostInfoList postinfoList = JsonUtility.FromJson<PostInfoList>(downloadhandler.text);
        allPost = postinfoList.postData;

        for (int i = 0; i < allPost.Count; i++)
        {
            GameObject go = Instantiate(prefabfactory, content.transform);
            PostThumb post = go.GetComponent<PostThumb>();
            Button bu = go.GetComponent<Button>();
            btns.Add(bu);
            btns[i].onClick.AddListener(OnClickMagContent);

            post.SetInfo(allPost[i]);
        }
       
        print(MagCanvas);
        MagCanvas.SetActive(false);

       
    }
    public void OnClickMagContent()
    {
        //MagCanvas.SetActive(true);
        //Channelcanvas.SetActive(false);
        HttpInfo info = new HttpInfo();
        info.url = "file:///" + Application.dataPath + "/KJS/UserInfo/Magazine.json";
        info.onComplete = OncompletePostDetalInfo;

        StartCoroutine(HttpManager.GetInstance().Get(info));
    }

    public void OncompletePostDetalInfo(DownloadHandler downloadhandler)
    {
        SerializableDictionary serializableDictionary = JsonUtility.FromJson<SerializableDictionary>(downloadhandler.text);
        print(downloadhandler.text);
    }


    public void OnClickExit()
    {
        MagCanvas.SetActive(false);
        Channelcanvas.SetActive(true);
        
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
