using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(AudioSource))]
public class BubbleEvent : MonoBehaviour
{
    [SerializeField] private float fogStart = 1.0f;
    [SerializeField] private float fogEnd = 1.0f;

    private float previousFogStart = 1.0f;
    private float previousFogEnd = 1.0f;

    private float fade = 0;
    [SerializeField] private float timeToFade = 1.0f;
    [SerializeField] private CanvasGroup imAfraudlol = null;
    AudioSource audio = null;

    [SerializeField] GameObject colliders = null;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        previousFogStart = RenderSettings.fogStartDistance;
        previousFogEnd = RenderSettings.fogEndDistance;

        foreach (Collider c in colliders.GetComponentsInChildren<Collider>())
            c.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            StartCoroutine(FogOff());
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name + "entered");
        if (other.gameObject.tag.ToLower().Equals("player"))
        {
            //Debug.Log("startFade");
            StartCoroutine(FogUp());
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void RemoveFog()
    {
        StartCoroutine(FogOff());
    }

    private IEnumerator FogUp()
    {
        StopCoroutine(FogUp());
        StopCoroutine(FogOff());


        foreach (Collider c in colliders.GetComponentsInChildren<Collider>())
            c.enabled = true;

        while (fade < timeToFade)
        {
            fade += Time.deltaTime;
            RenderSettings.fogStartDistance = Mathf.SmoothStep(previousFogStart, fogStart, fade / timeToFade);
            RenderSettings.fogEndDistance = Mathf.SmoothStep(previousFogEnd, fogEnd, fade / timeToFade);

            var bigSmart = Mathf.Sin(Mathf.SmoothStep(0, Mathf.PI / 2, fade / timeToFade));
            audio.volume = bigSmart * 0.6f;

            if (imAfraudlol != null)
                imAfraudlol.alpha = bigSmart;

            yield return null;
        }

        GetComponentInParent<PopupAnalogy>().StartPopupshit();
    }

    private IEnumerator FogOff()
    {
        StopCoroutine(FogUp());
        StopCoroutine(FogOff());


        foreach (Collider c in colliders.GetComponentsInChildren<Collider>())
            c.enabled = false;

        while (fade > 0)
        {
            fade -= Time.deltaTime;
            RenderSettings.fogStartDistance = Mathf.SmoothStep(previousFogStart, fogStart, fade / timeToFade);
            RenderSettings.fogEndDistance = Mathf.SmoothStep(previousFogEnd, fogEnd, fade / timeToFade);

            var bigSmart = Mathf.Sin(Mathf.SmoothStep(0, Mathf.PI / 2, fade / timeToFade));
            audio.volume = bigSmart * 0.6f;

            if (imAfraudlol != null)
                imAfraudlol.alpha = bigSmart;

            yield return null;
        }
    }
}
