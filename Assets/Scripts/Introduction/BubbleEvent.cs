using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        previousFogStart = RenderSettings.fogStartDistance;
        previousFogEnd = RenderSettings.fogEndDistance;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            StartCoroutine(FogOff());
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + "entered");
        if (other.gameObject.tag.ToLower().Equals("player"))
        {
            Debug.Log("startFade");
            StartCoroutine(FogUp());
        }
    }



    private IEnumerator FogUp()
    {
        StopCoroutine(FogUp());
        StopCoroutine(FogOff());

        while (fade < timeToFade)
        {
            fade += Time.deltaTime;
            RenderSettings.fogStartDistance = Mathf.SmoothStep(previousFogStart, fogStart, fade / timeToFade);
            RenderSettings.fogEndDistance = Mathf.SmoothStep(previousFogEnd, fogEnd, fade / timeToFade);

            var bigSmart = Mathf.Sin(Mathf.SmoothStep(0, Mathf.PI / 2, fade / timeToFade));
            audio.volume = bigSmart;

            if (imAfraudlol != null)
                imAfraudlol.alpha = bigSmart;

            yield return null;
        }
    }

    private IEnumerator FogOff()
    {
        StopCoroutine(FogUp());
        StopCoroutine(FogOff());

        while (fade > 0)
        {
            fade -= Time.deltaTime;
            RenderSettings.fogStartDistance = Mathf.SmoothStep(previousFogStart, fogStart, fade / timeToFade);
            RenderSettings.fogEndDistance = Mathf.SmoothStep(previousFogEnd, fogEnd, fade / timeToFade);

            var bigSmart = Mathf.Sin(Mathf.SmoothStep(0, Mathf.PI / 2, fade / timeToFade));
            audio.volume = bigSmart;

            if (imAfraudlol != null)
                imAfraudlol.alpha = bigSmart;

            yield return null;
        }
    }
}
