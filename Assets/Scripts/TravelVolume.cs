using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Oculus;
using System;
using System.Linq;

public class TravelVolume : MonoBehaviour
{
    [SerializeField]
    string sceneToLoad = "";

    [SerializeField]
    OVRScreenFade fade = null;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("hit by " + collider.gameObject.name);

        if (collider.gameObject.tag.Equals("Player"))
        {
            Debug.Log("hit by player");
            StartCoroutine(loadScene());
        }
    }


    private IEnumerator loadScene()
    {
        Debug.Log("Starting fade");

        fade.fadeOnStart = true;
        fade.fadeColor = Color.black;
        fade.fadeTime = 2.0f;
        fade.FadeOut();
        Debug.Log("fading");

        yield return new WaitForSeconds(2.0f);
        StartLoading(); 
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


    //private void Start()
    //{
    //    fade = GetComponent<OVRScreenFade>();
    //}
}
