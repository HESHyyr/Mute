using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    //Design Parameters
    public int playerHealth;
    [SerializeField]
    private float movementForce;
    [SerializeField]
    private float maxMoveSpeed;
    [SerializeField]
    private float muteStartDamageCD;
    [SerializeField]
    private float moveSoundDamper;
    private AudioSource moveSound;
    [SerializeField]
    private float frictionSpeed;

    public float dyingHP = 50;
    [Range(0.0f, 0.1f)]
    public float dyingVol;
    private AudioSource dyingSound;
    private AudioSource deathSound;

    private bool stabalized = true;

    //Reference
    private Rigidbody rb;

    [HideInInspector]
    public bool isMuted;
    [HideInInspector]
    public GameObject currentGoal;
    public UnityEvent playerDie;
    public UnityEvent playerWin;
    private int goalReached;
    private float lastMuteTime;
    private bool hasGoodTriangle;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isMuted = false;
        hasGoodTriangle = false;
        moveSound = this.GetComponent<AudioSource>();
        dyingSound = transform.Find("Dying").gameObject.GetComponent<AudioSource>();
        deathSound = transform.Find("Dead").gameObject.GetComponent<AudioSource>();

        goalReached = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude <= maxMoveSpeed)
            rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * movementForce, 0, Input.GetAxis("Vertical") * Time.deltaTime * movementForce));



        moveSound.volume = moveSoundDamper * rb.velocity.magnitude;
        moveSound.pitch = 1 + (moveSoundDamper * rb.velocity.magnitude);


        if (Input.GetKey("space"))
        {
            isMuted = true;
            moveSound.volume = 0.1f * moveSoundDamper * rb.velocity.magnitude;
        } 
        else {
            isMuted = false;
            lastMuteTime = Time.time;
        }

        if (playerHealth <= dyingHP)
        {
            if (stabalized)
            {
                stabalized = false;
                dyingSound.Play();
            }
            dyingSound.volume = 0.1f * (100 - playerHealth) * dyingVol;
        } else
        {
            stabalized = true;
            dyingSound.volume = 0.0f;
        }

        if (isMuted && Time.time - lastMuteTime >= muteStartDamageCD)
            takeDamage(1);

        if (goalReached == 3)
            playerWin.Invoke();

        if (currentGoal != null && currentGoal.GetComponent<zoneManager>().zoneGoodTriangle.GetComponent<AIController>().isChasing())
            hasGoodTriangle = true;
        else
            hasGoodTriangle = false;
    }

    public void takeDamage(int number)
    {
        playerHealth -= number;
        if (playerHealth > 100)
            playerHealth = 100;
        if (playerHealth <= 0)
        {
            playerDie.Invoke();
            dyingSound.Stop();
            deathSound.Play();
        }          
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject == currentGoal && !currentGoal.GetComponent<zoneManager>().activated && hasGoodTriangle)
        {
            currentGoal.SetActive(false);
            goalReached++;
        }
    }
}
