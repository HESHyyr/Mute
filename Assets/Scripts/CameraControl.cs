using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public Transform ThePlayer;
    public Vector3 offset;

    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        ThePlayer = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 desiredPosition = ThePlayer.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(ThePlayer);
    }
}
