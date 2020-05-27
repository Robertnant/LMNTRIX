using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings multiplayerSettings;

    public bool delayStart;
    public int maxPlayers;
    public int menuScene;
    public int multiPlayerScene;
    public bool isMultiplayer = false;
    public bool isTeamMode = false;

    void Awake()
    {
        if (MultiplayerSettings.multiplayerSettings == null)
        {
            MultiplayerSettings.multiplayerSettings = this;
        }
        else
        {
            if (MultiplayerSettings.multiplayerSettings != this)
                Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Reset isMultiplayer to false (useful when going back to main menu)
    public void SetMultiplayer (bool val)
    {
        isMultiplayer = val;
    }
    
    public void SetTeamMode (bool val)
    {
        isTeamMode = val;
    }
}
