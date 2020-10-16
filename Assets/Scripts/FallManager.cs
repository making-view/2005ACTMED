using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallManager : MonoBehaviour
{

    [SerializeField] private bool onBridge = true;
    [SerializeField] private string tagString = "";



    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!onBridge)
            Debug.LogWarning("off " + tagString);
        else
            Debug.LogWarning("on " + tagString);
    }


  
    private void OnTriggerStay(Collider other)
    {
        if (!onBridge && other.gameObject.tag.Equals(tagString))
            onBridge = true;

        //Debug.LogWarning("colliding with " + other.gameObject.tag + " on " + other.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!onBridge && other.gameObject.tag.Equals(tagString))
            onBridge = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (onBridge && other.gameObject.tag.Equals(tagString))
            onBridge = false;
    }
}

