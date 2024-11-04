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
    public GameObject btn_Pos;

    private LoadMgr_KJS loadManager; // LoadMgr_KJS 인스턴스를 참조하기 위한 변수 추가

    List<Button> btns = new List<Button>();
    public Button btn_Exit;

    // Start is called before the first frame update
    void Start()
    {
        // EditorManager 오브젝트에서 LoadMgr_KJS 컴포넌트 가져오기
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
        info.url = "C:\\Users\\Admin\\Desktop\\post\\postinfolist.txt";
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
            btns[i].onClick.AddListener(OnClickMagContent); // OnClickMagContent 이벤트 추가

            post.SetInfo(allPost[i]);
        }

        if (MagCanvas)
        {
            MagCanvas.SetActive(false);
        }
    }

    public void OnClickMagContent()
    {
        MagCanvas.SetActive(true);

        // loadManager가 존재하는 경우 LoadObjectsFromFile 호출
        if (loadManager != null)
        {
            loadManager.LoadObjectsFromFile();
        }
        else
        {
            Debug.LogError("loadManager가 null입니다. LoadMgr_KJS를 찾을 수 없습니다.");
        }
    }

    public void OncompletePostDetalInfo(DownloadHandler downloadhandler)
    {
        print(downloadhandler.text);
    }

    public void OnClickExit()
    {
        MagCanvas.SetActive(false);
    }
}
