using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class PostInfo
{
    public string editorname;
    public string thumburl;
}
[Serializable]
public class PostInfoList
{
    public List<PostInfo> postData;
}

public class PostMgr : MonoBehaviour
{
    public List<PostInfo> allPost = new List<PostInfo>();
    public GameObject prefabfactory;
    public GameObject content;

    // Start is called before the first frame update
    void Start()
    {
        HttpInfo info = new HttpInfo();
        info.url = "file:///C:\\Users\\haqqm\\Desktop\\post\\postinfolist.txt";
        info.onComplete = OncompletePostInfo;

        StartCoroutine(HttpManager.GetInstance().Get(info));
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
            post.SetInfo(allPost[i]);
        }

        //GameObject.Find("CanvasMag") ;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
