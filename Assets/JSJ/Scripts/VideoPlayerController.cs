using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        string videoPath = Path.Combine(Application.streamingAssetsPath, "Loginvideo.mp4");
        videoPlayer.url = "file:///" + videoPath;

        videoPlayer.Play();

        Debug.Log("Video Path: " + videoPlayer.url);
    }
}

