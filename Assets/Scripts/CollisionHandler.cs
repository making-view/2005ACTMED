using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    string sceneToLoad = "";

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("hit by " + collider.gameObject.name);

        if (collider.gameObject.tag.Equals("Player"))
        {
            Debug.Log("hit by player");
            StartLoading();
        }
    }

    private void StartLoading()
    {
        if(!sceneToLoad.Equals(""))
            SceneManager.LoadScene(sceneToLoad);
        else
        {
                Debug.Log("do other stuff");
        }
    }

}
