using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ScenechangeMgr : MonoBehaviour
{
    public string ChannelCanvas;

    void Start()
    {
        if (!string.IsNullOrEmpty(ChannelCanvas))
        {
            GameObject ChangeScene = GameObject.Find(ChannelCanvas);

            if (ChangeScene != null)
            {
                ChangeScene.SetActive(false);
            }
            else
            {
                Debug.LogWarning("채널 캔버스를 찾을 수 없습니다. 이름을 확인하세요.");
            }
        }
        
    }
}
