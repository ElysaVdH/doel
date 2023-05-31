using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoControllerScrip : MonoBehaviour
{
    public string url;
    VideoPlayer player;

    void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, url);
    }
}
