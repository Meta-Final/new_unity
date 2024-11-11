using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class KeywordUI : MonoBehaviour
{
    public RawImage trendmap;
    void Start()
    {
        
    }

    void Update()
    {
       
    }

    void ViewTrend()
    {
        StartCoroutine(TrendGet());
    }

    public IEnumerator TrendGet()
    {
        string jsonpngPath = "file:///C:/";

        UnityWebRequest request = UnityWebRequest.Get(jsonpngPath);
        //서버 요청 보내기
        yield return request.SendWebRequest();

        //서버 결과 정상이라면
        if(request.result == UnityWebRequest.Result.Success)
        {
            //데이터처리
            File.WriteAllBytes(Application.dataPath + "", request.downloadHandler.data);

            Texture2D trendtexture = DownloadHandlerTexture.GetContent(request);
            trendmap.texture = trendtexture;
        }
        //그렇지않으면 오류출력
        else
        {
            Debug.LogError("미안 안왔대!" + request.error);
        }
    }
}
