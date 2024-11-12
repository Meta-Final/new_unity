using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class H_PostInfo
{
    public string postid;
    public string thumburl;
}

[System.Serializable]
public class PostInfoList
{
    public List<H_PostInfo> postData = new List<H_PostInfo>();  // 리스트를 빈 상태로 초기화
}

public class PostMgr : MonoBehaviour
{
    public List<H_PostInfo> allPost = new List<H_PostInfo>();
    public GameObject prefabfactory;
    public GameObject content;
    public GameObject MagCanvas;
    public GameObject btn_Pos;

    private LoadMgr_KJS loadManager;

    List<Button> btns = new List<Button>();
    public Button btn_Exit;

    void Start()
    {
        return;
        GameObject editorManagerObj = GameObject.Find("EditorManager");
        if (editorManagerObj != null)
        {
            loadManager = editorManagerObj.GetComponent<LoadMgr_KJS>();
            if (loadManager == null)
            {
                Debug.LogError("EditorManager 오브젝트에 LoadMgr_KJS 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("EditorManager 오브젝트를 찾을 수 없습니다.");
        }

        ThumStart();
    }

    public void ThumStart()
    {
        HttpInfo info = new HttpInfo();
        info.url = "C:/Users/Admin/Documents/GitHub/new_unity/Assets/KJS/UserInfo/thumbnail.json";
        info.onComplete = OncompletePostInfo;

        StartCoroutine(HttpManager.GetInstance().Get(info));

        btn_Exit.onClick.AddListener(OnClickExit);
    }

    public void OncompletePostInfo(DownloadHandler downloadhandler)
    {
        PostInfoList postinfoList = JsonUtility.FromJson<PostInfoList>(downloadhandler.text);
        allPost = postinfoList.postData;

        for (int i = 0; i < allPost.Count; i++)
        {
            GameObject go = Instantiate(prefabfactory, content.transform);
            PostThumb post = go.GetComponent<PostThumb>();
            Button bu = go.GetComponent<Button>();
            btns.Add(bu);

            string postId = allPost[i].postid;
            btns[i].onClick.AddListener(() => OnClickMagContent(postId)); // 클릭 시 postId 전달

            post.SetInfo(allPost[i]);
        }

        if (MagCanvas)
        {
            MagCanvas.SetActive(false);
        }
    }

    public void OnClickMagContent(string postId)
    {
        MagCanvas.SetActive(true);

        if (loadManager != null)
        {
            loadManager.LoadObjectsFromFile(postId); // 특정 postId 전달
        }
        else
        {
            Debug.LogError("loadManager가 null입니다. LoadMgr_KJS를 찾을 수 없습니다.");
        }
    }

    public void OnClickExit()
    {
        MagCanvas.SetActive(false);
    }
}