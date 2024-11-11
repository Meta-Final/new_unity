using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLineEffect : MonoBehaviour
{
    public Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();

        outline.enabled = false;
    }

    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        outline.enabled = true;
    }

    public void OnMouseExit()
    {
        outline.enabled = false;
        
    }
}
