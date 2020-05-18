using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public bool isMultiplayer = false;
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {

        if (isMultiplayer)
        {
            PhotonNetwork.LoadLevel(MultiplayerSettings.multiplayerSettings.multiPlayerScene);

            while (PhotonNetwork.LevelLoadingProgress != 1)
            {
                float progress = Mathf.Clamp01(PhotonNetwork.LevelLoadingProgress / .9f);
                Debug.Log($"Loading Multiplayer scene: {(int) (progress * 100)}%");

                yield return null;
            }
        }
        else
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                Debug.Log($"Loading Singleplayer scene: {(int) (progress * 100)}%");

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
