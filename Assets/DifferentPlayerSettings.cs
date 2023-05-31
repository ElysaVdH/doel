using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DifferentPlayerSettings : MonoBehaviour
{
    GameObject thisVideo;
    public GameObject thisChoise, nextVideo, volume;

    VideoPlayer thisPlayer;
    bool isPreparing;
    int videoFrames, showFrame;

    public bool isFirst;

    void Start()
    {
        thisVideo = this.gameObject;
        thisPlayer = thisVideo.GetComponent<VideoPlayer>();
        thisPlayer.Prepare();
        isPreparing = true;  
    }

    void Update()
    {
        if (isPreparing)
        {
            PrepareVideo();
        }

        if (isFirst)
        {
            volume.SetActive(false);

            if (!thisChoise.activeSelf)
            {
                if (thisPlayer.frame >= showFrame)
                {
                    thisChoise.SetActive(true);
                }
            }
            else
            {
                int fx = videoFrames - 1;
                if (thisPlayer.frame >= fx)
                {
                    thisVideo.SetActive(false);
                    nextVideo.SetActive(true);
                }
            }
        }
        else if (thisPlayer.isLooping)
        {
            volume.SetActive(false);

            if (!thisChoise.activeSelf)
            {
                thisChoise.SetActive(true);
            }
        }
        else
        {
            if (thisPlayer.isPaused)
            {
                volume.GetComponent<Image>().enabled = false;
            }
            else
            {
                volume.GetComponent<Image>().enabled = true;
            }
            if (thisPlayer.frame >= showFrame)
            {
                thisVideo.SetActive(false);
                nextVideo.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider != null)
                {
                    if (thisPlayer.isPaused)
                    {
                        thisPlayer.Play();
                    }
                    else
                    {
                        thisPlayer.Pause();
                    }
                }       
            }
        }
    }

    public void PrepareVideo()
    {
        int.TryParse(thisPlayer.frameCount.ToString(), out videoFrames);
        if (videoFrames != 0)
        {
            showFrame = videoFrames - 290;
            if(!isFirst)
            {
                showFrame = videoFrames - 10;
            }
            isPreparing = false;
        }
    }
}
