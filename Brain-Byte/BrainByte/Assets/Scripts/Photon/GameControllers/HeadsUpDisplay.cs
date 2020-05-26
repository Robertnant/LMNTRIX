using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class HeadsUpDisplay : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerHealth;
    public int maxHealth;
    public HealthBar healthBar;

    private PhotonView PV;
    private Animator animator;
    private V2PlayerMovement movement;
    private AvatarCombat combat;
    public bool valsSet = false;
    public bool isDead = false;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        movement = GetComponentInParent<V2PlayerMovement>();
        combat = GetComponentInParent<AvatarCombat>();
    }

    void FixedUpdate()
    {
        if (PV.IsMine)
        {
            healthBar.SetHealth(playerHealth);
        }
    }
    public void WasHit()
    {
        if (playerHealth <= 0)
            return;

        PV.RPC("TakeDamage", RpcTarget.All);
    }

    [PunRPC]
    void TakeDamage()
    {
        playerHealth -= playerHealth - 5 <= 0 ? playerHealth : 5;
        healthBar.SetHealth(playerHealth);

        // Die if necessary
        if (playerHealth <= 0)
            Die();

        Debug.Log($"Enemy's remote health is now: {playerHealth}");
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

        isDead = true;                // can be useful when checking if player is dead by other components
        movement.enabled = false;
        combat.enabled = false;

        Debug.Log($"Player {PV.ViewID} has died");
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (MultiplayerSettings.multiplayerSettings.isMultiplayer)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(playerHealth);
                Debug.Log("I am the local client: " + GetComponent<PhotonView>().ViewID);
                Debug.Log("My local health is " + playerHealth);
            }
            else
            {
                playerHealth = (int)stream.ReceiveNext();
                Debug.Log("I am the remote client: " + GetComponent<PhotonView>().ViewID);
                Debug.Log("My remote health is " + playerHealth);
            }
        }
    }

}
