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
    public double minDist = 10f;
    public bool isMultiplayer = true;
    private float distance;

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
        distance = Vector3.Distance(transform.position, objPos);

        Physics.OverlapSphere(transform.position, distance);

        if (Vector3.Distance(transform.position, objPos) >= minDist)    // player is far
        {
            agent.SetDestination(objPos);
            animator.SetBool("isFar", true);
        }
        else
        {
            EnemyCombat.State currentState = GetComponent<EnemyCombat>().state;

            //Debug.Log("Distance between enemy and player: " + distance);

            // Distance for shooting
            if (distance <= minDist && distance >= minDist - 3f)
            {
                // Switch to Correct weapon
                Debug.Log("Setting shooting weapon " + distance);
                currentState = EnemyCombat.State.Shooting;
                animator.SetTrigger("Shoot");
            }
            else if (distance > minDist - 5f)
            {
                // Switch to Correct weapon
                Debug.Log("Setting punching weapon" + distance);
                currentState = EnemyCombat.State.Punching;
                animator.SetTrigger("Punch");
            }
            else
            {
                // Switch to Correct weapon
                Debug.Log("Setting scratching weapon" + distance);
                currentState = EnemyCombat.State.Scratching;
                animator.SetTrigger("Scratch");
            }

            //animator.SetBool("isFar", false);
            //agent.SetDestination(transform.position);
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distance);
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