using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abducter : MonoBehaviour
{  
    public Transform endMarker;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, endMarker.position);
    }
    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fracJourney = distCovered / journeyLength;
        transform.position = Vector3.Lerp(transform.position, endMarker.position, fracJourney);
    }

}
