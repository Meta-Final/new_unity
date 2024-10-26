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
                Debug.LogWarning("ä�� ĵ������ ã�� �� �����ϴ�. �̸��� Ȯ���ϼ���.");
            }
        }
        
    }
}
