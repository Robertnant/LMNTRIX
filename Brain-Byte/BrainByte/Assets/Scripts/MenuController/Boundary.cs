using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Boundary : MonoBehaviour
{
    public LevelLoader levelLoader;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
            {
                Debug.Log(levelLoader.photonView);
                levelLoader.photonView.RPC("LoseLevel", RpcTarget.All);
            }
            else
            {
                levelLoader.LoseLevel();
            }
        }
    }
}
