﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public static GameObject myAvatar;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        int spawnPicker = Random.Range(0, GameSetup.GS.spawnPoints.Length);
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                GameSetup.GS.spawnPoints[spawnPicker].position, GameSetup.GS.spawnPoints[spawnPicker].rotation, 0);

            // Enable scripts of character object
            myAvatar.GetComponent<AvatarSetup>().enabled = true;
            myAvatar.GetComponent<V2PlayerMovement>().enabled = true;
            myAvatar.GetComponent<AvatarCombat>().enabled = true;

            Debug.Log("Player Avatar instantiated");
        }
    }

}
