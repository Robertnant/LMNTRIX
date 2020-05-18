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

        if (Vector3.Distance(transform.position, objPos) >= minDist)
        {
            agent.SetDestination(objPos);
            animator.SetBool("isFar", true);
        }
        else
        {
            animator.SetBool("isFar", false);
            agent.SetDestination(transform.position);
        }

        /*
         * Use when there'll be an action such as shooting to do
         * if distance <= max
        if (Vector3.Distance(transform.position, objPos) <= maxDist)
        {
            // Do something
        }
        */
    }
    private GameObject FindClosestEnemy(GameObject[] playersList)
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
}