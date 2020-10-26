using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;





//This class plays sfx and handles particles and light sources for a volume of user area
public class EventVolume : MonoBehaviour
{
    //stucts for handling delayed invokes
    [Serializable]
    private struct invokeable
    {
        public float delay;
        public UnityEvent ue;
    }

    //class for handling lights
    [Serializable]
    private class lighting
    {
        public Light raito;
        public float intensity = 0;
        public bool lightfade = false;
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

    private List<lighting> lights;

    [SerializeField]
    List<ParticleSystem> currentParticles;

    [SerializeField]
    List<ParticleSystem> nextParticles;

    [SerializeField]
    List<invokeable> onEnterInvoke;

    [SerializeField]
    List<invokeable> onExitInvoke;

    [SerializeField]
    string customTag = "";

    // Start is called before the first frame update
    void Start()
    {
        //add existing lights
        lights = new List<lighting>();
        var lComponents = GetComponentsInChildren<Light>();
        foreach (Light l in lComponents)
            lights.Add(
                new lighting
                {
                    raito = l,
                    intensity = l.intensity,
                    lightfade = false
                });

        foreach (lighting l in lights)
        {
            if (l.raito != null)
            {
                l.intensity = l.raito.intensity;
                l.raito.intensity = 0;
                l.raito.enabled = false;
            }
        }

        //create audiosources
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
    {    

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
        foreach(invokeable i in onEnterInvoke)
        {
            if(i.delay < timer)
            {
                i.ue.Invoke();
                onEnterInvoke.Remove(i);
            }
        }
    }

    private void HandleLight()
    {
        foreach (lighting l in lights)
        {

            if (l.raito != null)
            {
                if (activated && l.lightfade && l.raito.intensity < l.intensity)
                {
                    l.raito.intensity += Time.deltaTime * 4.0f;
                } //when fading back out
                else if (!activated && l.raito.intensity > 0.0f)
                {
                    l.raito.intensity -= Time.deltaTime * 4.0f;
                }
                else if (l.lightfade && l.raito.intensity <= 0.0f)
                    l.raito.enabled = false;
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

        foreach (lighting l in lights)
        {
            //Debug.Log("enabling: " + l.raito.gameObject.name);
            if (l.raito != null)
            {
                //Debug.Log("not null");
                l.raito.enabled = true;
                l.lightfade = true;
            }
        }
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
