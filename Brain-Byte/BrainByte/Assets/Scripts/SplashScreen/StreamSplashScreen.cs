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
        SceneManager.LoadScene(menuScene);
    }

    /*
    IEnumerator StartVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);

        while(!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }

        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }
    */
}
