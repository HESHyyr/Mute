using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [Range(0.01f, 20.0f)]
    public float ZoomLevel = 8.0f;
    
    private GameObject player;
    private Vector3 offset;

    private SpriteRenderer Dimmer;
    private SpriteRenderer Bright;

    public bool FadingIn = false;
    public bool FadingOut = false;
    public bool FadingBright = false;

    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.5f;

    public float fadeSpeed = .1f;
    private float opacity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        Dimmer = GameObject.Find("Dim").GetComponent<SpriteRenderer>();
        Bright = GameObject.Find("Bright").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offset = new Vector3(0, ZoomLevel, -1 * ZoomLevel);
        Vector3 desiredPosition = player.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(player.transform);

        /*
        if (Input.GetKeyDown("8"))
        {
            FadingIn = true;
            opacity = 1.0f;
        }
        if (Input.GetKeyDown("9"))
        {
            FadingOut = true;
            opacity = 0.0f;
        }
        if (Input.GetKeyDown("0"))
        {
            FadingBright = true;
            opacity = 0.0f;
        }


        if (FadingIn)
        {
            Dimmer.color = new Color(Dimmer.color.r, Dimmer.color.g, Dimmer.color.b, opacity);

            opacity = Mathf.Lerp(Dimmer.color.a, 0, fadeSpeed);
            
            if (opacity <= 0.1) { FadingIn = false; }
        }
        if (FadingOut)
        {
            Dimmer.color = new Color(Dimmer.color.r, Dimmer.color.g, Dimmer.color.b, opacity);

            opacity = Mathf.Lerp(Dimmer.color.a, 1, fadeSpeed);

            if (opacity >= 0.9) { FadingOut = false; }
        }
        if (FadingBright)
        {
            Bright.color = new Color(Bright.color.r, Bright.color.g, Bright.color.b, opacity);

            opacity = Mathf.Lerp(Bright.color.a, 1, fadeSpeed);

            if (opacity >= 0.9) { FadingBright = false; }
        }
        */
    }
}
