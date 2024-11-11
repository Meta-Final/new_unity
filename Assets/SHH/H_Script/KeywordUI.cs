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
        //���� ��û ������
        yield return request.SendWebRequest();

        //���� ��� �����̶��
        if(request.result == UnityWebRequest.Result.Success)
        {
            //������ó��
            File.WriteAllBytes(Application.dataPath + "", request.downloadHandler.data);

            Texture2D trendtexture = DownloadHandlerTexture.GetContent(request);
            trendmap.texture = trendtexture;
        }
        //�׷��������� �������
        else
        {
            Debug.LogError("�̾� �ȿԴ�!" + request.error);
        }
    }
}
