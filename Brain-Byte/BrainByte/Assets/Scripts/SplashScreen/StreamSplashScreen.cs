using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class StreamSplashScreen : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public int menuScene;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(StartVideo());
        videoPlayer.Play();
        videoPlayer.loopPointReached += LoadScene;
    }

    void LoadScene(VideoPlayer vidP)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(menuScene);
    }

}
