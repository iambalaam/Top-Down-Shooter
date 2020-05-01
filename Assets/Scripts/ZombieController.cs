using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieController : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (navMeshAgent)
        {
            navMeshAgent.SetDestination(playerTransform.position);
        }        
    }
}
