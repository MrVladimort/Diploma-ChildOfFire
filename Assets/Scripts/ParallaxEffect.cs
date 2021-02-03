using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startpos;
    public GameObject camera;
    public float parallaxEffect;
    
    public 
    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = (camera.transform.position.x * (1 - parallaxEffect));
        float dist = (camera.transform.position.x * parallaxEffect);

        var position = transform.position;
        position = new Vector3(startpos + dist, position.y, position.z);
        transform.position = position;

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
