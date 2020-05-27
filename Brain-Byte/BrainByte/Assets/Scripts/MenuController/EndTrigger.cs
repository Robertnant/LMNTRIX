using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public LevelLoader levelLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
            {
                Debug.Log(levelLoader.photonView);
                levelLoader.photonView.RPC("CompleteLevel", RpcTarget.All);
            }
            else
            {
                Debug.Log("trying to load next singleplayer level");
                if (SceneManager.GetActiveScene().name != "LEVEL 4")
                {
                    levelLoader.SaveLevel();
                    Debug.Log("Saving level");
                }
                else
                {
                    levelLoader.DeleteSave();
                }
                levelLoader.CompleteLevel();
            }
        }
    }

}
