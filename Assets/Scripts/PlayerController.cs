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

    //Reference
    private Rigidbody rb;
    private AudioListener playerAudioListener;

    [HideInInspector]
    public bool isMuted;
    public UnityEvent playerDie;
    public UnityEvent playerWin;
    private int goalReached;
    private float lastMuteTime;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isMuted = false;
        playerAudioListener = GetComponent<AudioListener>();
        goalReached = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude <= maxMoveSpeed)
            rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * movementForce, 0, Input.GetAxis("Vertical") * Time.deltaTime * movementForce));

        if (Input.GetKeyDown("space"))
        {
            if (isMuted)
            {
                playerAudioListener.enabled = true;
            }
            else
            {
                playerAudioListener.enabled = false;
                lastMuteTime = Time.time;
            }
            isMuted = !isMuted;
        }

        if (isMuted && Time.time - lastMuteTime >= muteStartDamageCD)
            takeDamage(1);

        if (goalReached == 3)
            playerWin.Invoke();
    }

    public void takeDamage(int number)
    {
        playerHealth -= number;
        if (playerHealth > 100)
            playerHealth = 100;
        if (playerHealth <= 0)
            playerDie.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.CompareTag("Goal") && collision.other.gameObject.GetComponent<MeshRenderer>().material.color != Color.green)
        {
            collision.other.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
            goalReached++;
        }
    }
}
