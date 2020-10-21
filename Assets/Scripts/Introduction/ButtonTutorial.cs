using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTutorial : MonoBehaviour
{
    [Serializable]
    struct ActionElem
    {
        public OVRInput.Button input;
        public KeyCode debugKey;
        public int numTimes;
        //[SerializeField] float cooldown;
    }

    //[SerializeField] private bool inOrder = true;
    [SerializeField] private List<ActionElem> actionList;
    [SerializeField] UnityEvent doAfterTutorial;
    private ActionElem currAction;
    int i = 0;

    private void Start()
    {
        if (actionList.Count <= 0)
            Debug.LogWarning("empty tutorial " + gameObject.name);
        else
        {
            currAction = actionList[i];
        }
    }

    private void Update()
    {

        if(OVRInput.GetDown(currAction.input) || Input.GetKeyDown(currAction.debugKey))
        {
            //play celebratory sound, update GUI etc, idk
            Debug.Log("action done: " + currAction.input.ToString());
            currAction.numTimes -= 1;

            if(currAction.numTimes <= 0) //if done with task
            {
                i++;
                if(i >= actionList.Count) //if done with entire list
                {
                    doAfterTutorial.Invoke();
                    enabled = false;
                }
                else
                {
                    currAction = actionList[i];
                }
            }
        }
    }
}
