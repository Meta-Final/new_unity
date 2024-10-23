using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using ReqRes;
using UnityEditor.PackageManager.Requests;

public class APIManager : MonoBehaviour
{
    URL url = new URL();
  
    public void CallLLM(String chat)
    {   
        ChatRequest chatRequest = new ChatRequest { text = chat };
        string chatUrl = url.chatUrl;
        StartCoroutine(Request<ChatRequest, ChatResponse>(chatRequest, chatUrl));
    }

    public IEnumerator Request<TRequest, TResponse>(TRequest requestObject, string url)
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
}
