using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EnemyCombat : MonoBehaviour
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

            switch (state)
            {
                case State.Shooting:
                    selectedWeapon = 0;     // weapon
                    break;
                case State.Scratching:
                    selectedWeapon = -2;    // no weapon
                    break;
                case State.Punching:
                    selectedWeapon = -1;    // no weapon
                    break;

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
    [PunRPC]
    public void WasHit(int damage)
    {
        if (enemyHealth <= 0)
            return;

        if (isMultiplayer)
            PV.RPC("TakeDamage", RpcTarget.All);
        else
            TakeDamage(damage);
    }

    [PunRPC]
    void TakeDamage(int damage)
    {
        enemyHealth -= enemyHealth - damage <= 0 ? enemyHealth : damage;

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

        Debug.Log($"Enemy {PV.ViewID} was killed");

        GetComponent<EnemyFollow>().enabled = false;
        this.enabled = false;

    }
}