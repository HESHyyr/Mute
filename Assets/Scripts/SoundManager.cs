using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource[] Soundtracks;

    public AudioSource MuteMode;
    public bool Muted = false;
    [Range(0.0f, 1.0f)]
    public float MuteVolume = 0.1f;


    [Range(0.0f, 1.0f)]
    public float VolumeControl = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Soundtracks = GetComponentsInChildren<AudioSource>();
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


        if (Input.GetKey("8"))
        {
            Muted = true;
        } else { Muted = false; }

        if (Muted)
        {
            MuteMode.volume = 1.0f;
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
        }      
    }
}
