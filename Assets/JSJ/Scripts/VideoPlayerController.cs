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
       

        // VideoPlayer에 URL 설정
        videoPlayer.url = videoPath;

        // 비디오 재생
        videoPlayer.Play();

        // 디버그 로그로 경로 출력
        Debug.Log("Video Path: " + videoPath);
    }
}
