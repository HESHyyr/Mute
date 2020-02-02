using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoneManager : MonoBehaviour
{
    //Reference
    [SerializeField]
    private List<GameObject> zoneAgents;
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void zoneCleared()
    {
        foreach(GameObject agent in zoneAgents)
            agent.GetComponent<AIController>().repair();
    }
}
