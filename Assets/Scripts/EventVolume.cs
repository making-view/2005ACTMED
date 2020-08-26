using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class plays sfx and handles particles and light sources for a volume of user area
public class EventVolume : MonoBehaviour
{
    bool activated = false;
    bool done = false;

    [SerializeField]
    AudioClip narration = null;
    private AudioSource nr_source = null;

    [SerializeField]
    AudioClip sfx = null;
    private AudioSource sfx_source = null;

    [SerializeField]
    Light raito;
    float intensity = 0;
    float fadespeed = 4.0f;
    bool lightfade = false;

    [SerializeField]
    List<ParticleSystem> currentParticles;

    [SerializeField]
    List<ParticleSystem> nextParticles;

    [SerializeField]
    string tag = "";


    // Start is called before the first frame update
    void Start()
    {
        if(raito != null)
        {
            intensity = raito.intensity;
            raito.intensity = 0;

            raito.enabled = false;
        }

        nr_source = gameObject.AddComponent<AudioSource>();
        nr_source.spatialize = false;
        sfx_source = gameObject.AddComponent<AudioSource>();
        sfx_source.spatialize = false;

        if (sfx != null)
            sfx_source.clip = sfx;

        if (narration != null)
            nr_source.clip = narration;

        foreach (ParticleSystem p in nextParticles)
        {
            p.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {    //when fading in

        if (raito != null)
        {
            if (activated && lightfade && raito.intensity < intensity)
            {
                raito.intensity += Time.deltaTime * fadespeed;
            } //when fading back out
            else if (!activated && raito.intensity > 0.0f)
            {
                raito.intensity -= Time.deltaTime * fadespeed;
            }
        }
        //if you should fade back out
        if (narration != null && activated)
        {
            if (!nr_source.isPlaying)
                Stop();
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

        if (!done && other.gameObject.tag.Equals(tag))
        {
            Debug.Log(tag + " found");
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

        foreach (ParticleSystem p in currentParticles)
        {
            p.Stop();
        }


        foreach (ParticleSystem p in nextParticles)
        {
            p.Play();
        }
    }
    
}
