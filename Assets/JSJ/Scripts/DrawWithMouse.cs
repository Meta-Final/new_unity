using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawWithMouse : MonoBehaviour
{
    public RawImage rawImage;          // �׸��� �׸� RawImage
    public Color brushColor = Color.red; // �귯�� ����
    public int brushSize = 5;           // �귯�� ũ��

    private Texture2D texture;
    private RectTransform rawImageRect;

    private void Start()
    {
        rawImageRect = rawImage.GetComponent<RectTransform>();

        // Texture2D �ʱ�ȭ �� RawImage�� ����
        texture = new Texture2D((int)rawImageRect.rect.width, (int)rawImageRect.rect.height);
        rawImage.texture = texture;
        ClearTexture();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 localPoint;

            // ���콺 ��ġ�� RawImage ��ǥ�� ��ȯ
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImageRect, Input.mousePosition, null, out localPoint);

            // �ؽ�ó ��ǥ�� ��ȯ
            int x = (int)(localPoint.x + rawImageRect.rect.width * 0.5f);
            int y = (int)(localPoint.y + rawImageRect.rect.height * 0.5f);

            DrawCircle(x, y);
        }
    }

    private void DrawCircle(int centerX, int centerY)
    {
        for (int y = -brushSize; y <= brushSize; y++)
        {
            for (int x = -brushSize; x <= brushSize; x++)
            {
                if (x * x + y * y <= brushSize * brushSize)
                {
                    int pixelX = centerX + x;
                    int pixelY = centerY + y;

                    // �ؽ�ó ��� ���� ���� ��� �ȼ� ���� ����
                    if (pixelX >= 0 && pixelX < texture.width && pixelY >= 0 && pixelY < texture.height)
                    {
                        texture.SetPixel(pixelX, pixelY, brushColor);
                    }
                }
            }
        }
        texture.Apply(); // �ؽ�ó ������Ʈ
    }

    private void ClearTexture()
    {
        // �ؽ�ó �ʱ�ȭ
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, Color.white);
            }
        }
        texture.Apply();
    }
}
