using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class PopupAnalogy : MonoBehaviour
{
    public enum ToDo
    {
        bonk,
        pop,
        run,
        groper,
        none
    }

    [Serializable]
    private class Event
    {
        [SerializeField] public string name = "default";
        [SerializeField] public ToDo task = ToDo.none;
        [SerializeField] public AudioClip narration = null;
        [SerializeField] public int numberOfTimes = 4;
        [SerializeField] public float delayOnComplete = 2.0f;

        [SerializeField] public UnityEvent onStartOfEvent = null;
    }

    private int eventIndex = 0;
    [SerializeField] private List<Event> events = null;
    private bool waitingForNextEvent = true;
    private AudioSource audio = null;

    // Start is called before the first frame update
    private void Start()
    {
        audio = GetComponent<AudioSource>();        
    }

    void Awake()
    {
        if (events.Count <= 0)
            throw new SystemException("haha, fuck you! CIA niggers \n\t\t\t\t\t\t-" + name);
    }

    public void StartPopupshit()
    {
        eventIndex = 0;
        waitingForNextEvent = false;
        StartTask(0);
    }

    public void DoAction(ToDo action)
    {
        if(!waitingForNextEvent && eventIndex < events.Count)
        {
            if (events[eventIndex].task.Equals(action))
                events[eventIndex].numberOfTimes--;

            if (events[eventIndex].numberOfTimes <= 0)
            {
                waitingForNextEvent = true;
                StartCoroutine(StartTask(eventIndex));
            }
        }
    }

    private IEnumerator StartTask(int newIndex)
    {
        //if donzo
        if (newIndex >= events.Count)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            while (audio.isPlaying)
                yield return null;

            if (eventIndex > 0)
            {
                Debug.Log("starting task " + events[newIndex].name + " in " + events[newIndex - 1].delayOnComplete + " seconds after audio");
                yield return new WaitForSeconds(events[newIndex - 1].delayOnComplete);
            }

            waitingForNextEvent = false;
            events[newIndex].onStartOfEvent.Invoke();

            if (events[newIndex].narration != null)
            {
                audio.clip = events[newIndex].narration;
                audio.Play();
            }

            if (++eventIndex >= events.Count)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
            DoAction(ToDo.none);
        }
    }


    //Testshit
    private void Update()
    {
        //for testing, pls delete this shit
        if (Input.GetKeyDown(KeyCode.B))
            DoAction(ToDo.bonk);

        if (Input.GetKeyDown(KeyCode.G))
            DoAction(ToDo.groper);

        if (Input.GetKeyDown(KeyCode.P))
            DoAction(ToDo.pop);

        //apart from this hahahehe
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).magnitude > 0.5f)
            DoAction(ToDo.run);

    }

    public void ChangeHandMode(string newMode)
    {
        Slapper.InteractionMode mode = Slapper.InteractionMode.Bonkers;

        switch(newMode.ToLower())
        {
            case "bonkers":
                mode = Slapper.InteractionMode.Bonkers;
                break;
            case "changers":
                mode = Slapper.InteractionMode.Changers;
                break;
            case "poppers":
                mode = Slapper.InteractionMode.Poppers;
                break;
        }

        foreach (Slapper s in FindObjectsOfType<Slapper>())
            s.ChangeInteractionMode(mode);
    }

}
