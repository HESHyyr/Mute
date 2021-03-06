﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneManager : MonoBehaviour
{
    //Design parameters
    [SerializeField]
    private float checkPlayerRange;
    [SerializeField]
    private float checkGoodTriangleRange;

    //Reference
    [SerializeField]
    private GameObject zoneAgentsGroup;
    public GameObject zoneGoodTriangle;
    private GameObject player;
    private PlayerController playerController;

    private ParticleSystem baseParticles;
    private ParticleSystem burstParticles;

    public bool isActive = true;
    private bool wasActive = true;

    [HideInInspector]
    public bool activated;

    private AudioSource BurstSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
        activated = false;

        baseParticles = transform.Find("Particles").GetComponent<ParticleSystem>();
        burstParticles = transform.Find("Burst").GetComponent<ParticleSystem>();

        BurstSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= checkPlayerRange && (playerController.currentGoal == null || Vector3.Distance(playerController.currentGoal.transform.position, player.transform.position) > distanceToPlayer))
            playerController.currentGoal = gameObject;

        var emission = baseParticles.emission;
        emission.enabled = isActive;

        if (!isActive && wasActive)
        {
            wasActive = false;
            burstParticles.Emit(200);
            BurstSound.Play();
        }

        if(Vector3.Distance(transform.position, zoneGoodTriangle.transform.position) <= checkGoodTriangleRange)
        {
            if(player.GetComponent<PlayerController>().hasGoodTriangle && isActive)
            {
                isActive = false;
                zoneCleared();
                playerController.activateOneShrine();
                GetComponent<AudioSource>().Play();
            }
        }

    }

    public void zoneCleared()
    {
        foreach(Transform agent in zoneAgentsGroup.transform)
            agent.gameObject.GetComponent<AIController>().repair();
        activated = true;
    }
}