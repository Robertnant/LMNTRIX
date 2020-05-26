using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public LevelLoader levelLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
        {
            Debug.Log(levelLoader.photonView);
            levelLoader.photonView.RPC("CompleteLevel", RpcTarget.All);
        }
        else
        {
            Debug.Log("trying to load next singleplayer level");
            levelLoader.CompleteLevel();
        }
    }

}
