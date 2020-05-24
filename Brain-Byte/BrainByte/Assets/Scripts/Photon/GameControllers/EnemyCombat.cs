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
    float enemyMaxHealth = 85f;
    float ShootDamage = 10f;
    float PunchDamage = 6f;
    float ScratchDamage = 20f;

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
        PV = GetComponent<PhotonView>();
        rayOrigin = transform;
        animator = GetComponent<Animator>();

        // set enemy default weapon
        PV.RPC("RPC_SelectWeapon", RpcTarget.All);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
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
                PV.RPC("RPC_SelectWeapon", RpcTarget.All);
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
}