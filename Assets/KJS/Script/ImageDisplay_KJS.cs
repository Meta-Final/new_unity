using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay_KJS : MonoBehaviour
{
    public APIManager imageDownloader;
    public Image image;  // RawImage 대신 Image 컴포넌트 사용

    void Start()
    {
        StartCoroutine(CoverWaitForDownloadAndDisplayImage());
        StartCoroutine(TrendWaitForDownloadAndDisplayImage());
    }

    IEnumerator CoverWaitForDownloadAndDisplayImage()
    {
        // 이미지가 다운로드될 때까지 대기
        while (imageDownloader.CoverGetDownloadedImage() == null)
        {
            yield return null;
        }

        // 다운로드된 Texture2D를 Sprite로 변환하여 Image 컴포넌트에 표시
        Texture2D downloadedTexture = imageDownloader.CoverGetDownloadedImage();
        image.sprite = TextureToSprite(downloadedTexture);
        Debug.Log("Cover image displayed on UI.");
    }

    IEnumerator TrendWaitForDownloadAndDisplayImage()
    {
        // 이미지가 다운로드될 때까지 대기
        while (imageDownloader.TrendGetDownloadedImage() == null)
        {
            yield return null;
        }

        // 다운로드된 Texture2D를 Sprite로 변환하여 Image 컴포넌트에 표시
        Texture2D downloadedTexture = imageDownloader.TrendGetDownloadedImage();
        image.sprite = TextureToSprite(downloadedTexture);
        Debug.Log("Trend image displayed on UI.");
    }

    // Texture2D를 Sprite로 변환하는 메서드
    private Sprite TextureToSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}