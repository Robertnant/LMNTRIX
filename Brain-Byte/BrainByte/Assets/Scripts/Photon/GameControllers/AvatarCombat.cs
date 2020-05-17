using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCombat : MonoBehaviour
{
    private PhotonView PV;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;
    public Animator animator;

    // for attack
    public Transform attackPoint;
    public float attackRange = 0.6f;
    public LayerMask enemyLayers;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();

        rayOrigin = avatarSetup.myCharacter.transform;
        animator = avatarSetup.animator;

        // Get attack point
        foreach (Transform t in avatarSetup.myCharacter.transform)
        {
            if (t.tag == "AttackPoint")
            {
                attackPoint = t;
            }
        }

    }

    void Update()
    {
        if (!PV.IsMine)
            return;

        // might not be necessary
        if (animator == null)
            animator = avatarSetup.animator;

        if (Input.GetMouseButtonDown(0))
        {
            PV.RPC("RPC_Shooting", RpcTarget.All);
        }
        if (Input.GetMouseButtonDown(1))
        {
            PV.RPC("RPC_Hit", RpcTarget.All);
        }
    }

    [PunRPC]
    void RPC_Hit()
    {
        // Play attack animation
        animator.SetTrigger("Punch");

        // Detect enemy
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Damage enemy
        foreach (Collider enemy in hitEnemies)
        {
            if (!enemy.gameObject.GetComponent<PhotonView>().IsMine)    // prevent player from hitting itself
            {
                Debug.Log($"Player {PV.ViewID} hit {enemy.gameObject.GetComponent<PhotonView>().ViewID}");

                // Deduct health
                HeadsUpDisplay enemyHUD = enemy.gameObject.GetComponent<HeadsUpDisplay>();

                if (enemyHUD != null)
                {
                    enemyHUD.WasHit();
                    Debug.Log("Set new health");
                    Debug.Log($"Enemy's local health is now: {enemyHUD.playerHealth}");
                }
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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
                Debug.Log($"Player {PV.ViewID} shot {hit.transform.gameObject.GetComponent<PhotonView>().ViewID}");
                HeadsUpDisplay enemyHUD = hit.transform.gameObject.GetComponent<HeadsUpDisplay>(); // to modify with new script

                if (enemyHUD != null)
                {
                    enemyHUD.WasHit();
                    Debug.Log("Set new health");
                    Debug.Log($"Enemy's local health is now: {enemyHUD.playerHealth}");
                }
                else
                {
                    // Display all availble components of enemy
                    Debug.Log("Enemy AvatarSetup is null");
                    
                    Component[] enemyComponents = hit.collider.gameObject.GetComponents<Component>();
                    
                    Debug.Log("Available ennemy components are: ");

                    foreach (Component comp in enemyComponents)
                        Debug.Log("Supposed to be Enemy avatar's comps: " + comp.name + " " + comp);

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
