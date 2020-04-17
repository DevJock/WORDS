using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShow : MonoBehaviour
{

    public float colorRate;
    float t;
    Renderer[] renderers;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        t = colorRate;
        NewColor();
    }


    private void Update()
    {
        if(t <= 0)
        {
            t = colorRate;
            NewColor();
        }
        t -= Time.deltaTime;
    }

    private void NewColor()
    {
        float randR = Random.Range(51, 255);
        float randG = Random.Range(51, 255);
        float randB = Random.Range(51, 255);
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = new Color(randR / 255.0f, randG / 255.0f, randB / 255.0f);
        }
    }


}
