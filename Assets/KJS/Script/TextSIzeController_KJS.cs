using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextSIzeController_KJS : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform targetRect; // 조절할 Input Field의 RectTransform
    private Vector2 originalSize;
    private Vector2 originalMousePosition;
    private bool isResizing = false;

    void Start()
    {
        if (targetRect == null)
        {
            targetRect = GetComponent<RectTransform>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 우클릭으로 시작
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            isResizing = true;
            originalSize = targetRect.sizeDelta;
            originalMousePosition = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isResizing)
        {
            Vector2 delta = eventData.position - originalMousePosition;
            Vector2 newSize = new Vector2(originalSize.x + delta.x, originalSize.y + delta.y);

            // 최소 크기 제한 (원하는 값으로 조정 가능)
            newSize.x = Mathf.Max(newSize.x, 100);
            newSize.y = Mathf.Max(newSize.y, 30);

            targetRect.sizeDelta = newSize;
        }
    }

    private void Update()
    {
        // 마우스 우클릭을 떼면 조절 중지
        if (Input.GetMouseButtonUp(1))
        {
            isResizing = false;
        }
    }
}