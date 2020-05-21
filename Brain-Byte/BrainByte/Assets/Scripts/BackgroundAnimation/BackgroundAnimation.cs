using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class BackgroundAnimation : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public AudioSource backgroundMusic;
    public AudioSource backgroundWind;
    public int menuScene;
    public GameObject menuDisplay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartVideo());
    }

    IEnumerator StartVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        while(!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }

        videoPlayer.Play();
        backgroundMusic.Play();
        backgroundWind.Play();
        menuDisplay.SetActive(true);
    }
}
