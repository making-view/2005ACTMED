using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private BubbleSpawner parent = null;
    private Text text = null;

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

    private Rigidbody body = null;
    private bool upward = true;

    [SerializeField]
    private bool clockwise = true;

    private float size = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        //50% chance of going clockwise
        if (UnityEngine.Random.Range(0, 1.0f) > 0.5f)
            clockwise = false;

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

        parent = GameObject.FindObjectOfType<BubbleSpawner>();

        if (parent != null)
        {
            //Debug.Log("topbot set");
            YMin = parent.minY;
            YMax = parent.maxY;
        }

        offset = UnityEngine.Random.Range(0, Mathf.PI);

        text.text = parent.getText();
        XZTarget = parent.getTarget();
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

        //make follow coordinate based
        //this.gameObject.transform.position += Time.deltaTime * speed * dir;

        body.AddForce(dir * speed * Time.deltaTime);

        var deviate = devMag;

        if (!clockwise)
            deviate *= -1;

        Vector3 devec = new Vector3(Mathf.Sin(Time.time + offset), 0, Mathf.Cos(Time.time + offset)) * deviate * Time.deltaTime;

        body.AddForce(devec);

        //go down when all the way up
        if (transform.position.y > YMax.transform.position.y)
        {
            //Respawn();
            //Destroy(this.gameObject);
            upward = false;
        }

        //go up when all the way down
        if (transform.position.y < YMin.transform.position.y)
        {
            //Respawn();
            //Destroy(this.gameObject);
            upward = true;
        }

        //increase in size and float around XZTarget
        //disappear when reached height of ZMax
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
        text.text = parent.getText();
        XZTarget = parent.getTarget();
        //reset y position
        this.gameObject.transform.position = new Vector3(XZTarget.transform.position.x, YMin.transform.position.y, XZTarget.transform.position.z);
    }

}
