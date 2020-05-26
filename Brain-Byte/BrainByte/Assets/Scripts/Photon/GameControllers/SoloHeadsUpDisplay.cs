using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class SoloHeadsUpDisplay : MonoBehaviour
{
    public int playerHealth = 100;
    public int maxHealth = 100;
    public HealthBar healthBar;

    private Animator animator;
    private SoloPlayerMovement movement;
    private SoloAvatarCombat combat;
    public bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<SoloPlayerMovement>();
        combat = GetComponent<SoloAvatarCombat>();

    }

    void FixedUpdate()
    {
        healthBar.SetHealth(playerHealth);
    }
    public void WasHit()
    {
        if (playerHealth <= 0)
            return;
        
        TakeDamage();
    }

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

        // randomly play one of the two death animations
        if (deathAnim == 0)
            animator.SetTrigger("Dead1");
        else
            animator.SetTrigger("Dead2");

        isDead = true;                // can be useful when checking if player is dead by other components
        movement.enabled = false;
        combat.enabled = false;
        FindObjectOfType<LevelLoader>().LoseLevel();
    }

}
