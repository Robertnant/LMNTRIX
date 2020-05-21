using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public bool isMultiplayer = false;
    public GameObject loadingScreen;
    public Slider slider;
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {

        if (isMultiplayer)
        {
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiPlayerScene);

            loadingScreen.SetActive(true);

            while (PhotonNetwork.LevelLoadingProgress != 1)
            {
                float progress = Mathf.Clamp01(PhotonNetwork.LevelLoadingProgress / .9f);
                slider.value = progress;

                yield return null;
            }
        }
        else
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                slider.value = progress;

                yield return null;
            }
        }

    }

    // Reset isMultiplayer to false (useful when going back to main menu)
    public bool NotMultiplayer
    {
        set { isMultiplayer = false; }
    }
}
