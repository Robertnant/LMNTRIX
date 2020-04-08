using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent agent;
    public static GameObject[] trackableObjs;
    public static GameObject objToFollow;

    void Start()
    {
        trackableObjs = GameObject.FindGameObjectsWithTag("Player");

        if (trackableObjs.Length != 0)
        {
            objToFollow = trackableObjs[0];
        }
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

        agent.SetDestination(objPos);
    }
}
