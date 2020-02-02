using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float rotateSpeed;
    private float newRotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        newRotate += rotateSpeed * Time.deltaTime;

        if (newRotate >= 360f) { newRotate -= 360f; }
        this.transform.Rotate(0, 0, newRotate, Space.Self);
    }
}
