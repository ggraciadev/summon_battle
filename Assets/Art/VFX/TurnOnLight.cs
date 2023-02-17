using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLight : MonoBehaviour
{
    Light Light;
    float maxIntensity;
    float lightStep = 1f;

    void Start()
    {
        Light = gameObject.GetComponent<Light>();
        maxIntensity = Light.intensity;
        Light.intensity = 0;
    }

    void Update()
    {
        if(Light.intensity < maxIntensity) Light.intensity += lightStep * Time.deltaTime;
    }
}
