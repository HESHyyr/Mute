using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSounds : MonoBehaviour
{
    private AudioSource Music;
    private AudioSource Ambient;

    private SoundManager MasterVol;

    public float SoundRadius = 30;

    // Start is called before the first frame update
    void Start()
    {
        Music = transform.Find("Music").gameObject.GetComponent<AudioSource>();
        Ambient = transform.Find("Ambient").gameObject.GetComponent<AudioSource>();

        MasterVol = GameObject.Find("Audio Manager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Music.volume = MasterVol.MusicVol;
        Ambient.volume = MasterVol.AmbVol;

        Music.maxDistance = SoundRadius;
        Ambient.maxDistance = SoundRadius;
    }
}
