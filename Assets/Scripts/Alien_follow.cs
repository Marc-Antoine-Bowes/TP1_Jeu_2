using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Actor : MonoBehaviour
{
    [SerializeField] GameObject goal;
    private NavMeshAgent navMeshAgent;
    private Transform transformAlien;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        transformAlien = GetComponent<Transform>();
    }

    void Update()
    {

        if (navMeshAgent != null && canMove())
        {
            navMeshAgent.destination = goal.transform.position;
        }

    }

    private bool canMove()
    {
        if(transformAlien.position.y <= -72.7f)
        {
            navMeshAgent.enabled = true;
            return true;
            
        }
        else
        {
            return false;
        }
    }
}
