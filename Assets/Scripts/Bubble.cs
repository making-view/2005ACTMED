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

    [Range(0.0f, 100f)]
    public float speed = 50f;

    [Range(0.0f, 100f)]
    public float devMag = 50f;

    [Range(0.0f, 10f)]
    public float maxFollow = 3.0f;

    [Range(0.0f, 10f)]
    public float followOrAscend = 0.5f;

    //[Range(0.0f, 10.0f)]
    //public float deviation = 1;

    private Rigidbody body = null;

    // Start is called before the first frame update
    void Start()
    {
        body = this.GetComponent<Rigidbody>();
        text = this.GetComponentInChildren<Text>();
        LookAtConstraint lookat = this.GetComponentInChildren<LookAtConstraint>();

        GameObject camera = GameObject.FindObjectOfType<Camera>().gameObject;
        ConstraintSource constraint = new ConstraintSource();
        constraint.weight = 1;
        constraint.sourceTransform = camera.transform;

        lookat.AddSource(constraint);

        parent = GameObject.FindObjectOfType<BubbleSpawner>();

        if (parent != null)
        {
            Debug.Log("topbot set");

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
        Vector3 dir = CalculateDir();

        //make follow coordinate based
        //this.gameObject.transform.position += Time.deltaTime * speed * dir;

        body.AddForce(dir * speed * Time.deltaTime);


        Vector3 devec = new Vector3(Mathf.Sin(Time.time + offset), 0, Mathf.Cos(Time.time + offset)) * devMag * Time.deltaTime;

        body.AddForce(devec);

        if (transform.position.y > YMax.transform.position.y)
        {
            //Respawn();
            Destroy(this.gameObject);
        }

        //increase in size and float around XZTarget
        //disappear when reached height of ZMax
    }

    private Vector3 CalculateDir()
    {
        
        //calculate direction vector
        return new Vector3(
            Mathf.Clamp(XZTarget.transform.position.x - transform.position.x, -maxFollow, maxFollow),
            followOrAscend,
            Mathf.Clamp(XZTarget.transform.position.z - transform.position.z, -maxFollow, maxFollow))
            .normalized;
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
