using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myCharacter;
    public int characterValue;
    public int playerHealth;
    public int playerDamage;

    public Camera myCamera;
    private AudioListener myAL;
    public Animator animator;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        Debug.Log($"{PV.GetInstanceID()}");
        
        if(PV.IsMine)
        {
            Debug.Log("Added character");
            AddCharacter(PlayerInfo.PI.selectedCharacter);
        }
        else
        {
            Destroy(myCamera);
            Destroy(myAL);
            Debug.Log("Did not add character");
        }
    }

    void AddCharacter(int characterIndex)
    {
        characterValue = characterIndex;
        myCharacter = PhotonNetwork.Instantiate(Path.Combine(@"PhotonPrefabs\Characters", PlayerInfo.PI.allCharacters[characterIndex].name),
            transform.position, transform.rotation);

        Debug.Log("Created player");

        myCharacter.transform.parent = transform;
        animator = myCharacter.GetComponent<Animator>();

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
