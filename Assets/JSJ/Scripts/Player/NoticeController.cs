using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeController : MonoBehaviour
{
    bool isNoticeOpen = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isNoticeOpen)
        {

        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
