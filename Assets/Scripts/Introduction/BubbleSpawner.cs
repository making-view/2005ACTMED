using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer))]
public class BubbleSpawner : MonoBehaviour
{
    //thoughts in files
    private string positive;
    private string negative;

    [SerializeField] private bool objectsVisible = true;
    [SerializeField] GameObject bubblePrefab;

    //These are the XZ follow targets assigned to bubbles
    [SerializeField] List<GameObject> targets;
    private int tarn = 0;

    //top and bottom of bubble stream
    public GameObject minY;
    public GameObject maxY;

    //list of positive thoughts and last one used
    [SerializeField] List<string> positiveThoughts;
    private int posn = 0;

    //list of negative thoughts and last one used
    [SerializeField]
    List<string> negativeThoughts;
    private int negn = 0;

    //bubbles per minute
    [Range(0, 600)]
    public int BPM = 60;
    public int numBublees = 50;
    float cooldown = 1;

    private List<GameObject> bubbles = null;

    [SerializeField] List<AudioClip> bubblePopSounds = null;
    private int bubPosn = 0;

    public bool active { get; set; }
    public string currentBehaviour = "default";

    //amount of bubbles containing positive messagess
    [SerializeField]
    [Range(0, 100)]
    private int positivePercentage = 0;

    // Start is called before the first frame update
    void Start()
    {
        //toggle debug objects
        ToggleVisibility(objectsVisible);
        bubbles = new List<GameObject>();

        if (BPM > 0)
            active = true;
    }

    //returns if file found or not and debug logs
    bool FindFile (string filename)
    {
        if (File.Exists(filename))
        {
            Debug.Log("found: " + filename);
            return true;
        }
       
        Debug.LogError("can't find file: " + filename);
        return false;
    }

    //parses lines of text file into list of strings
    List<string> ParseText(string file)
    {
        var list = new List<string>();

        int counter = 0;
        string line;

        System.IO.StreamReader stream =
            new System.IO.StreamReader(file);
        while ((line = stream.ReadLine()) != null)
        {
            //Debug.Log("reading: " + line);
            counter++;
            list.Add(line);
        }

        stream.Close();
        //Debug.Log("There were " + counter + " lines.");

        System.Console.ReadLine();
        return list;
    }

    void OnValidate()
    {
        // find files containing thoughts and parse them
        positive = UnityEngine.Application.dataPath + "/Localized/positive.txt";
        negative = UnityEngine.Application.dataPath + "/Localized/negative.txt";

        if (FindFile(positive))
            positiveThoughts = ParseText(positive);

        if (FindFile(negative))
            negativeThoughts = ParseText(negative);


        ToggleVisibility(objectsVisible);
    }
     
    private void ToggleVisibility(bool visible)
    {
        if(GetComponentInParent<MeshRenderer>() != null)
            GetComponentInParent<MeshRenderer>().enabled = visible;

        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in renderers)
            r.enabled = visible;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if ((cooldown -= Time.deltaTime) <= 0 && bubbles.Count < numBublees)
            {
                SpawnBuuuble();
            }

            while (bubbles.Count > numBublees)
            {
                var buble = bubbles[bubbles.Count - 1];
                bubbles.Remove(buble);
                Destroy(buble);
            }

            ////for testing, pls delete this shit
            //if (Input.GetKeyDown(KeyCode.A))
            //    ChangeAllBubblesBehaviour("angwy");

            //if (Input.GetKeyDown(KeyCode.S))
            //    ChangeAllBubblesBehaviour("default");

            //if (Input.GetKeyDown(KeyCode.D))
            //    ChangeAllBubblesBehaviour("calm");

            //if (Input.GetKeyDown(KeyCode.F))
            //    ChangeAllBubblesBehaviour("positive");
        }
    }

    public void ChangeAllBubblesBehaviour(string newBehaviour)
    {
        currentBehaviour = newBehaviour;

        foreach (GameObject b in bubbles)
            b.GetComponent<Bubble>().ChangeBehaviour(currentBehaviour);
    }

    public GameObject SpawnBuuuble()
    {
        GameObject target = GetTarget();
        Vector3 spawnPoint = target.transform.position;
        spawnPoint.y = minY.transform.position.y;
        var newBubble = Instantiate(bubblePrefab, spawnPoint, transform.rotation);
        newBubble.GetComponent<Bubble>().ChangeBehaviour(currentBehaviour);
        bubbles.Add(newBubble);
        newBubble.GetComponentInChildren<Text>().text = GetNegative();

        cooldown = 60.0f / BPM;

        return newBubble;
    }

    public GameObject SpawnBuuuble(Vector3 position, string behaviour, int index)
    {
        numBublees++;
        var newBubble = Instantiate(bubblePrefab, position, transform.rotation);
        newBubble.GetComponentInChildren<Text>().text = negativeThoughts[index];
        newBubble.GetComponent<Bubble>().ChangeBehaviour(behaviour);
        bubbles.Add(newBubble);

        cooldown = 60.0f / BPM;
        return newBubble;
    }

    public void SpawnBubbles(int num)
    {
        numBublees += num;
    }

    public void PopBubble(GameObject bubble)
    {
        bubbles.Remove(bubble);
        bubble.GetComponent<Bubble>().Kill();
    }

    //gets new target for bubble to follow
    public GameObject GetTarget()
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
    public string GetText()
    {
        int random = Random.Range(0, 100);

        if (random < positivePercentage)
            return GetPositive();
        else return GetNegative();
    }

    //return a message from the list of positive thoughts
    private string GetPositive()
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
    private string GetNegative()
    {
        string negative = "Negative";

        if (negativeThoughts.Count > 0)
        {
            negative = negativeThoughts[negn];
            negn = (negn + 1) % negativeThoughts.Count;
        }

        return negative;
    }

    //return a clip from the list of sounds
    public AudioClip GetPopSound()
    {
        var clip = bubblePopSounds[bubPosn];
        bubPosn = (bubPosn + 1) % bubblePopSounds.Count;

        return clip;
    }


    public int MakePositive(GameObject buuublies)
    {
        var index = -1;

        var buuublieText = buuublies.GetComponentInChildren<Text>();

        if (!positiveThoughts.Any(pt => pt.Equals(buuublieText)))
        {
            for (int i = 0; i < negativeThoughts.Count; i++)
            {
                //Debug.Log("Comparing '" + buuublies.GetComponentInChildren<Text>().text.Trim().ToLower() + "' and '" + negativeThoughts[i].Trim().ToLower() + "'");

                if(buuublieText.text.Equals(negativeThoughts[i]))
                {
                    //Debug.Log("Setting text from: " + negativeThoughts[i] + ", to: " + positiveThoughts[i]);
                    buuublies.GetComponentInChildren<Text>().text = positiveThoughts[i];
                    index = i;
                    break;
                }
            }
        }

        return index;
    }
}
