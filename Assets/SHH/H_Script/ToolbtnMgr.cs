using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolbtnMgr : MonoBehaviour
{
    public static ToolbtnMgr instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
   
    }

    public void Exitpanel()
    {
        

    }
}
