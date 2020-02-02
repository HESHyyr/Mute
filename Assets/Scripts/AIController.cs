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
    [SerializeField]
    private string wanderMode;
    [SerializeField]
    private float resetRotationSpeed;

    //Reference
    [SerializeField]
    private AudioClip goodSound;
    [SerializeField]
    private AudioClip badSound;

    //Bad is 0, Good is 1
    [HideInInspector]
    public int triangleType;
    private int damageDealt;
    private enum agentStates { idle, chase, backoff, backToStartPosition};
    private agentStates agentState;
    private NavMeshAgent agent;
    private GameObject player;
    private float distanceToPlayer;
    private AudioSource agentAudioSource;
    private float lastFireTime;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private Quaternion previousRotation;
    private bool isResetingRotation;
    private float slerpTimer;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        isResetingRotation = false;
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
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= chaseDistance && distanceToPlayer > talkDistance)
        {
            if (player.GetComponent<PlayerController>().isMuted)
            {
                Debug.Log(agentState);
                if(agentState == agentStates.chase)
                {
                    agentState = agentStates.backoff;
                    agent.SetDestination(transform.position + (transform.position - player.transform.position).normalized * 2);
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
                    agent.SetDestination(transform.position + (transform.position - player.transform.position).normalized * 2);
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

        //AI movement pattern
        if (agentState == agentStates.idle)
        {
            switch (wanderMode)
            {
                case "Circle":
                    agent.SetDestination(transform.position + new Vector3(1.0f,0.0f,1.0f));
                    break;

                case "Static":
                    break;

                default:
                    break;
            }
        }

        if (agent.remainingDistance <= 0.01f)
        {
            switch (agentState)
            {
                case agentStates.backoff:
                    agentState = agentStates.backToStartPosition;
                    agent.SetDestination(startPosition);
                    break;

                case agentStates.backToStartPosition:
                    StartCoroutine(WaitForResetRotation());
                    break;

                default:
                    break;
                    
            }
        }

        if (isResetingRotation)
        {
            transform.rotation = Quaternion.Slerp(previousRotation, startRotation, slerpTimer);
            slerpTimer += resetRotationSpeed;
        }
    }

    IEnumerator WaitForResetRotation()
    {
        previousRotation = transform.rotation;
        isResetingRotation = true;
        slerpTimer = 0.0f;
        yield return transform.rotation.Equals(startRotation);
        isResetingRotation = false;
        agentState = agentStates.idle;
    }

}
