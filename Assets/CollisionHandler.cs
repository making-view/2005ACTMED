using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hit by " + collision.gameObject.name);
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log("hit by " + hit.gameObject.name);
    //}
}
