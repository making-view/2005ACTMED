using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayDo : MonoBehaviour
{

    [SerializeField] AudioSource audiosource;
    private bool playing = false;

    [SerializeField] UnityEvent onAudioComplete;


    // Start is called before the first frame update
    void Start()
    {
        if (audiosource == null)
            audiosource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //do stuff when audiosource done playing
        if (!audiosource.isPlaying && playing)
        {
            onAudioComplete.Invoke();
            this.GetComponent<PlayDo>().enabled = false;
        }
        else playing = audiosource.isPlaying;
    }

    public void Play()
    {
        audiosource.Play();
    }
}
