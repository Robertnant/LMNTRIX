using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;
    public int menuScene;

    public Transform[] spawnPoints;

    private void OnEnable()
    {
        if (GameSetup.GS == null)
            GameSetup.GS = this;

        if (!MultiplayerSettings.multiplayerSettings.isMultiplayer)
        {
            int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);

            Instantiate(PlayerInfo.PI.allCharacters[PlayerInfo.PI.selectedCharacter],
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation);

            Debug.Log("Instantiating character");
        }
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.InRoom)
            yield return null;
        SceneManager.LoadScene(menuScene);
    }
}
