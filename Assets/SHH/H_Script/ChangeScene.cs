using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string MyRoomScene;
    public string StartScene;
    public void SceneChange()
    {
        SceneManager.LoadScene("MyRoomScene");
        Debug.Log("씬전환성공");
    }
    public void ChannelSceneChange()
    {
        SceneManager.LoadScene("StartScene");
        Debug.Log("씬전환성공");
    }

}
