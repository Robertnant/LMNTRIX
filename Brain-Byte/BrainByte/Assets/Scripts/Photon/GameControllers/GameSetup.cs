using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    public Transform[] spawnPoints;

    private void OnEnable()
    {
        if (GameSetup.GS == null)
            GameSetup.GS = this;
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();

        while (PhotonNetwork.IsConnected)
            yield return null;
        SceneManager.LoadScene(0);
    }
}
