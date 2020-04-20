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
        trackableObjs = GameObject.FindGameObjectsWithTag("Player");

        if (trackableObjs.Length != 0)
        {
            objToFollow = trackableObjs[0];
        }

        // New
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (objToFollow == null)
        {
            trackableObjs = GameObject.FindGameObjectsWithTag("Player");
            objToFollow = trackableObjs[0];
        }

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
}
