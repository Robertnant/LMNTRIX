using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCombat : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Attributes

    private PhotonView PV;
    private AvatarSetup avatarSetup;
    public Transform rayOrigin;
    public Animator animator;

    // for attack
    public Transform weaponHolder;
    public Transform attackPoint;
    public float attackRange = 0.6f;
    public float attackRate = 1.25f;
    private float nextAttackTime = 0f;
    public LayerMask enemyLayers;
    public int playerDamage;

    public int selectedWeapon = -1; // -1 for no weapon
    public float attackSelectionRate = 2f;
    private float nextSelectionTime = 0f;
    public string currentWeapon;

    #endregion

    void Start()
    {
        PV = GetComponent<PhotonView>();
        avatarSetup = GetComponent<AvatarSetup>();

        rayOrigin = avatarSetup.myCharacter.transform;
        animator = avatarSetup.animator;

        /* Get weapon holder and attack point*/

        foreach (Transform t in avatarSetup.myCharacter.transform)
        {
            if (t.tag == "HitPoint")
            {
                attackPoint = t;
            }
        }

        FindWeaponHolder(avatarSetup.myCharacter.transform);
        Debug.Log("Found weapon holder: " + weaponHolder.name);

        PV.RPC("RPC_SelectWeapon", RpcTarget.All);

    }

    void Update()
    {
        
        if (!PV.IsMine)
            return;

        // might not be necessary
        if (animator == null)
            animator = avatarSetup.animator;

        // for remote weapon switch
        //RPC_SelectWeapon();  // if fails, change to normal call

        // attack
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                // Play attack animation
                animator.SetTrigger(currentWeapon);

                if (currentWeapon == "SARP" || currentWeapon == "Pistol")
                {
                    // test without rpc
                    PV.RPC("RPC_Shooting", RpcTarget.All);
                    //RPC_Shooting();
                }
                else
                {
                    // test without rpc
                    PV.RPC("RPC_Hit", RpcTarget.All);
                    //RPC_Hit();
                }

                nextAttackTime = Time.time + 1f / attackRate;
            }
            
        }

        // weapon switch
        if (Time.time >= nextSelectionTime)
        {
            int previousSelectedWeapon = selectedWeapon;

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= weaponHolder.childCount - 1)
                    selectedWeapon = -1;
                else
                    selectedWeapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= -1)
                    selectedWeapon = weaponHolder.childCount - 1;
                else
                    selectedWeapon--;
            }

            if (Input.anyKeyDown)
            {
                for (int i = 1; i <= weaponHolder.childCount; i++)
                {
                    if (Input.inputString == i.ToString())
                    {
                        selectedWeapon = i - 2;     // i - 2 since character has no weapon mode
                    }
                }
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                // test without RPC
                PV.RPC("RPC_SelectWeapon", RpcTarget.All);
                //RPC_SelectWeapon();
                nextSelectionTime = Time.time + 1f / attackSelectionRate;
            }
        }

    }

    void FindWeaponHolder(Transform point)
    {
        foreach (Transform t in point)
        {
            if (t.tag == "AttackPoint") // tag should have named differently (weaponHolder)
            {
                weaponHolder = t;
                FindWeaponHolder(t);
            }
        }
    }

    [PunRPC]
    public void RPC_Hit()
    {
        // Detect enemy
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Damage enemy
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Character" && !enemy.gameObject.GetComponent<PhotonView>().IsMine)    // 2nd condition prevents player from hitting itself
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
            else
            {
                foreach (Collider soloEnemy in hitEnemies)
                {
                    EnemyCombat enemyState = enemy.gameObject.GetComponent<EnemyCombat>();

                    if (enemyState != null)
                    {
                        enemyState.WasHit();
                        Debug.Log("Set new enemy health");
                        Debug.Log($"Enemy's health is now: {enemyState.enemyHealth}");
                    }
                }
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        if (weaponHolder == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    [PunRPC]
    //To activate or deactivate the weapon
    public void RPC_SelectWeapon()
    {
        if (selectedWeapon == -1)
            currentWeapon = "Punch";

        int i = 0;
        foreach (Transform weapon in weaponHolder)
        {
            Debug.Log(weapon.name);

            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                currentWeapon = weapon.gameObject.name;
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    [PunRPC]
    public void RPC_Shooting()
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
            else if (hit.transform.tag == "Enemy")
            {
                Debug.Log($"Player {PV.ViewID} shot {hit.transform.gameObject.GetComponent<PhotonView>().ViewID}");
                EnemyCombat enemyState = hit.transform.gameObject.GetComponent<EnemyCombat>();

                if (enemyState != null)
                {
                    enemyState.WasHit();
                    Debug.Log("Set new health");
                    Debug.Log($"Enemy's local health is now: {enemyState.enemyHealth}");
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

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(selectedWeapon);
            Debug.Log("I am the local client: " + GetComponent<PhotonView>().ViewID + ". Current weapon is " + selectedWeapon);
        }
        else
        {
            selectedWeapon = (int)stream.ReceiveNext();
            Debug.Log("I am the remote client: " + GetComponent<PhotonView>().ViewID + ". Current weapon is " + selectedWeapon);
        }
    }
    
}
