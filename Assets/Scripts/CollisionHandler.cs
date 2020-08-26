using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    string sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("hit by " + collider.gameObject.name);

        if (collider.gameObject.tag.Equals("Player"))
        {
            Debug.Log("hit by player");
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log("hit by " + hit.gameObject.name);
    //}
}
