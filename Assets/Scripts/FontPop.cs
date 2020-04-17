using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FontPop : MonoBehaviour {

    public float minSize;
    public float maxSize;
    public float growRate;
    public float time;

    TextMesh text;
	void Start ()
    {
        time = 0.1f - 0.1f * growRate;
        text = gameObject.GetComponent<TextMesh>();	
	}

    // Update is called once per frame
    void Update()
    {
        if (text.fontSize <= maxSize)
        {
            if (time <= 0)
            {
                time = 0.1f - 0.1f * growRate;
                text.fontSize += 1;
            }
            time -= Time.deltaTime;
        }
    }
}
