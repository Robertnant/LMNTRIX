using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerAvatarCombat : MonoBehaviour
{
    private PhotonView PV;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
            return;

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward), out hit, 1000))
            {
                Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * hit.distance, Color.red);
                Debug.Log("Did it");

                if(hit.transform.tag == "Avatar")
                {
                    hit.transform.gameObject.GetComponent<AvatarSetup>().playerHealth -= avatarSetup.playerDamage;
                }
            }
            else
            {
                Debug.DrawRay(rayOrigin.position, rayOrigin.TransformDirection(Vector3.forward) * 1000, Color.white);
                Debug.Log("Did not hit");
            }
        }
    }
}
