using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;





//This class plays sfx and handles particles and light sources for a volume of user area
public class EventVolume : MonoBehaviour
{
    [Serializable]
    private struct invokeable
    {
        public float delay;
        public UnityEvent ue;
    }

    float timer = 0.0f; 

    bool activated = false;
    bool done = false;

    [SerializeField]
    float volume = 0.8f;

    [SerializeField]
    AudioClip narration = null;
    private AudioSource nr_source = null;

    [SerializeField]
    AudioClip sfx = null;
    private AudioSource sfx_source = null;

    [SerializeField]
    Light raito;
    float intensity = 0;
    bool lightfade = false;

    [SerializeField]
    List<ParticleSystem> currentParticles;

    [SerializeField]
    List<ParticleSystem> nextParticles;

    [SerializeField]
    List<invokeable> invokeables;

    [SerializeField]
    string customTag = "";


    float shadertest = 0;

    // Start is called before the first frame update
    void Start()
    {

        if (raito != null)
        {
            intensity = raito.intensity;
            raito.intensity = 0;

            raito.enabled = false;
        }

        nr_source = gameObject.AddComponent<AudioSource>();
        nr_source.volume = volume;
        nr_source.spatialize = false;
        sfx_source = gameObject.AddComponent<AudioSource>();
        sfx_source.volume = volume;
        sfx_source.spatialize = false;

        if (sfx != null)
            sfx_source.clip = sfx;

        if (narration != null)
            nr_source.clip = narration;

        if (nextParticles != null)
            foreach (ParticleSystem p in nextParticles)
            {
                p.Stop();
            }
    }

    // Update is called once per frame
    void Update()
    {    //when fading in

        Shader.SetGlobalFloat("NormalSwitch", Mathf.Clamp(Mathf.Sin(timer), 0, 1.0f));
        Shader.SetGlobalFloat("ColourSwitch", Mathf.Clamp(Mathf.Sin(timer), 0, 1.0f));

        Debug.Log("Switch: " + Mathf.Clamp(Mathf.Sin(timer), 0, 1.0f));

        if(activated)
            timer += Time.deltaTime;

        HandleLight();

        //fade back out when narration ended
        if (narration != null && activated)
        {
            if (!nr_source.isPlaying)
                Stop();
        }

        //run events 
        foreach(invokeable i in invokeables)
        {
            if(i.delay <= timer)
            {
                i.ue.Invoke();
                invokeables.Remove(i);
            }
        }


    }

    private void HandleLight()
    {
        if (raito != null)
        {
            if (activated && lightfade && raito.intensity < intensity)
            {
                raito.intensity += Time.deltaTime * 4.0f;
            } //when fading back out
            else if (!activated && raito.intensity > 0.0f)
            {
                raito.intensity -= Time.deltaTime * 4.0f;
            }
        }
    }

    private void Stop()
    {
        activated = false;
        done = true;

        if (sfx != null)
            sfx_source.Stop();
    }

    //when player enters trigger / new area
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " entered " + this.gameObject.name);

        if (!done && other.gameObject.tag.Equals(customTag))
        {
            Debug.Log(customTag + " found");
            activated = true;
            Play();
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (narration == null)
            Stop();
    }

    void Play()
    {

        //play narration
        if (narration != null)
        {
            nr_source.Play();
        }


        //play sfx
        if (sfx != null)
        {
            sfx_source.Play();
        }

        if(raito != null)
            raito.enabled = true;


        lightfade = true;

        if (currentParticles != null)
            foreach (ParticleSystem p in currentParticles)
            {
                p.Stop();
            }

        if (nextParticles != null)
            foreach (ParticleSystem p in nextParticles)
            {
                p.Play();
            }
    }
    
}
