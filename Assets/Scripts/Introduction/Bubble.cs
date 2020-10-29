using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class Bubble : MonoBehaviour
{
    [Serializable]
    private class Behaviour
    {
        public string name = "default";
        public float speed = 50f;
        public float devMag = 50f;
        public float maxFollow = 20.0f;
        public float followOrAscend = 0.5f;
        public Color spotsColor = Color.white;
        public Color extremeColor = Color.white;
    }

    [SerializeField] List<Behaviour> behaviours;


    [SerializeField] private BubbleSpawner parent = null;
    private Text text = null;
    [SerializeField] private AudioClip plopp = null;

    private GameObject XZTarget;
    private GameObject YMin;
    private GameObject YMax;

    private float offset;

    [Tooltip("Speed of bubble")]
    [Range(0.0f, 1000f)]
    public float speed = 50f;

    [Tooltip("magnitude of deviation from path")]
    [Range(0.0f, 100f)]
    public float devMag = 50f;

    [Tooltip("A limit to how fast the bubble can move to follow")]
    [Range(0.0f, 20f)]
    public float maxFollow = 20.0f;

    [Tooltip("0 = only follow. 10 = ascent 10 times more than following")]
    [Range(0.0f, 10f)]
    public float followOrAscend = 0.5f;

    //[Range(0.0f, 10.0f)]
    //public float deviation = 1;
    private MeshRenderer meshRenderer = null;
    private Material instancedMaterial = null;
    private Rigidbody body = null;
    private bool upward = true;
    private AudioSource audio = null;

    [SerializeField]
    private bool clockwise = true;

    private float size = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        //50% chance of going clockwise
        if (UnityEngine.Random.Range(0, 1.0f) > 0.5f)
            clockwise = false;

        meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        instancedMaterial = meshRenderer.material;

        //get references 
        body = this.GetComponent<Rigidbody>();
        text = this.GetComponentInChildren<Text>();
        LookAtConstraint lookat = this.GetComponentInChildren<LookAtConstraint>();

        //make bubbles follow camera
        GameObject camera = GameObject.FindObjectOfType<Camera>().gameObject;
        ConstraintSource constraint = new ConstraintSource();
        constraint.weight = 1;
        constraint.sourceTransform = camera.transform;

        lookat.AddSource(constraint);

        parent = 
            FindObjectOfType<BubbleSpawner>();
        audio = GetComponent<AudioSource>();

        if (parent != null)
        {
            //Debug.Log("topbot set");
            YMin = parent.minY;
            YMax = parent.maxY;
            plopp = parent.GetPopSound();

            audio.pitch += UnityEngine.Random.Range(-0.2f, 0.2f);
            XZTarget = parent.GetTarget();
        }
        else
            Debug.Log("orphan desu");

        offset = UnityEngine.Random.Range(0, Mathf.PI);

        //text.text = parent.GetText();
    }

    // Update is called once per frame

    void Update()
    {
        if (size < Mathf.PI / 2)
        {
            size += Time.deltaTime;
            float scale = Mathf.Sin(size);
            transform.localScale = new Vector3(scale, scale, scale);
        }

        Vector3 dir = CalculateDir();
        body.AddForce(dir * speed * Time.deltaTime);

        var deviate = devMag;

        if (!clockwise)
            deviate *= -1;

        Vector3 devec = new Vector3(Mathf.Sin(Time.time + offset), 0, Mathf.Cos(Time.time + offset)) * deviate * Time.deltaTime;

        body.AddForce(devec);

        //go down when all the way up
        if (transform.position.y > YMax.transform.position.y)
            upward = false;

        if (transform.position.y < YMin.transform.position.y)
            upward = true;
    }

    private Vector3 CalculateDir()
    {
        float up = followOrAscend;

        //go down if not going upward
        if (!upward)
            up = -up;

        
        //calculate direction vector
        return new Vector3(
            Mathf.Clamp(XZTarget.transform.position.x - transform.position.x, -maxFollow, maxFollow),
            up,
            Mathf.Clamp(XZTarget.transform.position.z - transform.position.z, -maxFollow, maxFollow));
    }

    //Todo, fade bubble instead of just moving it
    void Respawn()
    {
        body.velocity = Vector3.zero;
        text.text = parent.GetText();
        XZTarget = parent.GetTarget();
        //reset y position
        this.gameObject.transform.position = new Vector3(XZTarget.transform.position.x, YMin.transform.position.y, XZTarget.transform.position.z);
    }

    public void Kill()
    {
       StartCoroutine(PlayPlopp());
    }

    private IEnumerator PlayPlopp()
    {

        var grabbable = GetComponent<OVRGrabbable>();

        if (grabbable.grabbedBy != null)
        {
            grabbable.grabbedBy.ForceRelease(grabbable);
        }

        yield return new WaitForEndOfFrame();

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        audio.Stop();
        audio.clip = plopp;
        audio.Play();

        while (audio.isPlaying)
            yield return null;

        Destroy(gameObject);
    }

    public void ChangeBehaviour(string newBehaviour)
    {
        foreach (Behaviour b in behaviours)
        {
            if(b.name.ToLower().Equals(newBehaviour.ToLower()))
            {
                speed = b.speed;
                devMag = b.devMag;
                maxFollow = b.maxFollow;
                followOrAscend = b.followOrAscend;

                if (!instancedMaterial)
                {
                    meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
                    instancedMaterial = meshRenderer.material;
                }

                instancedMaterial.SetColor("_Colour_Tint", b.spotsColor);
                instancedMaterial.SetColor("_ExtremeColour", b.extremeColor);

                return;
            }
        }
    }
}
