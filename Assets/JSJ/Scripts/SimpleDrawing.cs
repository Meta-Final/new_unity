using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SimpleDrawing : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    public RawImage rawImage; // ������� RawImage
    private Texture2D texture;

    private void Start()
    {
        // RawImage�� �ؽ�ó ����
        texture = new Texture2D(512, 512);
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, Color.clear); // ���� ������� �ʱ�ȭ
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
        // Ŭ���� ��ġ�� ���
        RectTransform rectTransform = rawImage.rectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // �̹��� ��ǥ�� ��ȯ
        int x = Mathf.RoundToInt((localPoint.x / rectTransform.sizeDelta.x) * texture.width);
        int y = Mathf.RoundToInt((localPoint.y / rectTransform.sizeDelta.y) * texture.height);

        // ��ȿ�� ���� ������ ���� ����
        if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
        {
            texture.SetPixel(x, y, Color.black); // ���ϴ� �������� �ȼ� ����
            texture.Apply();
        }
    }
}
