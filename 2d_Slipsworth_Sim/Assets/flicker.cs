using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class flicker : MonoBehaviour
{
    public bool flicker_enabled = false; //override in inspector
    public float flicker_time_1 = 0.1f;
    public float flicker_time_2 = 0.1f;
    public float flicker_intensity = 0.1f;

    public Light2D instance;


    private void Start() {
        instance = GetComponent<Light2D>();
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker() {
        if (instance != null && instance.GetComponent<Light2D>() != null) {
            while (flicker_enabled == true) {
                yield return new WaitForSeconds(Random.Range(flicker_time_1, flicker_time_2));
                instance.intensity = instance.intensity - flicker_intensity;
                yield return new WaitForSeconds(Random.Range(flicker_time_1, flicker_time_2));
                instance.intensity = instance.intensity + flicker_intensity;
            }        
        }
    }
}
