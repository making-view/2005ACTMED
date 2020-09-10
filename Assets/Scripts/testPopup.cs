using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public class testPopup : MonoBehaviour
{
    [SerializeField]
    BubbleSpawner spawner = null;

    bool activated = false;
    bool done = false;

    [SerializeField]
    float timer = 20;

    [SerializeField]
    float BPM = 30;

    float timerMax = 0;

    // Start is called before the first frame update
    void Start()
    {
        timerMax = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                timer = timerMax;
                activated = false;
                spawner.active = false;
                spawner.BPM = 1;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("starting bubbles from space press");
            StartSpawning();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name + " entered " + this.gameObject.name);

        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("player found");

            spawner.destroyBubbles();

            StartSpawning();
        }
    }

    private void StartSpawning()
    {
        spawner.BPM = BPM;
        activated = true;
        spawner.active = true;
    }
}
