using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBarController : MonoBehaviour
{
    public GameObject btn_Channel;

    public float moveSpeed = 5f;

    public Vector3 channelOriginPos;
    public Vector3 offset = new Vector3(100, 0, 0);
    
    
    void Start()
    {
        channelOriginPos = btn_Channel.transform.position;
        print(channelOriginPos);
        
    }

    public void MoveButton()
    {
        Vector3 targetPos = channelOriginPos + offset;
        btn_Channel.transform.position = Vector3.Lerp(channelOriginPos, targetPos, moveSpeed);
        btn_Channel.transform.position = targetPos;


    }

    
    

    

   
    






}
