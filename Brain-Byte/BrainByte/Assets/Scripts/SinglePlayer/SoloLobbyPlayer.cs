using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloLobbyPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject myAvatar;

    void CreatePlayer()
    {
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);

        Instantiate(myAvatar, GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
