using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    //Design Parameters
    [SerializeField]
    private float maxChaseRange;
    [SerializeField]
    private float chaseDistance;
    [SerializeField]
    private float talkDistance;
    [SerializeField]
    private float generateEnemyChances;
    [SerializeField]
    private float speakCD;
    [SerializeField]
    private string wanderMode;
    [SerializeField]
    private float resetRotationSpeed;

    //Parameters for circle pattern
    [SerializeField]
    private Vector3 centerOffset;
    [SerializeField]
    private float rotationStep;
    [SerializeField]
    private float circleRotationRadius;
    private float angle;

    //Parameters for linear pattern
    [SerializeField]
    private Vector3[] waypoints;
    private int currentWaypointIndex;

    //Bad is 0, Good is 1
    [HideInInspector]
    public int triangleType;
    private int damageDealt;
    private enum agentStates { idle, chase, backoff, backToStartPosition};
    private agentStates agentState;
    private NavMeshAgent agent;
    private GameObject player;
    private float distanceToPlayer;
    private float distanceStartToPlayer;
    private float lastSpeakTime;
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
        currentWaypointIndex = 0;
        if (Random.Range(0.0f, 1.0f) <= generateEnemyChances)
        {
            triangleType = 0;
            damageDealt = 10;
        }
        else
        {
            triangleType = 1;
            damageDealt = -10;
        }
        agentState = agentStates.idle;
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        lastSpeakTime = Time.time;
        agentState = agentStates.idle;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        distanceStartToPlayer = Vector3.Distance(startPosition, player.transform.position);
        if (distanceToPlayer <= chaseDistance && distanceToPlayer > talkDistance && distanceStartToPlayer <= maxChaseRange)
        {
            if (player.GetComponent<PlayerController>().isMuted)
            {
                if (agentState == agentStates.chase)
                {
                    agentState = agentStates.backoff;
                    agent.SetDestination(transform.position + (transform.position - player.transform.position).normalized * 2);
                }
            }
            else if (agentState != agentStates.backoff)
            {
                agentState = agentStates.chase;
                agent.SetDestination(player.transform.position);
            }
        }
        else if (distanceToPlayer <= talkDistance && distanceStartToPlayer <= maxChaseRange)
        {
            if (player.GetComponent<PlayerController>().isMuted)
            {
                if (agentState == agentStates.chase)
                {
                    agentState = agentStates.backoff;
                    agent.SetDestination(transform.position + (transform.position - player.transform.position).normalized * 2);
                }
            }
            else if (agentState != agentStates.backoff)
            {
                agentState = agentStates.chase;
                agent.SetDestination(transform.position);
                transform.LookAt(player.transform.position);
            }

            if (Time.time - lastSpeakTime >= speakCD)
            {
                lastSpeakTime = Time.time;
                if(!player.GetComponent<PlayerController>().isMuted)
                    player.GetComponent<PlayerController>().takeDamage(damageDealt);
                transform.GetChild(0).gameObject.GetComponent<EnemySounds>().playVoice(triangleType);
            }
        }
        else
        {
            if (agentState == agentStates.chase)
                agentState = agentStates.backToStartPosition;
        }


        //AI movement pattern
        if (agentState == agentStates.idle)
        {
            switch (wanderMode)
            {
                case "Circle":
                    angle += rotationStep * Time.deltaTime;
                    Vector3 offset = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)) * circleRotationRadius;
                    agent.SetDestination(startPosition + centerOffset + offset);
                    break;


                case "Linear":
                    if (agent.remainingDistance <= 0.01f)
                        agent.SetDestination(waypoints[currentWaypointIndex++]);
                    if (currentWaypointIndex >= waypoints.Length)
                        currentWaypointIndex = 0;
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

    public void repair()
    {
        if(triangleType == 0)
        {
            triangleType = 1;
            damageDealt = -10;
        }
    }

    public bool isChasing()
    {
        return agentState == agentStates.chase;
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
