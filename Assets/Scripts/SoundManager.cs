using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource BaseMusic;
    private AudioSource Intro;

    [Range(0.01f, 1.0f)]
    public float MuteStaticVolume;
    private AudioSource MuteStart;
    private AudioSource MuteStop;
    private AudioSource MuteMode;
    [Range(0.01f, 1.0f)]
    public float MuteVolume = 0.1f;
    private bool wasMuted;

    private GameObject player;

    [Range(0.0f, 1.0f)]
    public float VolumeControl = 0.5f;

    [HideInInspector]
    public float MusicVol;
    [HideInInspector]
    public float AmbVol;
    [Range(-0.5f, 0.5f)]
    public float AmbDamp = -0.1f;
    [HideInInspector]
    public float VoiceVol;
    [Range(-0.5f, 0.5f)]
    public float VoiceDamp = -0.1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");

        BaseMusic = GetComponent<AudioSource>();
        Intro = GameObject.Find("IntroSound").GetComponent<AudioSource>();
        Intro.volume = VolumeControl;
        Intro.Play();

        MuteMode = GameObject.Find("MuteLoop").GetComponent<AudioSource>();
        MuteStart = GameObject.Find("MuteStart").GetComponent<AudioSource>();
        MuteStop = GameObject.Find("MuteStop").GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().isMuted)
        {
            BaseMusic.volume = MuteVolume;
            MusicVol = MuteVolume;
            AmbVol = MuteVolume + AmbDamp;
            VoiceVol = MuteVolume + VoiceDamp;

            if (!wasMuted)
            {
                MuteStart.volume = MuteStaticVolume;
                MuteStart.Play();
                wasMuted = true;
            }

            MuteMode.volume = MuteStaticVolume;
        } else
        {
            BaseMusic.volume = VolumeControl;
            MusicVol = VolumeControl;
            AmbVol = VolumeControl + AmbDamp;
            VoiceVol = VolumeControl + VoiceDamp;

            if (wasMuted)
            {
                MuteStop.volume = MuteStaticVolume;
                MuteStop.Play();
                wasMuted = false;
            }

            MuteMode.volume = 0.0f;
        }      
    }
}
