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

            if (hit.transform.tag == "Player")
            {
                // Test with new always active function in charge of decreasing player health

                Debug.Log($"Player {PV.ViewID} shot {hit.transform.gameObject.GetComponent<PhotonView>().ViewID}");
                hit.transform.parent.gameObject.GetComponent<HealthController>().WasHit(25);
                Debug.Log("Changed player's health");

                /*
                Debug.Log($"Player {PV.ViewID} shot {hit.transform.gameObject.GetComponent<PhotonView>().ViewID}");
                hit.transform.parent.gameObject.GetComponent<AvatarSetup>().playerHealth -= avatarSetup.playerDamage;
                Debug.Log("Set new health");
                hit.transform.parent.gameObject.GetComponent<AvatarSetup>().healthBar.SetHealth(
                    hit.transform.parent.gameObject.GetComponent<AvatarSetup>().playerHealth);
                Debug.Log($"Player's health is now: {hit.transform.parent.gameObject.GetComponent<AvatarSetup>().playerHealth}");
                */
            }
            else
            {
                Debug.Log(hit.transform.tag);
            }
        }
        else
        {
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not hit anyone");
        }

    }
}
