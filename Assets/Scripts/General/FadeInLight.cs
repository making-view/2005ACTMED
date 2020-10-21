using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent((typeof(Light)))]
public class FadeInLight : MonoBehaviour
{
    private Light light = null;
    private float originalIntensity = 0;
    private float originalRange = 0;
    private float progress = 0;
    private float pulseRange = 0;
    [SerializeField] private bool pulse = false;
    [SerializeField] float timer = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        originalIntensity = light.intensity;
        originalRange = light.range;
        light.intensity = 0;
    }

    private void Update()
    {
        if (pulse)
        {
            pulseRange = (pulseRange + Time.deltaTime) % (Mathf.PI);
            light.range = Mathf.SmoothStep(originalRange/3, originalRange, Mathf.Sin(pulseRange));
        }
    }

    public void FadeOut()
    {
        StopExistingFade();
        StartCoroutine(AsyncFadeOut());
    }

    public void FadeIn()
    {
        StopExistingFade();
        StartCoroutine(AsyncFadeIn());
    }

    private void StopExistingFade()
    {
        StopCoroutine(AsyncFadeIn());
        StopCoroutine(AsyncFadeOut());
    }

    private IEnumerator AsyncFadeIn()
    {
        while (progress < timer)
        {
            progress += Time.deltaTime;
            light.intensity = Mathf.SmoothStep(0, originalIntensity, progress);
            yield return new WaitForEndOfFrame();
        }
        light.intensity = originalIntensity;
    }


    private IEnumerator AsyncFadeOut()
    {
        while (progress > timer)
        {
            progress -= Time.deltaTime;
            light.intensity = Mathf.SmoothStep(0, originalIntensity, progress);
            yield return new WaitForEndOfFrame();
        }
        light.intensity = 0;
    }
}
