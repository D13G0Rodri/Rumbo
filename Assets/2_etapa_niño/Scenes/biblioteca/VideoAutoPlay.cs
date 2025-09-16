using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoAutoPlay : MonoBehaviour
{
    VideoPlayer vp;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        vp.errorReceived += (source, msg) => Debug.LogError("Video error: " + msg);
        vp.prepareCompleted += (source) => { vp.Play(); };
        vp.Prepare();
    }
}
