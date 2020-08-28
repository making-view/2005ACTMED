using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{

    [SerializeField]
    private bool objectsVisible = true; 

    [SerializeField]
    GameObject bubblePrefab;

    //These are the XZ follow targets assigned to bubbles
    [SerializeField]
    List<GameObject> targets;
    private int tarn = 0;

    //top and bottom of bubble stream
    public GameObject minY;
    public GameObject maxY;

    //list of positive thoughts and last one used
    [SerializeField]
    List<string> positiveThoughts;
    private int posn = 0;

    //list of negative thoughts and last one used
    [SerializeField]
    List<string> negativeThoughts;
    private int negn = 0;

    //bubbles per minute
    [Range(0, 600)]
    public float BPM = 60;
    float cooldown = 1;

    //amount of bubbles containing positive messagess
    [SerializeField]
    [Range(0, 100)]
    private float positivePercentage = 0;

    // Start is called before the first frame update
    void Start()
    {
        ToggleVisibility(objectsVisible);
    }

    void OnValidate()
    {
        ToggleVisibility(objectsVisible);
    }

    private void ToggleVisibility(bool visible)
    {
        //make target meshes invisible
        GetComponentInParent<MeshRenderer>().enabled = visible;
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in renderers)
            r.enabled = visible;
    }

    // Update is called once per frame
    void Update()
    {
        TestBPM();


        if((cooldown -= Time.deltaTime) <= 0)
        {
            Debug.Log("Bubble Spawn");

            GameObject target = getTarget();
            Vector3 spawnPoint = target.transform.position;
            spawnPoint.y = minY.transform.position.y;

            Instantiate(bubblePrefab, spawnPoint, transform.rotation);

            cooldown = 60/BPM;
        }
    }

    private void TestBPM()
    {
        int maxBubbles = 600;
        BPM = maxBubbles * (Time.time / 120) % maxBubbles;
    }

    //gets new target for bubble to follow
    public GameObject getTarget()
    {
        GameObject target = this.gameObject;

        if (targets.Count > 0)
        {
            target = targets[tarn];
            tarn = (tarn + 1) % targets.Count;
        }
        else
            Debug.Log("no targets assigned bubble spawner", this);

        return target;
    }

    //get text to put in bubble
    public string getText()
    {
        if (Random.Range(0, 100) <= positivePercentage)
            return getPositive();
        else return getNegative();
    }

    //return a message from the list of positive thoughts
    private string getPositive()
    {
        string positive = "Positive";

        if(positiveThoughts.Count > 0)
        {
            positive = positiveThoughts[posn];
            posn = (posn + 1) % positiveThoughts.Count;
        }

        return positive;
    }

    //return a message from the list of negative thoughts
    private string getNegative()
    {
        string positive = "Negative";

        if (positiveThoughts.Count > 0)
        {
            positive = positiveThoughts[posn];
            posn = (posn + 1) % positiveThoughts.Count;
        }

        return positive;
    }
}
