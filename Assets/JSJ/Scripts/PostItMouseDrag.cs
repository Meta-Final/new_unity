using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostItMouseDrag : MonoBehaviour
{
    
    public bool isDragging = false;

    Camera cam;
    Vector3 offset;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        isDragging = true;
        
    }

    public void OnMouseUp()
    {
        isDragging = false;
    }
}
