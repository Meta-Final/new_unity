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
        GameObject ChangeScene = GameObject.Find(ChannelCanvas);

        if (ChangeScene != null)
        {
            ChangeScene.SetActive(false);
        }
       else
       {
           Debug.LogWarning("�־ȵɱ�����������");
       }

    }
}
