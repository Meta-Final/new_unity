using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawWithMouse : MonoBehaviour
{
    public RawImage rawImage;          // 그림을 그릴 RawImage
    public Color brushColor = Color.red; // 브러시 색상
    public int brushSize = 5;           // 브러시 크기

    private Texture2D texture;
    private RectTransform rawImageRect;

    private void Start()
    {
        rawImageRect = rawImage.GetComponent<RectTransform>();

        // Texture2D 초기화 및 RawImage에 설정
        texture = new Texture2D((int)rawImageRect.rect.width, (int)rawImageRect.rect.height);
        rawImage.texture = texture;
        ClearTexture();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 localPoint;

            // 마우스 위치를 RawImage 좌표로 변환
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rawImageRect, Input.mousePosition, null, out localPoint);

            // 텍스처 좌표로 변환
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

                    // 텍스처 경계 내에 있을 경우 픽셀 색상 변경
                    if (pixelX >= 0 && pixelX < texture.width && pixelY >= 0 && pixelY < texture.height)
                    {
                        texture.SetPixel(pixelX, pixelY, brushColor);
                    }
                }
            }
        }
        texture.Apply(); // 텍스처 업데이트
    }

    private void ClearTexture()
    {
        // 텍스처 초기화
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
