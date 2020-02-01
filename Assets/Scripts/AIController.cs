using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private float chaseDistance;
    [SerializeField]
    private float talkDistance;

    //Bad is 0, Good is 1
    [HideInInspector]
    public int enemyType;
    private int damageDealt;
    //Enemy state, wandering is 1, chasing is 2
    private int agentState;
    private NavMeshAgent agent;
    private GameObject player;
    private float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        agentState = 1;
        player = GameObject.Find("Player");
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        if(distanceToPlayer <= chaseDistance && distanceToPlayer > talkDistance)
        {
            agentState = 2;
            agent.SetDestination(player.transform.position);
        }
        else if(distanceToPlayer <= talkDistance)
        {
            agentState = 2;
            agent.SetDestination(transform.position);
            player.GetComponent<PlayerController>().takeDamage(damageDealt);
        }
    }
    
}
