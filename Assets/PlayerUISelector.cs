using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayerUISelector : MonoBehaviour
{
    //videostuff
    GameObject thisVideo;
    VideoPlayer thisPlayer;
    bool isPreparing;
    int videoFrames, showFrame, choiseFrame, n;

    //buttonstuff
    public GameObject thisChoise, volume;
    RectTransform bottom, top, timeBar;
    Button left, right;
    float w, h;

    bool isReady, isClicked;

    public GameObject chooseMessage, firstMessage;

    void Start()
    {
        //buttonsettings
        bottom = thisChoise.transform.Find("bottom").GetComponent<RectTransform>();
        top = thisChoise.transform.Find("top").GetComponent<RectTransform>();
        timeBar = thisChoise.transform.Find("time").GetComponent<RectTransform>();

        foreach (Transform b in thisChoise.transform.Find("bottom"))
        {
            if (b.name == "Left")
            {
                left = b.GetComponent<Button>();
            }
            else if (b.name == "Right")
            {
                right = b.GetComponent<Button>();
            }
            else if (b.name == "NoSwitch")
            {
                right = b.GetComponent<Button>();
            }
        }
        ButtonStartSettings();

        //playersettings
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

        if (thisPlayer.isPaused)
        {
            volume.GetComponent<Image>().enabled = false;
        }
        else
        {
            volume.GetComponent<Image>().enabled = true;
        }

        if (thisChoise.activeSelf)
        {
            ChoiseAnimation();
        }
        else if (thisPlayer.frame >= showFrame)
        {
            thisChoise.SetActive(true);
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

        //if first player
        if (firstMessage != null)
        {
            if (!ButtonUI.HasShown())
            {
                if (thisPlayer.frame >= 30 && thisPlayer.frame < 300)
                {
                    firstMessage.SetActive(true);
                }
                else if (thisPlayer.frame >= 300)
                {
                    firstMessage.SetActive(false);
                }
            }
            else
            {
                firstMessage.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                firstMessage.SetActive(false);
            }
        }
        if (thisPlayer.frame < 5)
        {
            ButtonStartSettings();
        }
    }

    public void PrepareVideo()
    {
        int.TryParse(thisPlayer.frameCount.ToString(), out videoFrames);
        if (videoFrames != 0)
        {
            showFrame = videoFrames - 150;
            showFrame -= 80;
            isPreparing = false;
        }
    }

    public void ButtonStartSettings()
    {
        thisChoise.SetActive(false);

        volume.SetActive(true);

        foreach (Transform b in thisChoise.transform.Find("bottom"))
        {
            b.gameObject.SetActive(false);
        }

        h = timeBar.sizeDelta.y;
        timeBar.sizeDelta = new Vector2(0, h);

        h = 70;
        w = bottom.sizeDelta.x;
        bottom.sizeDelta = new Vector2(w, h);
        top.sizeDelta = new Vector2(w, h);
        isReady = false;
        choiseFrame = showFrame;
        isClicked = false;
    }

    public void ChoiseAnimation()
    {
        if (!isReady)
        {
            if (thisPlayer.frame > choiseFrame)
            {
                choiseFrame++;
                h++;

                if(h == 150)
                {
                    isReady = true;
                }
                else if (thisPlayer.frame - choiseFrame == 1)
                {
                    h++;
                    choiseFrame++;
                }
                if (h == 150)
                {
                    isReady = true;
                }

                bottom.sizeDelta = new Vector2(w, h);
                top.sizeDelta = new Vector2(w, h);

                if (isReady)
                {
                    h = timeBar.sizeDelta.y;
                    w = 0;
                    foreach (Transform b in thisChoise.transform.Find("bottom"))
                    {
                        b.gameObject.SetActive(true);
                    }
                    if (left != null)
                    {
                        chooseMessage.SetActive(true);
                        n = Random.Range(0, 2);
                    }
                }
            }
        }
        else
        {
            //time up
            if (w >= 1300)
            {
                thisPlayer.Pause();
                if (!isClicked)
                {
                    if (left != null)
                    {
                        if (n == 1)
                        {
                            left.onClick.Invoke();
                            isClicked = true;
                        }
                        else
                        {
                            right.onClick.Invoke();
                            isClicked = true;
                        }
                    }
                    else
                    {
                        right.onClick.Invoke();
                        isClicked = true;
                    }
                }
            }
            else
            {
                if (thisPlayer.frame > choiseFrame)
                {
                    w += 10;
                    choiseFrame++;
                    while (thisPlayer.frame - choiseFrame == 1)
                    {
                        w += 10;
                        choiseFrame++;
                    }

                    timeBar.sizeDelta = new Vector2(w, h);
                }
            }

            if (left != null)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    left.onClick.Invoke();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    right.onClick.Invoke();
                }
            }
        }
    }
}
