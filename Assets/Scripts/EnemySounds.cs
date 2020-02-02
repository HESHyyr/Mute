using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] goodVoices;
    public AudioClip[] badVoices;
    public AudioSource Voice;

    public int RandomLine;

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

    }

    public void playVoice(int triangleType)
    {
        if(triangleType == 0)
        {
            RandomLine = Random.Range(0, badVoices.Length);
            Voice.clip = badVoices[RandomLine];
        }
        else
        {
            RandomLine = Random.Range(0, goodVoices.Length);
            Voice.clip = goodVoices[RandomLine];
        }

        if (player.GetComponent<PlayerController>().isMuted)
        {
<<<<<<< HEAD
            WaitTimer = WaitTime;
            RandomLine = Random.Range(0, Voices.Length);
            Voice.clip = Voices[RandomLine];

            Voice.volume = MasterVol.VoiceVol;
            
            Voice.Play();
=======
            Voice.volume = MuteVolume;
>>>>>>> b63ec4d7419c853ec8e1df2b9aa562770ed5766a
        }
        else
        {
            Voice.volume = VolumeControl;
        }

        Voice.Play();
    }
}
