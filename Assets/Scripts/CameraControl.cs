using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Range(0.01f, 20.0f)]
    public float ZoomLevel = 8.0f;

    public Transform ThePlayer;
    private Vector3 offset;

    public SpriteRenderer Dimmer;

    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        ThePlayer = GameObject.Find("Player").transform;
        Dimmer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset = new Vector3(0, ZoomLevel, -1 * ZoomLevel);
        Vector3 desiredPosition = ThePlayer.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(ThePlayer);
    }
}
