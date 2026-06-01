using UnityEngine;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public ButtonManager buttonManager;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        buttonManager.LoadGameScene();
    }
}