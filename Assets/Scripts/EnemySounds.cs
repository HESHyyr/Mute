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

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Voice = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (WaitTimer <= 0.0f)
        {
            WaitTimer = WaitTime;
            RandomLine = Random.Range(0, Voices.Length);
            Voice.clip = Voices[RandomLine];

            if (player.GetComponent<PlayerController>().isMuted)
            {
                Voice.volume = MuteVolume;
            }
            else
            {
                Voice.volume = VolumeControl;
            }
            
            Voice.Play();
        }
        else
        {
            WaitTimer -= 1 * Time.deltaTime;
        }
    }
}
