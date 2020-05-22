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
    public Animator transition;
    private float transitionTime = 1f;
    public bool isFirtOnlineLevel = true;

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Alpha9))
            LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);    // Test to move to next scene: must be replaced

        if (Input.GetKeyDown(KeyCode.Alpha8))   // This is just a test: must be deleted
            LoadLevel(4);*/
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        if (isMultiplayer)
        {
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiPlayerScene);

            if (!isFirtOnlineLevel)
                FindObjectOfType<PhotonPlayer>().OnMovedToNextLevel();

            loadingScreen.SetActive(true);

            while (PhotonNetwork.LevelLoadingProgress != 1)
            {
                float progress = Mathf.Clamp01(PhotonNetwork.LevelLoadingProgress / .9f);
                slider.value = progress;

                yield return null;
            }

            isFirtOnlineLevel = false;
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
