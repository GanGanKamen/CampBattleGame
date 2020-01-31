using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentTest : MonoBehaviour
{
    [SerializeField] Transform pos;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.destination = pos.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
