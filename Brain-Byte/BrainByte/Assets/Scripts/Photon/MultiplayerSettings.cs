using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings multiplayerSettings;

    public bool delayStart;
    public int maxPlayers;
    public int menuScene;
    public int multiPlayerScene;
    public bool isMultiplayer = false;
    public bool isTeamMode = false;
    public int currentScene;

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene().buildIndex;

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
