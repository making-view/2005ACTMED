using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class changePitchOnPlay : MonoBehaviour
{
    private AudioSource audiosource = null;
    private bool playing = false;
    [SerializeField] private float change = 0.2f;

    private void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (audiosource.isPlaying && !playing)
        {
            playing = true;
            StartCoroutine(changePitchDelayed());
        }
        else playing = audiosource.isPlaying;
    }

    private IEnumerator changePitchDelayed()
    {
        yield return new WaitForSeconds(audiosource.time);
        audiosource.pitch += change;
    }
}
