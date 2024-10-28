using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using ReqRes;
using UnityEditor.PackageManager.Requests;
using System.Net.NetworkInformation;

public class APIManager : MonoBehaviour
{
    URL url = new URL();
    public AiChatMgr_KJS aiChatMgr;


    public void CallLLM(String chat)
    {   
        ChatRequest chatRequest = new ChatRequest { text = chat };
        string chatUrl = url.chatUrl;
        StartCoroutine(PostHttp<ChatRequest, ChatResponse>(chatRequest, chatUrl));
    }

    public void CallTrend(string trendReq)
    {   
        TrendRequest trendRequest = new TrendRequest {text = trendReq};
        string trendUrl = url.trendUrl;
        StartCoroutine(GetHttp<TrendRequest, TrendResponse>(trendRequest, trendUrl));
    }

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
                String responseText = www.downloadHandler.text;
                Debug.Log($"API ����: {responseText}"); // ����� �α� �߰�

                aiChatMgr.UpdateChatResponse(responseText); // AiChatMgr_KJS�� ���� ����
                // JSON ���� �Ľ�
                JsonData jsonResponse = JsonUtility.FromJson<JsonData>(www.downloadHandler.text);
                Debug.Log("Response: " + www.downloadHandler.text);

                if (jsonResponse.text == "/img.json")
                {
                    LoadImageFromPath(@"C:\Users\Admin\Desktop\�丮.jpg");
                }
                else if (jsonResponse.text == "/object.json")
                {
                    // �ٸ� ���� ó��
                }
            }
            else
            {
                Debug.LogError($"Error: {www.error}, Status Code: {www.responseCode}, Response: {www.downloadHandler.text}");
            }
        }
    }

    public IEnumerator GetHttp<TRequest, TResponse>(TRequest requestObject, string url)
    {   
        string json = JsonUtility.ToJson(requestObject);
        
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
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
    private void LoadImageFromPath(string path)
    {
        if (File.Exists(path))
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D tex = new Texture2D(2, 2); // �ӽ÷� 2x2 �ؽ�ó ����
            if (tex.LoadImage(fileData))
            {
                Debug.Log("�̹��� �ε� ����!");

                // �ؽ�ó�� ������ �� �ִ� ���� (��: UI �Ǵ� ������Ʈ�� ��Ƽ���� ����)
                // ����: RawImage �Ǵ� Renderer ���
                // GetComponent<Renderer>().material.mainTexture = tex;
            }
            else
            {
                Debug.LogError("�̹��� �ε� ����!");
            }
        }
        else
        {
            Debug.LogError($"�̹��� ������ ã�� �� �����ϴ�: {path}");
        }
    }

    [Serializable]
    public class JsonData
    {
        public string text;
    }
}
