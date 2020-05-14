using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCombat : MonoBehaviour
{
    private PhotonView PV;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();

        rayOrigin = avatarSetup.myCharacter.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;

        /*
        if(rayOrigin == null)
            rayOrigin = avatarSetup.myCharacter.transform;
            */

        if (Input.GetMouseButtonDown(0))
        {
            PV.RPC("RPC_Shooting", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_Shooting()
    {
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            if (hit.transform.tag == "Character")
            {
                // Test with new always active function in charge of decreasing player health

                /*Debug.Log($"Player {PV.ViewID} shot {hit.transform.gameObject.GetComponent<PhotonView>().ViewID}"); 
                hit.transform.parent.gameObject.GetComponent<HealthController>().WasHit(25);
                Debug.Log("Changed player's health");*/

                
                Debug.Log($"Player {PV.ViewID} shot {hit.transform.gameObject.GetComponent<PhotonView>().ViewID}");
                HeadsUpDisplay enemyHUD = hit.collider.gameObject.GetComponent<HeadsUpDisplay>(); // to modify with new script

                if (enemyHUD != null)
                {
                    enemyHUD.playerHealth -= avatarSetup.playerDamage;
                    Debug.Log("Set new health");
                    enemyHUD.healthBar.SetHealth(enemyHUD.playerHealth);
                    Debug.Log($"Player's health is now: {enemyHUD.playerHealth}");
                }
                else
                {
                    // Display all availble components of enemy
                    Debug.Log("Enemy AvatarSetup is null");
                    Component[] enemyComponents = hit.collider.gameObject.GetComponents<Component>();   //maybe should be changed to transform
                    /*Component[] enemyComponentsChildren = hit.collider.gameObject.GetComponentsInChildren<Component>();
                    Component[] enemyComponentsParent = hit.collider.gameObject.GetComponentsInParent<Component>();*/   // should be empty in theory

                    Debug.Log("Available ennemy components are: ");

                    foreach (Component comp in enemyComponents)
                        Debug.Log("Supposed to be Enemy avatar's comps: " + comp.name + " " + comp);

                    /*Debug.Log("Line break");

                    foreach (Component childComp in enemyComponentsChildren)
                        Debug.Log("Supposed to be Enemy avatar's Child comps: " + childComp.name + " " + childComp);

                    Debug.Log("Line break");

                    foreach (Component parentComp in enemyComponentsParent)
                        Debug.Log("Supposed to be Enemy avatar's Parent comps: " + parentComp.name + " " + parentComp);*/

                }

            }
            else
            {
                Debug.Log($"Hit a random {hit.transform.tag} object");
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not hit anyone");
        }

    }
}
