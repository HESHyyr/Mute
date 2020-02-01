using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    //Design Parameters
    [SerializeField]
    private float chaseDistance;
    [SerializeField]
    private float talkDistance;
    [SerializeField]
    private float generateEnemyChances;
    [SerializeField]
    private float fireCD;

    //Reference
    [SerializeField]
    private AudioClip goodSound;
    [SerializeField]
    private AudioClip badSound;

    //Bad is 0, Good is 1
    [HideInInspector]
    public int triangleType;
    private int damageDealt;
    private enum agentStates { idle, chase, backoff};
    private agentStates agentState;
    private NavMeshAgent agent;
    private GameObject player;
    private float distanceToPlayer;
    private AudioSource agentAudioSource;
    private float lastFireTime;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        agentAudioSource = GetComponent<AudioSource>();
        if (Random.Range(0.0f, 1.0f) <= generateEnemyChances)
        {
            triangleType = 0;
            agentAudioSource.clip = badSound;
            damageDealt = 10;
        }
        else
        {
            triangleType = 1;
            agentAudioSource.clip = goodSound;
            damageDealt = -10;
        }
        agentState = agentStates.idle;
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        lastFireTime = Time.time;
        agentState = agentStates.idle;
    }

    // Update is called once per frame
    void Update()
    {
        lastPosition = transform.position;
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= chaseDistance && distanceToPlayer > talkDistance)
        {
            if (player.GetComponent<PlayerController>().isMuted)
            {
                if(agentState == agentStates.chase)
                {
                    agentState = agentStates.backoff;
                    agent.SetDestination(transform.position + (transform.position - player.transform.position).normalized * (chaseDistance - distanceToPlayer));
                }
            }
            else if(agentState != agentStates.backoff)
            {
                agentState = agentStates.chase;
                agent.SetDestination(player.transform.position);
            }
        }
        else if (distanceToPlayer <= talkDistance)
        {
            if (player.GetComponent<PlayerController>().isMuted)
            {
                if(agentState == agentStates.chase)
                {
                    agentState = agentStates.backoff;
                    agent.SetDestination(transform.position + (transform.position - player.transform.position).normalized * (chaseDistance - distanceToPlayer));
                }
            }
            else if(agentState != agentStates.backoff)
            {
                agentState = agentStates.chase;
                agent.SetDestination(transform.position);
                if (Time.time - lastFireTime >= fireCD)
                {
                    player.GetComponent<PlayerController>().takeDamage(damageDealt);
                    agentAudioSource.Play();
                    lastFireTime = Time.time;
                }
            }
        }


        if (agent.remainingDistance <= 0.01f && agentState != agentStates.chase)
            agentState = agentStates.idle;

    }
    
}
