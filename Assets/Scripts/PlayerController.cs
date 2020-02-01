using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int playerHealth;
    [SerializeField]
    private float movementForce;
    [SerializeField]
    private float maxMoveSpeed;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rb.velocity.magnitude <= maxMoveSpeed)
            rb.AddForce(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * movementForce, 0, Input.GetAxis("Vertical") * Time.deltaTime * movementForce));
    }

    public void takeDamage(int number)
    {
        playerHealth -= number;
    }
}
