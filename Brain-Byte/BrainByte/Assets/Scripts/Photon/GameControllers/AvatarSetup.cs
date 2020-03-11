using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myCharacter;
    public int characterValue;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        if(PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.selectedCharacter);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int characterIndex)
    {
        characterValue = characterIndex;
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[characterIndex], transform.position, transform.rotation,
            transform);
    }
}
