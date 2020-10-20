using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTutorial : MonoBehaviour
{
    [Serializable]
    struct ActionElem
    {
        public OVRInput.Button input;
        public KeyCode debugKey;
        public int numTimes;
        //[SerializeField] float cooldown;
        //[SerializeField] Action eventAction;
    }

    //[SerializeField] private bool inOrder = true;
    [SerializeField]  private List<ActionElem> actions;
    private ActionElem currAction;
    int i = 0;

    private void Start()
    {
        if (actions.Count <= 0)
            Debug.LogWarning("empty tutorial " + gameObject.name);
        else
        {
            currAction = actions[i];
        }
    }

    private void Update()
    {

        if(OVRInput.GetDown(currAction.input) || Input.GetKeyDown(currAction.debugKey))
        {
            Debug.Log("action done: " + currAction.input.ToString());
        }
    }
}
