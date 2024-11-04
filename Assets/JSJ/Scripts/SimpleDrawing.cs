using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleDrawing : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RawImage rawImage; // 드로잉할 RawImage
    private Texture2D texture;

    private void Start()
    {
        // RawImage의 텍스처 생성
        texture = new Texture2D(512, 512);
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, Color.clear); // 투명 배경으로 초기화
            }
        }
        texture.Apply();
        rawImage.texture = texture;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Draw(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Draw(eventData);
    }

    private void Draw(PointerEventData eventData)
    {
        // 클릭한 위치를 계산
        RectTransform rectTransform = rawImage.rectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // 이미지 좌표로 변환
        int x = Mathf.RoundToInt((localPoint.x / rectTransform.sizeDelta.x) * texture.width);
        int y = Mathf.RoundToInt((localPoint.y / rectTransform.sizeDelta.y) * texture.height);

        // 유효한 범위 내에서 색상 설정
        if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
        {
            texture.SetPixel(x, y, Color.black); // 원하는 색상으로 픽셀 설정
            texture.Apply();
        }
    }
}
