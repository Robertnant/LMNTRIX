using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloAvatarSetup : MonoBehaviour
{
    public int characterValue;
    public void InstantiateSoloPlayer()
    {
        //levelLoader.LoadLevel(MultiplayerSettings.multiplayerSettings.multiPlayerScene);
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);

        characterValue = PlayerInfo.PI.selectedCharacter;
        Instantiate(PlayerInfo.PI.allCharacters[PlayerInfo.PI.selectedCharacter], 
            GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation); //, transform);
    }

}
