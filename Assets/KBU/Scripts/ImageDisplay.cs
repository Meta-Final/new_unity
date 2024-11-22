using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageDisplay : MonoBehaviour
{
    public APIManager imageDownloader; 
    public RawImage rawImage;

    void Start()
    {
        StartCoroutine(CoverWaitForDownloadAndDisplayImage());
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
}