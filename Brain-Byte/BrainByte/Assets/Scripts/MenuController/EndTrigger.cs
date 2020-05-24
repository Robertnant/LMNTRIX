using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public LevelLoader levelLoader;

    private void OnTriggerEnter(Collider other)
    {
        levelLoader.photonView.RPC("CompleteLevel", RpcTarget.All);
    }

}
