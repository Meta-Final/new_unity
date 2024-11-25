using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarController : MonoBehaviour
{
    public RectTransform btn_Channel;

    public float moveSpeed = 5f;

    public Vector3 channelOriginPos;
    public Vector3 offset = new Vector3(50, 0, 0);
    
    
    void Start()
    {
        channelOriginPos = btn_Channel.position;
        print(channelOriginPos);
        
    }

    public void MoveButton()
    {
        Vector3 targetPos = channelOriginPos + offset;
        btn_Channel.position = Vector3.Lerp(channelOriginPos, targetPos, moveSpeed);
        btn_Channel.transform.position = targetPos;


    }

    
    

    

   
    






}
