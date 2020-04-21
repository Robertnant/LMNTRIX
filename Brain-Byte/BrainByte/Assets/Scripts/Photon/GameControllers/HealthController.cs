using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private AvatarSetup avatarSetup;
    private PhotonView PV;
    public int playerHealth;
    private HealthBar healthBar;

    void Start()
    {
        PV = GetComponent<PhotonView>();

        if (PV.IsMine)
        {
            if (avatarSetup == null && healthBar == null)
            {
                avatarSetup = GetComponent<AvatarSetup>();
                playerHealth = avatarSetup.playerHealth;
                healthBar = avatarSetup.healthBar;
                Debug.Log("Set player health and health bar");
            }
        }
        else
        {
            Debug.Log("Did nothing");
        }
    }
    public void WasHit(int combatDamage)
    {
        playerHealth -= combatDamage;
        healthBar.SetHealth(playerHealth);
        Debug.Log($"Player health: {playerHealth}");
    }
}
