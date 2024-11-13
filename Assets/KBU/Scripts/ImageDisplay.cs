using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay : MonoBehaviour
{
    public APIManager imageDownloader; 
    public RawImage rawImage;

    void StartCover()
    {
        StartCoroutine(CoverWaitForDownloadAndDisplayImage());
    }

    void StartTrend()
    {
        StartCoroutine(TrendWaitForDownloadAndDisplayImage());
    }

    IEnumerator CoverWaitForDownloadAndDisplayImage()
    {
       
        while (imageDownloader.CoverGetDownloadedImage() == null)
        {
            yield return null; 
        }

        rawImage.texture = imageDownloader.CoverGetDownloadedImage();
        Debug.Log("Image displayed on UI.");
    }

    IEnumerator TrendWaitForDownloadAndDisplayImage()
    {
       
        while (imageDownloader.TrendGetDownloadedImage() == null)
        {
            yield return null; 
        }

        rawImage.texture = imageDownloader.TrendGetDownloadedImage();
        Debug.Log("Image displayed on UI.");
    }
}