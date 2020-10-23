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
    string tag = "Player";

    [SerializeField]
    OVRScreenFade fade = null;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals(tag))
        {
            if (fade == null)
            {
                fade = GameObject.Find("CenterEyeAnchor").GetComponent<OVRScreenFade>();
            }

            Debug.Log("hit by player");
            StartCoroutine(loadScene());
        }
    }


    private IEnumerator loadScene()
    {
        if (fade != null)
        {
            Debug.Log("Starting fade");

            fade.fadeOnStart = true;
            fade.fadeColor = Color.black;
            fade.fadeTime = 1.5f;
            fade.FadeOut();

            yield return new WaitForSeconds(2.0f);
        }
        else
            Debug.Log("fade == null - " + this.gameObject.name);

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
