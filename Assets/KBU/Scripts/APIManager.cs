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
    public void Auth()
    {
        AuthRequest auth = new AuthRequest 
        {
            userId = firestore.GetUserInfo().userId,
            name = firestore.GetUserInfo().name, 
            nickName = firestore.GetUserInfo().nickName
        };

        string authUrl = authURL.authURL;
        StartCoroutine(PostHttp<AuthRequest>(auth, authUrl));
    }

    //기사 호출 관련 함수
    public void SearchArticle(string query, int limit)
    {   
        SearchRequest searchRequest = new SearchRequest{query = query, limit = limit};
        string searchUrl = articleURL.searchURL;
        StartCoroutine(PostHttp<SearchRequest, SearchResponse>(searchRequest, searchUrl));
    }
    
    public void CreateArticle()
    {   
        AuthRequest auth = new AuthRequest 
        {
            userId = firestore.GetUserInfo().userId,
            name = firestore.GetUserInfo().name, 
            nickName = firestore.GetUserInfo().nickName
        };
        
        Article article = new Article{authRequest = auth};

        string createUrl = articleURL.createURL;
        StartCoroutine(PostHttp<Article, Article>(article, createUrl));
    }

    public void GetArticle()
    {   
        AuthRequest auth = new AuthRequest 
        {
            userId = firestore.GetUserInfo().userId,
            name = firestore.GetUserInfo().name, 
            nickName = firestore.GetUserInfo().nickName
        };
        
        Article article = new Article{authRequest = auth};
        string getUrl = articleURL.getURL;
        StartCoroutine(PostHttp<Article, Article>(article, getUrl));
    }

    //LLM 호출 메서드
    public void LLM(string text)
    {
        string userId = firestore.GetUserInfo().userId;
        string chatUrl = aiUrl.chatURL;

        ChatRequest chatRequest = new ChatRequest{userId = userId, text = text};
        StartCoroutine(PostHttp<ChatRequest, ChatResponse>(chatRequest, chatUrl));
    }

    //Cover 생성/호출 메서드
    public void LoadCover(string imgPath, string fileSavePath)
    {
        Files files = new Files
        {
            imgPath = imgPath
        };
        LoadCoverRequest loadCoverRequest = new LoadCoverRequest {filePath = new List<Files> { files }};
        string loadCoverUrl = aiUrl.loadCoverURL;
        StartCoroutine(PostHttpFile<LoadCoverRequest>(loadCoverRequest, loadCoverUrl, fileSavePath));
    }
    
    //Object 생성/호출 메서드
    public void Loadobject(string objPath, string pngPath, string fileSavePath)
    {   
        Files files = new Files
        {
            imgPath = pngPath,
            objPath = objPath 
        };
        LoadObjectRequest loadObjectRequest = new LoadObjectRequest {filePath = new List<Files> { files }};
        string loadObjectUrl = aiUrl.loadObjectURL;
        StartCoroutine(PostHttpFile<LoadObjectRequest>(loadObjectRequest, loadObjectUrl, fileSavePath));
    }

    //Trend 생성/호출 메서드 
    public void Trend(string fileSavePath)
    {
        string trendUrl = aiUrl.trendURL;
        StartCoroutine(PostHttpFile(trendUrl, fileSavePath));
    }

    //Http Json Post
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

    public IEnumerator PostHttp<TRequest>(TRequest requestObject, string url)
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
                Debug.Log("Request Success");
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}, Response: {www.downloadHandler.text}" );
            }

        }
    }

    //Http File Post
    public IEnumerator PostHttpFile<TRequest>(TRequest requestObject, string url, string fileSavePath)
    {
        string json = JsonUtility.ToJson(requestObject);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerFile(fileSavePath);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"File downloaded successfully and saved at: {fileSavePath}");
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}");
            }
        }
    }

    public IEnumerator PostHttpFile(string url, string fileSavePath)
    {
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.downloadHandler = new DownloadHandlerFile(fileSavePath);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"File downloaded successfully and saved at: {fileSavePath}");
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}");
            }
        }
    }


    //진짜 망했을때 용
    public Texture2D coverDownloadTexture;
    public Texture2D trendDownloadTexture;


    AlphaURL alphaURL = new AlphaURL();
    //알파 시연 메서드
    //첫 번째 썸네일 이미지 호출 코드
    //두 번째 트렌드 이미지 호출 코드
    public void Cover()
    {   
        string coverURL = alphaURL.coverURL;
        StartCoroutine(CoverDownloadImage(coverURL));
    }

    public void Trend()
    {
        string trendURL = alphaURL.trendURL;
        StartCoroutine(TrendDownloadImage(trendURL));
    }

    IEnumerator CoverDownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            coverDownloadTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        else
        {
            Debug.LogError("Failed to download image: " + request.error);
        }
    }

    
    IEnumerator TrendDownloadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            trendDownloadTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
        else
        {
            Debug.LogError("Failed to download image: " + request.error);
        }
    }


    public Texture2D CoverGetDownloadedImage()
    {
        return coverDownloadTexture;
    }
    
     public Texture2D TrendGetDownloadedImage()
    {
        return trendDownloadTexture;
    }


}
