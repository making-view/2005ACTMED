using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private BubbleSpawner parent = null;
    private Text text = null;

    private GameObject XZTarget;
    private GameObject YMin;
    private GameObject YMax;

    [Range(0.0f, 2f)]
    public float speed = 1;

    [Range(0.1f, 2f)]
    public float followOrAscend = 0.5f;

    [Range(1f, 5f)]
    public float maxFollow = 2.0f;

    //[Range(0.0f, 10.0f)]
    //public float deviation = 1;

    public bool floating = true;

    // Start is called before the first frame update
    void Start()
    {
        text = this.GetComponentInChildren<Text>();

        if(parent != null)
        {
            Debug.Log("topbot set");

            YMin = parent.minY;
            YMax = parent.maxY;
        }

        Respawn();
    }

    // Update is called once per frame

    void Update()
    {

        //calculate direction vector
        Vector3 dir = new Vector3 (
            Mathf.Clamp(XZTarget.transform.position.x - transform.position.x, -maxFollow, maxFollow),
            followOrAscend,
            Mathf.Clamp(XZTarget.transform.position.z - transform.position.z, -maxFollow, maxFollow))
            .normalized;

        //make follow force based instead of coordinate based
        this.gameObject.transform.position += Time.deltaTime * speed * dir;


        if (transform.position.y > YMax.transform.position.y)
        {
            Respawn();
        }
            
        //increase in size and float around XZTarget
        //disappear when reached height of ZMax
    }

    void Respawn()
    {
        text.text = parent.getText();
        XZTarget = parent.getTarget();
        //reset y position
        this.gameObject.transform.position = new Vector3(XZTarget.transform.position.x, YMin.transform.position.y, XZTarget.transform.position.z);
    }

}
