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
            Debug.Log($"Player { PV.ViewID} Hit someone");
            Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            if (hit.transform.tag == "Player")
            {
                Debug.Log($"Player {PV.ViewID} shot {hit.transform.gameObject.GetComponent<PhotonView>().ViewID}");
                hit.transform.gameObject.GetComponent<AvatarSetup>().playerHealth -= avatarSetup.playerDamage;
                hit.transform.gameObject.GetComponent<AvatarSetup>().healthBar.SetHealth(
                    hit.transform.gameObject.GetComponent<AvatarSetup>().playerHealth);
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
