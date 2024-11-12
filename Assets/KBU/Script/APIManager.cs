using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using ReqRes;

public class APIManager : MonoBehaviour
{
    FireStore firestore;

    AuthURL authURL = new AuthURL();
    ArticleURL articleURL = new ArticleURL();
    AIURL aiUrl = new AIURL();
    
    public void Start()
    {
        GameObject firebase = GameObject.Find("Firebase");
        firestore = firebase.GetComponent<FireStore>();
    }
    
    //회원가입
    //public void Auth()
    //{
    //    string id = firestore.UserInfo().userid;
    //    AuthRequest auth = new AuthRequest {userid = id};
    //    string authUrl = authURL.authURL;
    //    StartCoroutine(GetHttp<AuthRequest>(auth, authUrl));
    //}

    //기사 호출 관련 함수
    public void SearchArticle(string query, int limit)
    {   
        SearchRequest searchRequest = new SearchRequest{query = query, limit = limit};
        string searchUrl = articleURL.searchURL;
        StartCoroutine(PostHttp<SearchRequest, SearchResponse>(searchRequest, searchUrl));
    }
    
    //public void CreateArticle()
    //{   
    //    string id = firestore.UserInfo().userid;
    //    Article article = new Article{userid = id};
    //    string createUrl = articleURL.createURL;
    //    StartCoroutine(PostHttp<Article, Article>(article, createUrl));
    //}

    //public void GetArticle()
    //{   
    //    string id = firestore.UserInfo().userid;
    //    Article article = new Article{userid = id};
    //    string getUrl = articleURL.getURL;
    //    StartCoroutine(PostHttp<Article, Article>(article, getUrl));
    //}

    //LLM 호출 메서드

    //Cover 생성/호출 메서드
    public void GenCover(string prompt)
    {
        GenCoverRequest genCoverRequest = new GenCoverRequest {text = prompt};
        string genCoverUrl = aiUrl.genCoverURL;
        StartCoroutine(PostHttp<GenCoverRequest, GenCoverResponse>(genCoverRequest, genCoverUrl));
    }
    public void LoadCover(string path)
    {
        LoadCoverRequest loadCoverRequest = new LoadCoverRequest {imgPath = path};
        string loadCoverUrl = aiUrl.loadCoverURL;
        StartCoroutine(PostHttp<LoadCoverRequest, LoadCoverResponse>(loadCoverRequest, loadCoverUrl)); 
    }
    
    //Object 생성/호출 메서드
    public void GenObject(string prompt)
    {
        GenObjectRequest genObjectRequest = new GenObjectRequest {text = prompt};
        string genObjectUrl = aiUrl.genObjectURL;
        StartCoroutine(PostHttp<GenObjectRequest, GenObjectResponse>(genObjectRequest, genObjectUrl));
    }
    public void loadobject(string objPath, string mtlPath)
    {
        LoadObjectRequest loadObjectRequest = new LoadObjectRequest {objFilePath = objPath, mtlFilePath = mtlPath};
        string loadObjectUrl = aiUrl.loadObjectURL;
        StartCoroutine(PostHttp<LoadObjectRequest, LoadObjectResponse>(loadObjectRequest, loadObjectUrl));
    }

    //Http 메서드 post
    public IEnumerator PostHttp<TRequest, TResponse>(TRequest requestObject, string url)
    {   
        string json = JsonUtility.ToJson(requestObject);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                TResponse responseObject = JsonUtility.FromJson<TResponse>(www.downloadHandler.text);
                Debug.Log("Response: " + JsonUtility.ToJson(responseObject));
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}, Response: {www.downloadHandler.text}" );
            }

        }
    }

    //Http 메서드 get
    public IEnumerator GetHttp<TRequest>(TRequest requestObject, string url)
    {   
        string json = JsonUtility.ToJson(requestObject);
    
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Request successful. Status Code: " + www.responseCode);
            }
            else
            {   
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}");
            }
        }
    }
}
