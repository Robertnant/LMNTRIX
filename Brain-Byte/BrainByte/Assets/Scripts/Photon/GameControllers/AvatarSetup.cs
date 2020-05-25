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
    public int maxHealth = 100;
    public int playerDamage;
    public HealthBar healthBar;

    public Camera myCamera;
    private AudioListener myAL;
    public Animator animator;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        Debug.Log($"{PV.GetInstanceID()}");

        if (PV.IsMine)
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

        // Get Camera and enable it

        foreach (Transform t in myCharacter.transform)
        {
            if (t.tag == "AvatarCamera")
            {
                t.gameObject.SetActive(true);
                myCamera = t.gameObject.GetComponent<Camera>();
            }
        }
        
        animator = myCharacter.GetComponent<Animator>();

        /* Remotely set all health stats*/

        PV.RPC("SetRemoteVals", RpcTarget.All);

        // check if HeadsUpDisplay values were set
        if (myCharacter.GetComponent<HeadsUpDisplay>().maxHealth == maxHealth &&
            //myCharacter.GetComponent<HeadsUpDisplay>().playerDamage == playerDamage &&
            myCharacter.GetComponent<HeadsUpDisplay>().playerHealth == playerHealth)
        {
            myCharacter.GetComponent<HeadsUpDisplay>().valsSet = true;
        }

    }

    [PunRPC]
    void SetRemoteVals()
    {
        myCharacter.GetComponent<HeadsUpDisplay>().maxHealth = maxHealth;
        //myCharacter.GetComponent<HeadsUpDisplay>().playerDamage = playerDamage;
        myCharacter.GetComponent<HeadsUpDisplay>().playerHealth = playerHealth;

    }
    
    void Update()
    {
        // if HeadsUpDisplay values were not set
        if (!myCharacter.GetComponent<HeadsUpDisplay>().valsSet)
        {
            Debug.Log("Trying to set HeadsUpDisplay values again");
            
            PV.RPC("SetRemoteVals", RpcTarget.All);
            myCharacter.GetComponent<HeadsUpDisplay>().valsSet = true;
        }

    }
}
