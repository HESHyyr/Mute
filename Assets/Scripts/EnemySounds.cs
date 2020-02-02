using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{

    public AudioSource[] Voices;

    public int RandomLine;
    public float WaitTime = 4.0f;
    public float WaitTimer = 0.0f;

    [Range(0.0f, 1.0f)]
    public float VolumeControl = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Voices = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WaitTimer <= 0.0f)
        {
            WaitTimer = WaitTime;
            RandomLine = Random.Range(0, Voices.Length);

            Voices[RandomLine].volume = VolumeControl;
            Voices[RandomLine].Play();
        }
        else
        {
            WaitTimer -= 1 * Time.deltaTime;
        }
    }
}
