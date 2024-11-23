using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Loginscenevideo.mp4");
       

        // VideoPlayer�� URL ����
        videoPlayer.url = videoPath;

        // ���� ���
        videoPlayer.Play();

        // ����� �α׷� ��� ���
        Debug.Log("Video Path: " + videoPath);
    }
}
