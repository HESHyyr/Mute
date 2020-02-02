using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource[] Soundtracks;

    [Range(0.01f, 1.0f)]
    public float MuteStaticVolume;
    public AudioSource MuteStart;
    public AudioSource MuteStop;
    public AudioSource MuteMode;
    [Range(0.01f, 1.0f)]
    public float MuteVolume = 0.1f;
    private bool wasMuted;

    private GameObject player;

    [Range(0.0f, 1.0f)]
    public float VolumeControl = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Soundtracks = GetComponentsInChildren<AudioSource>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {
            VolumeControl += .1f;
        }
        if (Input.GetKeyDown("9"))
        {
            VolumeControl -= .1f;
        }
        VolumeControl = Mathf.Clamp01(VolumeControl);


        if (player.GetComponent<PlayerController>().isMuted)
        {
            if (!wasMuted)
            {
                MuteStart.volume = MuteStaticVolume;
                MuteStart.Play();
                wasMuted = true;
            }

            MuteMode.volume = MuteStaticVolume;
            for (int i = 0; i < Soundtracks.Length; i++)
            {
                Soundtracks[i].volume = MuteVolume;
            }
        } else
        {
            MuteMode.volume = 0.0f;
            for (int i = 0; i < Soundtracks.Length; i++)
            {
                Soundtracks[i].volume = VolumeControl;
            }

            if (wasMuted)
            {
                MuteStop.volume = MuteStaticVolume;
                MuteStop.Play();
                wasMuted = false;
            }
        }      
    }
}
