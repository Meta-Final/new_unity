using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextSIzeController_KJS : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public RectTransform targetRect; // ������ Input Field�� RectTransform
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
        // ��Ŭ������ ����
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

            // �ּ� ũ�� ���� (���ϴ� ������ ���� ����)
            newSize.x = Mathf.Max(newSize.x, 100);
            newSize.y = Mathf.Max(newSize.y, 30);

            targetRect.sizeDelta = newSize;
        }
    }

    private void Update()
    {
        // ���콺 ��Ŭ���� ���� ���� ����
        if (Input.GetMouseButtonUp(1))
        {
            isResizing = false;
        }
    }
}