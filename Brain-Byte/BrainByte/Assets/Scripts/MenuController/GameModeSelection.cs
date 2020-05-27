using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeSelection : MonoBehaviour
{
    public GameObject modeButton;
    // Start is called before the first frame update
    public void OnModeButtonClicked()
    {
        if (modeButton.GetComponentInChildren<Text>().text == "TEAM MODE")
        {
            MultiplayerSettings.multiplayerSettings.isTeamMode = false;
            modeButton.GetComponentInChildren<Text>().text = "FREE FOR ALL";
            modeButton.GetComponentInChildren<Text>().color = Color.green;
        }
        else
        {
            MultiplayerSettings.multiplayerSettings.isTeamMode = true;
            modeButton.GetComponentInChildren<Text>().text = "TEAM MODE";
            modeButton.GetComponentInChildren<Text>().color = Color.red;
        }
    }
}
