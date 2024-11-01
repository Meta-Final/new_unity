using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class FolderPreview : MonoBehaviour
{
    public GameObject previewImage; // 미리보기 이미지 UI 요소
    public Vector3 offset = new Vector3(100, 50, 0); // 커서 위치에서 떨어진 미리보기 위치

    private void Start()
    {
        // 미리보기 이미지를 처음에는 숨깁니다.
        previewImage.SetActive(false);
    }

    // 마우스를 버튼 위로 올렸을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        previewImage.SetActive(true); // 미리보기 이미지 활성화
        UpdatePreviewPosition();
    }

    // 마우스가 버튼에서 벗어났을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        previewImage.SetActive(false); // 미리보기 이미지 비활성화
    }

    // 마우스 위치에 따라 미리보기 이미지 위치 업데이트
    private void Update()
    {
        if (previewImage.activeSelf)
        {
            UpdatePreviewPosition();
        }
    }

    // 미리보기 위치 설정 함수
    private void UpdatePreviewPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        previewImage.transform.position = mousePos + offset;
    }



}
