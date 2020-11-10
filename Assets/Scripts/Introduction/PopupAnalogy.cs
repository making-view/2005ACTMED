using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PopupAnalogy : MonoBehaviour
{
    public enum Task
    {
        Bonk,
        Pop,
        Run,
        Groper,
        None
    }


    private int[] scale = new int[] { 0, 2, 4, 5, 7, 9, 11, 12 };

    [Serializable]
    private class Event
    {
        [SerializeField] public string name = "default";
        [SerializeField] public Task task = Task.None;
        [SerializeField] public AudioClip narration = null;
        [SerializeField] public int numberOfTimes = 4;
        [SerializeField] public float delayOnComplete = 2.0f;

        [SerializeField] public bool playFeedback = true;
        [SerializeField] public UnityEvent onStartOfEvent = null;
        [SerializeField] public UnityEvent onBeforeEventNarration = null;
    }

    private int maxTask = 0;
    private int eventIndex = 0;
    [SerializeField] private List<Event> events = null;
    private bool waitingForNextEvent = true;
    private AudioSource narrationSource = null;
    private AudioSource feedbackSource = null;

    [SerializeField] private AudioClip feedback = null;

    // Start is called before the first frame update
    private void Start()
    {
        narrationSource = gameObject.AddComponent<AudioSource>();
        feedbackSource = gameObject.AddComponent<AudioSource>();
        feedbackSource.volume = 0.3f;
        feedbackSource.clip = feedback;
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
        StartCoroutine(StartEvent(eventIndex));
    }

    public bool DoTask(Task action)
    {
        var didTask = false;

        if (!waitingForNextEvent && eventIndex < events.Count)
        {
            didTask = true;

            if (events[eventIndex].task.Equals(action))
            {
                feedbackSource.Stop();
                events[eventIndex].numberOfTimes--;

                if (!action.Equals(Task.None))
                {
                    float step = 0.0f;
                    if (maxTask == 1)
                        step = 0;
                    else
                        step = (maxTask - 1.0f - events[eventIndex].numberOfTimes) / (maxTask - 1.0f);

                    //step goes from 0 to 1 based on tasks done
                    var tone = Mathf.Pow(1.05946f, scale[Mathf.RoundToInt(step * 7)]);

                    feedbackSource.pitch = tone;

                    if(events[eventIndex].playFeedback || events[eventIndex].numberOfTimes <= 0)
                        feedbackSource.Play();
                }
            }

            if (events[eventIndex].numberOfTimes <= 0)
            {
                ++eventIndex;

                waitingForNextEvent = true;
                StartCoroutine(StartEvent(eventIndex));
            }
        }

        return didTask;
    }

    private IEnumerator StartEvent(int newIndex)
    {
        //if donzo
        if (newIndex >= events.Count)
        {
            yield return new WaitForSeconds(events[newIndex - 1].delayOnComplete);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            if (newIndex > 0)
                yield return new WaitForSeconds(events[newIndex - 1].delayOnComplete);

            if (events[newIndex].narration != null)
            {
                narrationSource.clip = events[newIndex].narration;
                narrationSource.Play();
            }

            events[newIndex].onBeforeEventNarration.Invoke();

            while (narrationSource.isPlaying)
                yield return null;

            events[newIndex].onStartOfEvent.Invoke();

            maxTask = events[newIndex].numberOfTimes;

            waitingForNextEvent = false;

            DoTask(Task.None);
        }
    }

    private void Update()
    {
        //for testing, pls delete this shit
        if (Input.GetKeyDown(KeyCode.B))
            DoTask(Task.Bonk);

        if (Input.GetKeyDown(KeyCode.G))
            DoTask(Task.Groper);

        if (Input.GetKeyDown(KeyCode.P))
            DoTask(Task.Pop);

        //apart from this hahahehe
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).magnitude > 0.5f)
            DoTask(Task.Run);

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
