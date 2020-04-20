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
    public HealthBar healthBar;     // New

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


    }
}
