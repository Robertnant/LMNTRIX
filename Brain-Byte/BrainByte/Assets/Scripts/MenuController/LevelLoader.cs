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

            while (PhotonNetwork.LevelLoadingProgress != 100)
            {
                Debug.Log($"Loading scene: {PhotonNetwork.LevelLoadingProgress * 100}%");

                yield return null;
            }

            Debug.Log($"Loading scene: 100%");
        }
        else
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

            while (!operation.isDone)
            {
                Debug.Log($"Loading scene: {operation.progress * 100}%");

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
