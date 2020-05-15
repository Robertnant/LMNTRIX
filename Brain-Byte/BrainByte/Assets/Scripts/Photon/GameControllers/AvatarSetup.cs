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

        // Get Camera and enable it

        foreach (Transform t in myCharacter.transform)
        {
            if (t.tag == "AvatarCamera")
            {
                t.gameObject.SetActive(true);
            }
        }
        
        animator = myCharacter.GetComponent<Animator>();

        /*
        // Set canvas as active and get HealthBar

        foreach (GameObject gameObj in gameObject.scene.GetRootGameObjects())
        {
            if(gameObj.name == "Canvas")
            {
                gameObj.SetActive(true);

                foreach (Transform t in gameObj.transform)
                {
                    if(t.tag == "HealthBar")
                    {
                        healthBar = t.gameObject.GetComponent<HealthBar>();
                        healthBar.SetMaxHealth(maxHealth);
                        break;
                    }
                }

                break;
            }
            
        }

        // Assign health bar and all health info in HeadsUpDisplay script of avatar's character:

        myCharacter.GetComponent<HeadsUpDisplay>().healthBar = healthBar;*/     // old way of getting health bar before assigning a canvas to each player
        myCharacter.GetComponent<HeadsUpDisplay>().maxHealth = maxHealth;
        myCharacter.GetComponent<HeadsUpDisplay>().playerDamage = playerDamage;
        myCharacter.GetComponent<HeadsUpDisplay>().playerHealth = playerHealth;

        // check if HeadsUpDisplay values were set
        if (myCharacter.GetComponent<HeadsUpDisplay>().maxHealth == maxHealth && 
            myCharacter.GetComponent<HeadsUpDisplay>().playerDamage == playerDamage &&
            myCharacter.GetComponent<HeadsUpDisplay>().playerHealth == playerHealth)
        {
            myCharacter.GetComponent<HeadsUpDisplay>().valsSet = true;
        }

    }

    void Update()
    {
        // if HeadsUpDisplay values were not set
        if (!myCharacter.GetComponent<HeadsUpDisplay>().valsSet)
        {
            Debug.Log("Trying to set HeadsUpDisplay values again");
            myCharacter.GetComponent<HeadsUpDisplay>().maxHealth = maxHealth;
            myCharacter.GetComponent<HeadsUpDisplay>().playerDamage = playerDamage;
            myCharacter.GetComponent<HeadsUpDisplay>().playerHealth = playerHealth;
            
            myCharacter.GetComponent<HeadsUpDisplay>().valsSet = true;
        }

        /*if (myCharacter.GetComponent<HeadsUpDisplay>().maxHealth != maxHealth)
        {
            Debug.Log("Trying to set player max health");
            myCharacter.GetComponent<HeadsUpDisplay>().maxHealth = maxHealth;
        }

        if (myCharacter.GetComponent<HeadsUpDisplay>().playerDamage != playerDamage)
        {
            Debug.Log("Trying to set player damage");
            myCharacter.GetComponent<HeadsUpDisplay>().playerDamage = playerDamage;
        }

        if (myCharacter.GetComponent<HeadsUpDisplay>().playerHealth != playerHealth)
        {
            Debug.Log("Trying to set player health");
            myCharacter.GetComponent<HeadsUpDisplay>().playerHealth = playerHealth;
        }
        */
    }
}
