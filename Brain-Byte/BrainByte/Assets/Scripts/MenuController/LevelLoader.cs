﻿using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public GameObject loadingScreen;
    public GameObject completeLevelUI;
    public GameObject finishGameUI;
    public GameObject gameOverUI;
    public Slider slider;

    public Animator transition;
    public float transitionTime = 1f;
    //private PhotonRoom photonRoom;
    private PhotonView PV;

    public int level = 3;   // default

    void Start()
    {
        if (FindObjectOfType<VictoryReference>() != null)
            completeLevelUI = FindObjectOfType<VictoryReference>().gameObject;
        if (FindObjectOfType<GameOverReference>() != null)
            gameOverUI = FindObjectOfType<GameOverReference>().gameObject;
        if (FindObjectOfType<FinishGameReference>() != null)
            finishGameUI = FindObjectOfType<FinishGameReference>().gameObject;


        if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
        {
            //photonRoom = FindObjectOfType<PhotonRoom>();
            PV = GetComponent<PhotonView>();
        }
        /*
        else if (SceneManager.GetActiveScene().name != "Menu")
        {
            Debug.Log("not in menu");

            int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);

            Instantiate(PlayerInfo.PI.allCharacters[PlayerInfo.PI.selectedCharacter],
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation);
        }
            //instantiate player if not in main menu scene
            */
    }
    void Update()
    {
        /* TEST */
        if (Input.GetKeyDown(KeyCode.Alpha9))
            PV.RPC("LoadLevel", RpcTarget.All, SceneManager.GetActiveScene().buildIndex + 1);    // Test to move to next scene: must be replaced

        if (SceneManager.GetActiveScene().name == "LEVEL 4")
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (finishGameUI != null)
                {
                    finishGameUI.SetActive(false);
                    LoadCredits();
                }
            }
    }

    [PunRPC]
    public void RestartLevel()
    {
        completeLevelUI.SetActive(false);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex));
    }
    
    [PunRPC]
    public void CompleteLevel()
    {
        if (SceneManager.GetActiveScene().name != "LEVEL 4")
        {
            completeLevelUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            finishGameUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    [PunRPC]
    public void LoseLevel()
    {
        gameOverUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    [PunRPC]
    public void MoveToNextLevel()
    {
        completeLevelUI.SetActive(false);
        StartCoroutine(LoadAsynchronously(SceneManager.GetActiveScene().buildIndex + 1));
    }

    [PunRPC]
    public void LoadLevel(int sceneIndex)
    {
        if (!MultiplayerSettings.multiplayerSettings.isMultiplayer && FindObjectOfType<PhotonRoom>() != null)
            Destroy(FindObjectOfType<PhotonRoom>().gameObject);

        StartCoroutine(LoadAsynchronously(sceneIndex));
        //photonRoom.isFirtOnlineLevel = false;   // migth need to put this after StarCoroutine
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;

        if (!MultiplayerSettings.multiplayerSettings.isMultiplayer)
            SceneManager.LoadScene("Menu");
        else
            GameSetup.GS.DisconnectPlayer();
    }

    public void ResumeLevel()
    {
        PlayerData data = SaveSystem.LoadPayer();

        if (data != null)
        {
            level = data.level;
            PlayerInfo.PI.selectedCharacter = data.currentCharacter;

            if (FindObjectOfType<MenuController>() != null)
                FindObjectOfType<MenuController>().OnClickCharacterPick(data.currentCharacter);
        }

        LoadLevel(level);
    }
    public void SaveLevel()
    {
        SaveSystem.SavePlayer(MultiplayerSettings.multiplayerSettings);
    }

    public void DeleteSave()
    {
        SaveSystem.DeleteData();
        level = 3;
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
