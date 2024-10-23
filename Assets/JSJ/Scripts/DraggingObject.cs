using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggingObject : MonoBehaviour
{
    bool isDragging = false;

    // 포스트잇의 초기위치
    Vector3 offset;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;

        offset = transform.position - Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector3 newPos = Camera.main.ScreenToWorldPoint(eventData.position) + offset;
            newPos.z = 0;
            transform.position = newPos;
        }
    }
}
