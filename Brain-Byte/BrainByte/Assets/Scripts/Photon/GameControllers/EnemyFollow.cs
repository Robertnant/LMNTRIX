using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent agent;
    public static GameObject[] trackableObjs;
    public static GameObject objToFollow;
    private Animator animator;
    public double minDist = 0.2;
    public bool isMultiplayer = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        trackableObjs = GameObject.FindGameObjectsWithTag("Character");
        objToFollow = FindClosestEnemy(trackableObjs);

        Vector3 objPos = objToFollow.GetComponent<Transform>().position;
        float distance = Vector3.Distance(transform.position, objPos);

        if (Vector3.Distance(transform.position, objPos) >= minDist)    // player is far
        {
            agent.SetDestination(objPos);
            animator.SetBool("isFar", true);
        }
        else
        {
            EnemyCombat.State currentState = GetComponent<EnemyCombat>().state;

            // Distance for shooting
            if (distance >= 5f)
            {
                // Switch to Correct weapon
                currentState = EnemyCombat.State.Shooting;
                animator.SetTrigger("Shoot");
            }

            // Distance for Punching
            if (distance < 5f && distance > 2f)
            {
                // Switch to Correct weapon
                currentState = EnemyCombat.State.Punching;
                animator.SetTrigger("Punch");
            }

            // Distance for Scratching
            if (distance <= 2f)
            {
                // Switch to Correct weapon
                currentState = EnemyCombat.State.Scratching;
                animator.SetTrigger("Scratch");
            }

            animator.SetBool("isFar", false);
            agent.SetDestination(transform.position);
        }

    }

    private GameObject FindClosestEnemy(GameObject[] playersList)
    {
        if (isMultiplayer)
        {
            GameObject minDistPlayer = null;

            foreach (GameObject player in playersList)
            {
                if (minDistPlayer == null && !player.GetComponent<HeadsUpDisplay>().isDead)
                    minDistPlayer = player;
                else
                {
                    float curDist = Vector3.Distance(transform.position, player.transform.position);

                    if (!player.GetComponent<HeadsUpDisplay>().isDead)
                    {
                        if (curDist < Vector3.Distance(transform.position, minDistPlayer.transform.position))
                            minDistPlayer = player;
                    }
                }
            }

            return minDistPlayer;
        }

        // Else
        return playersList[0];
    }
}