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
        Debug.Log("����ȯ����");
    }
    public void ChannelSceneChange()
    {
        SceneManager.LoadScene("StartScene");
        Debug.Log("����ȯ����");
    }

}
