using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int currentCharacter;

    public PlayerData (MultiplayerSettings settings)
    {
        level = settings.currentScene + 1;
        currentCharacter = PlayerInfo.PI.selectedCharacter;
    }
}
