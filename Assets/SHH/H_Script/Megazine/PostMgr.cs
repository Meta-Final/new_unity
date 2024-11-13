using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

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

    private string baseDirectory = Application.dataPath; // 기본 저장 경로

    void Start()
    {
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
        // postId별로 저장된 폴더에서 thumbnail.json 파일을 찾습니다.
        if (Directory.Exists(baseDirectory))
        {
            string[] postDirectories = Directory.GetDirectories(baseDirectory);

            foreach (string postDirectory in postDirectories)
            {
                string jsonFilePath = Path.Combine(postDirectory, "thumbnail.json");

                if (File.Exists(jsonFilePath))
                {
                    try
                    {
                        string json = File.ReadAllText(jsonFilePath);
                        PostInfoList postInfoList = JsonUtility.FromJson<PostInfoList>(json);
                        allPost.AddRange(postInfoList.postData);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"JSON 파일을 로드하는 동안 오류가 발생했습니다: {e.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"폴더 {postDirectory}에 thumbnail.json 파일이 없습니다.");
                }
            }
        }
        else
        {
            Debug.LogError($"기본 디렉토리가 존재하지 않습니다: {baseDirectory}");
        }

        // 모든 포스트 정보를 로드한 후 UI 생성
        CreatePostThumbnails();
        //btn_Exit.onClick.AddListener(OnClickExit);
    }

    private void CreatePostThumbnails()
    {
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