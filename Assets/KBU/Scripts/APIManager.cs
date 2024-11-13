using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using ReqRes;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class APIManager : MonoBehaviour
{   
    public Texture2D coverDownloadTexture;
    public Texture2D trendDownloadTexture;
    public Image trendImage;


    AlphaURL alphaURL = new AlphaURL();
    //알파 시연 메서드
    //첫 번째 썸네일 이미지 호출 코드
    //두 번째 트렌드 이미지 호출 코드

    private void Start()
    {
        Trend();
    }
    public void Cover()
    {   
        string coverURL = alphaURL.coverURL;
        StartCoroutine(CoverDownloadImage(coverURL));
    }

    public void Trend()
    {
        string trendURL = alphaURL.trendURL;
        //string trendURL = "https://cdn.weaversmind.com/landing/202409/25/on_land/Paper-122-01-01/cnt_01.jpg";
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
            Sprite sprite = Sprite.Create(trendDownloadTexture, new Rect(0, 0, trendDownloadTexture.width, trendDownloadTexture.height), Vector2.zero);

            trendImage.sprite = sprite;
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
