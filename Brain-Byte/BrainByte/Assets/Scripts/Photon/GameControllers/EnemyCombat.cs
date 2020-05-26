using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyCombat : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Attributes

    private PhotonView PV;
    public Transform rayOrigin;
    public Animator animator;

    // for attack
    public Transform weaponHolder;
    public float attackRange = 0.6f;
    public float attackRate = 1.25f;
    private float nextAttackTime = 0f;
    public LayerMask enemyLayers;
    public Transform attackPoint;

    public int selectedWeapon = -1; // -1 for no weapon

    // Enemy Health and Damage
    public float enemyHealth = 85f;
    public float enemyDamage = 5f;
    public float ShootDamage = 10f;
    public float PunchDamage = 6f;
    public float ScratchDamage = 20f;

    private bool isMultiplayer;

    // attack state
    public enum State
    {
        Punching,
        Scratching,
        Shooting,
        Death,
        Roaming
    }

    public State state;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        isMultiplayer = MultiplayerSettings.multiplayerSettings.isMultiplayer;

        if (isMultiplayer)
            PV = GetComponent<PhotonView>();

        foreach (Transform t in transform)
        {
            if (t.tag == "HitPoint")
            {
                attackPoint = t;
            }
        }

        rayOrigin = transform;
        animator = GetComponent<Animator>();

        // set enemy default weapon
        if (isMultiplayer)
            PV.RPC("RPC_SelectWeapon", RpcTarget.All);
        else
            RPC_SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMultiplayer && !PV.IsMine)
            return;

        // attack
        if (Time.time >= nextAttackTime)
        {
            int previousSelectedWeapon = selectedWeapon;

            if (isMultiplayer)
            {
                switch (state)
                {
                    case State.Shooting:
                        selectedWeapon = 0;     // weapon
                        PV.RPC("RPC_Shooting", RpcTarget.All);
                        Debug.Log("Shooting!");
                        break;
                    case State.Scratching:
                        selectedWeapon = -2;    // no weapon
                        PV.RPC("RPC_Hit", RpcTarget.All);
                        Debug.Log("Hit!");
                        break;
                    case State.Punching:
                        selectedWeapon = -1;    // no weapon
                        PV.RPC("RPC_Hit", RpcTarget.All);
                        Debug.Log("Hit!");
                        break;

                }
            }
            else
            {
                switch (state)
                {
                    case State.Shooting:
                        selectedWeapon = 0;     // weapon
                        Shooting();
                        break;
                    case State.Scratching:
                        selectedWeapon = -2;    // no weapon
                        RPC_Hit();
                        break;
                    case State.Punching:
                        selectedWeapon = -1;    // no weapon
                        RPC_Hit();
                        break;

                }
            }

            if (previousSelectedWeapon != selectedWeapon)
            {
                if (isMultiplayer)
                    PV.RPC("RPC_SelectWeapon", RpcTarget.All);
                else
                    RPC_SelectWeapon();

                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    [PunRPC]
    //To activate or deactivate the weapon
    void RPC_SelectWeapon()
    {
        if (selectedWeapon == -1 || selectedWeapon == -2)
        {
            weaponHolder.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            weaponHolder.GetChild(0).gameObject.SetActive(true);
        }
    }

    #region EnemyDamage
    [PunRPC]
    public void WasHit()
    {
        if (enemyHealth <= 0)
            return;

        if (isMultiplayer)
            PV.RPC("TakeDamage", RpcTarget.All);
        else
            TakeDamage();
    }

    [PunRPC]
    void TakeDamage()
    {
        enemyHealth -= enemyHealth - 25 <= 0 ? enemyHealth : 25;

        // Die if necessary
        if (enemyHealth <= 0)
            Die();

        Debug.Log($"Enemy's remote health is now: {enemyHealth}");
    }

    void Die()
    {
        System.Random random = new System.Random();

        int deathAnim = random.Next(2);

        // randomaly play one of the two death animations
        if (deathAnim == 0)
            animator.SetTrigger("Dead1");
        else
            animator.SetTrigger("Dead2");

        if (isMultiplayer)
            Debug.Log($"Enemy {PV.ViewID} was killed");

        GetComponent<EnemyFollow>().enabled = false;
        this.enabled = false;

    }
    #endregion

    #region Attacks
    [PunRPC]
    void RPC_Shooting()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1000))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

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
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not hit anyone");
        }

    }

    void Shooting()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1000))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);

            if (hit.transform.tag == "Character")
            {
                SoloHeadsUpDisplay enemyHUD = hit.transform.gameObject.GetComponent<SoloHeadsUpDisplay>(); // to modify with new script

                if (enemyHUD != null)
                {
                    enemyHUD.WasHit();
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
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not hit anyone");
        }

    }

    [PunRPC]
    void RPC_Hit()
    {

        // Detect enemy
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Damage enemy
        foreach (Collider enemy in hitEnemies)
        {
            if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
            {
                Debug.Log($"Player {PV.ViewID} hit {enemy.gameObject.GetComponent<PhotonView>().ViewID}");

                // Deduct health
                HeadsUpDisplay enemyHUD = enemy.gameObject.GetComponent<HeadsUpDisplay>();

                if (enemyHUD != null)
                {
                    enemyHUD.WasHit();
                    Debug.Log($"Enemy's new health is now: {enemyHUD.playerHealth}");
                }
            }
            else
            {
                SoloHeadsUpDisplay enemyHUD = enemy.gameObject.GetComponent<SoloHeadsUpDisplay>();

                if (enemyHUD != null)
                {
                    enemyHUD.WasHit();
                    Debug.Log($"Enemy's new health is now: {enemyHUD.playerHealth}");
                }
            }

        }
    }
    #endregion

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(selectedWeapon);
                stream.SendNext(enemyHealth);
                Debug.Log("I am the local client: " + GetComponent<PhotonView>().ViewID);
            }
            else
            {
                selectedWeapon = (int)stream.ReceiveNext();
                enemyHealth = (float)stream.ReceiveNext();
                Debug.Log("I am the remote client: " + GetComponent<PhotonView>().ViewID);
            }
        }
    }
}