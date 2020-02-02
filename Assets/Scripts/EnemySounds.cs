using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{

    public AudioClip[] Voices;
    public AudioSource Voice;

    public int RandomLine;
    public float WaitTime = 4.0f;
    public float WaitTimer = 0.0f;

    [Range(0.0f, 1.0f)]
    public float VolumeControl = 0.5f;
    [Range(0.0f, 1.0f)]
    public float MuteVolume = 0.1f;

    private SoundManager MasterVol;

    // Start is called before the first frame update
    void Start()
    {
        Voice = GetComponent<AudioSource>();

        MasterVol = GameObject.Find("Audio Manager").GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WaitTimer <= 0.0f)
        {
            WaitTimer = WaitTime;
            RandomLine = Random.Range(0, Voices.Length);
            Voice.clip = Voices[RandomLine];

            Voice.volume = MasterVol.VoiceVol;
            
            Voice.Play();
        }
        else
        {
            WaitTimer -= 1 * Time.deltaTime;
        }
    }
}
