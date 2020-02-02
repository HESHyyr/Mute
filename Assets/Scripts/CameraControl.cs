using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Range(0.01f, 20.0f)]
    public float ZoomLevel = 8.0f;
    
    public GameObject player;
    private Vector3 offset;

    public SpriteRenderer Dimmer;

    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Dimmer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset = new Vector3(0, ZoomLevel, -1 * ZoomLevel);
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(player.transform);

        if(player.GetComponent<PlayerController>().isMuted)
        {
            Dimmer.color = new Color(Dimmer.color.r, Dimmer.color.g, Dimmer.color.b, 0.5f);
        }
        else { Dimmer.color = new Color(Dimmer.color.r, Dimmer.color.g, Dimmer.color.b, 0); }
    }
}
