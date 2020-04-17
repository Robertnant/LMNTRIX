using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myCharacter;
    public int characterValue;
    public int playerHealth;
    public int playerDamage;

    public Camera myCamera;
    private AudioListener myAL;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        if(PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.selectedCharacter);
        }
        else
        {
            Destroy(myCamera);
            Destroy(myAL);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int characterIndex)
    {
        characterValue = characterIndex;
        myCharacter = Instantiate(PlayerInfo.PI.allCharacters[characterIndex], transform.position, transform.rotation,
            transform);

        foreach(Transform t in myCharacter.transform)
        {
            if (t.tag == "AvatarCamera")
            {
                myCamera = t.gameObject.GetComponent<Camera>();
                myAL = t.gameObject.GetComponent<AudioListener>();

                Debug.Log("Found camera");
                break;
            }
            else
                Debug.Log("Did not find Camera");
        }

    }
}
