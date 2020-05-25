using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public GameObject loadingScreen;
    public GameObject completeLevelUI;
    public GameObject gameOverUI;
    public Slider slider;

    public Animator transition;
    public float transitionTime = 1f;
    //private PhotonRoom photonRoom;
    private PhotonView PV;

    void Start()
    {
        completeLevelUI = FindObjectOfType<GameOverReference>().gameObject;
        gameOverUI = FindObjectOfType<VictoryReference>().gameObject;

        if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
        {
            //photonRoom = FindObjectOfType<PhotonRoom>();
            PV = GetComponent<PhotonView>();
        }
        else if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            FindObjectOfType<SoloAvatarSetup>().InstantiateSoloPlayer();
        }
            //instantiate player if not in main menu scene
    }
    void Update()
    {
        /* TEST */
        if (Input.GetKeyDown(KeyCode.Alpha9))
            PV.RPC("LoadLevel", RpcTarget.All, SceneManager.GetActiveScene().buildIndex + 1);    // Test to move to next scene: must be replaced

    }

    [PunRPC]
    public void RestartLevel()
    {
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }
    
    [PunRPC]
    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }

    [PunRPC]
    public void MoveToNextLevel()
    {
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
    }

    [PunRPC]
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
        //photonRoom.isFirtOnlineLevel = false;   // migth need to put this after StarCoroutine
        Debug.Log("Instantiating character");
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        /*if (!photonRoom.isFirtOnlineLevel)
        {
            PhotonPlayer[] players = FindObjectsOfType<PhotonPlayer>();
            Debug.Log("Loading players into new scene");
            foreach (PhotonPlayer player in players)
                player.OnMovedToNextLevel();
        }*/

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
        {
            PhotonNetwork.LoadLevel(sceneIndex);

            loadingScreen.SetActive(true);

            while (PhotonNetwork.LevelLoadingProgress != 1)
            {
                float progress = Mathf.Clamp01(PhotonNetwork.LevelLoadingProgress / .9f);
                slider.value = progress;

                yield return null;
            }

            Debug.Log("Trying to yield return after loop");
            // Don't add any code after while loop, it won't run
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

}
