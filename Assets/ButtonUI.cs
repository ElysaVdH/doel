using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ButtonUI : MonoBehaviour
{
    //Transform parent;
    public Transform players, UIPanel, messages, left, right, switching, noSwitch;
    List<GameObject> allObjects;

    int buttonKind, timer;
    public GameObject nextVideo, choise;
    bool isAnimating, leftOrRight;
    static bool hasEverPaused;
    byte alpha;
    float volumeSet;

    public Slider volume;
    public GameObject pauseloop;
    public GameObject pausebuttons;

    void Start()
    {
        //animation
        isAnimating = false;
        hasEverPaused = false;

        //volume and stop
        volume.value = 1;
        pauseloop.SetActive(false);
        pausebuttons.SetActive(false);
        volume.gameObject.SetActive(false);

        //make total list
        allObjects = new List<GameObject>();

        for (int i = 0; i < players.childCount; i++)
        {
            GameObject b;

            if (players.GetChild(i).name == "KillerStory")
            {
                b = players.GetChild(i).gameObject;
                for (int j = 0; j < b.transform.childCount; j++)
                {
                    allObjects.Add(players.GetChild(i).GetChild(j).gameObject);
                }
            }
            else if (players.GetChild(i).name == "GirlStory")
            {
                b = players.GetChild(i).gameObject;
                for (int j = 0; j < b.transform.childCount; j++)
                {
                    allObjects.Add(players.GetChild(i).GetChild(j).gameObject);
                }
            }
            else
            {
                allObjects.Add(players.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < UIPanel.childCount; i++)
        {
            allObjects.Add(UIPanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < messages.childCount; i++)
        {
            allObjects.Add(messages.GetChild(i).gameObject);
        }
        allObjects.Add(left.gameObject);
        allObjects.Add(right.gameObject);
        allObjects.Add(switching.gameObject);
        allObjects.Add(noSwitch.gameObject);

        for (int i = 1; i < allObjects.Count; i++)
        {
            allObjects[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (nextVideo != null)
        {
            if (nextVideo.GetComponent<VideoPlayer>().isPaused)
            {
                volume.gameObject.SetActive(false);
            }
        }

        if (allObjects[0].activeSelf || allObjects[1].activeSelf)
        {
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                    QuitButton();
            }
        }

        if (isAnimating)
        {
            ButtonAnimation();
        }
        else if (nextVideo != null)
        {
            if (nextVideo.name != "VideoIntro")
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))
                {
                    hasEverPaused = true;
                    for (int i = 0; i < allObjects.Count; i++)
                    {
                        if (allObjects[i].name == "pressplay")
                        {
                            if (allObjects[i].activeSelf || nextVideo.GetComponent<VideoPlayer>().isPaused)
                            {
                                allObjects[i].SetActive(false);
                            }
                            else
                            {
                                StartCoroutine(ShowMessage());
                            }
                        }
                    }
                }
            }
        }
    }

    public IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(.5f);
        if(!nextVideo.GetComponent<VideoPlayer>().isPaused)
        {
        }
        else if (!isAnimating)
        {
            if (volume.gameObject.activeSelf || pauseloop.activeSelf)
            { }
            else
            {
                for (int i = 0; i < allObjects.Count; i++)
                {
                    if (allObjects[i].name == "pressplay")
                    {
                        allObjects[i].SetActive(true);
                    }
                }
            }
        }
    }

    public void VolumeSlider()
    {
        volumeSet = volume.value;

        for (int i = 0; i < players.childCount; i++)
        {
            Transform b;

            if (players.GetChild(i).name == "KillerStory")
            {
                b = players.GetChild(i);
                for (int j = 0; j < b.transform.childCount; j++)
                {
                    players.GetChild(i).GetChild(j).gameObject.GetComponent<VideoPlayer>().SetDirectAudioVolume(0, volumeSet);
                }
            }
            else if (players.GetChild(i).name == "GirlStory")
            {
                b = players.GetChild(i);
                for (int j = 0; j < b.transform.childCount; j++)
                {
                    players.GetChild(i).GetChild(j).gameObject.GetComponent<VideoPlayer>().SetDirectAudioVolume(0, volumeSet);
                }
            }
            else
            {
                players.GetChild(i).gameObject.GetComponent<VideoPlayer>().SetDirectAudioVolume(0, volumeSet);
            }
        }
    }

    public void VolumeSettings()
    {
        if (volume.gameObject.activeSelf)
        {
            volume.gameObject.SetActive(false);
        }
        else
        {
            volume.gameObject.SetActive(true);
        }
    }

    public void QuitButton()
    {
        nextVideo.GetComponent<VideoPlayer>().Pause();
        pauseloop.SetActive(true);
        pausebuttons.SetActive(true);
    }

    public void ButtonsWhenPaused(int q)
    {
        if (q == 1) 
        {
            volume.value = 0;
            noSwitch.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            noSwitch.gameObject.SetActive(true);
            Application.Quit();
            ///UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            pauseloop.SetActive(false);
            pausebuttons.SetActive(false);
            nextVideo.GetComponent<VideoPlayer>().Play();
        }
    }

    public void ButtonChoise(int x)
    {
        buttonKind = x;
    }

    public void ButtonClicked(GameObject next)
    {
        nextVideo = next;
        isAnimating = true;
        leftOrRight = false;
        timer = 0;
    }

    public void ButtonAnimation()
    {
        //start
        if (buttonKind == 3)
        {
            nextVideo.SetActive(true);
            StartNextVideo();
            isAnimating = false;
        }
        //switchwait
        if(buttonKind == 5)
        {
            if (!leftOrRight)
            {
                noSwitch.gameObject.SetActive(true);
                alpha = 0;
                noSwitch.GetComponent<Image>().color = new Color32(0, 0, 0, alpha);
                leftOrRight = true;
            }
            else if (alpha < 253)
            {
                alpha += 2;
                noSwitch.GetComponent<Image>().color = new Color32(0, 0, 0, alpha);
            }
            else
            {
                noSwitch.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                isAnimating = false;
                nextVideo.SetActive(true);
                StartNextVideo();
            }

        }

        else if (!leftOrRight)
        {
            //right
            if (buttonKind == 2)
            {
                choise = right.gameObject;
                leftOrRight = true;
            }
            //left
            else if (buttonKind == 1)
            {
                choise = left.gameObject;
                leftOrRight = true;
            }
            else if (buttonKind == 4)
            {
                choise = switching.gameObject;
                leftOrRight = true;
            }
        }
        else
        {
            if (timer >= 400)
            {
                choise.SetActive(true);
            }
            else if (timer >= 200)
            {
                choise.SetActive(false);
            }
            else
            {
                choise.SetActive(true);
            }
            timer += 8;

            if (timer >= 600)
            {
                isAnimating = false;
                nextVideo.SetActive(true);
                hasEverPaused = true;
                StartNextVideo();
            }
        }
    }

    public void StartNextVideo()
    {
        for (int i = 0; i < allObjects.Count; i++)
        {
            if (allObjects[i] != nextVideo)
            {
                allObjects[i].SetActive(false);
            }
        }
    }
    public static bool HasShown()
    {
        return hasEverPaused;
    }
}
