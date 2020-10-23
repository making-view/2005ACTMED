using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonTutorial : MonoBehaviour
{
    [Serializable]
    private class ActionElem
    {
        public OVRInput.Button input;
        public KeyCode debugKey;
        public int numTimes;
        [HideInInspector] public int startNumTimes;
    }

    [SerializeField] private List<ActionElem> actionList;
    [SerializeField] UnityEvent doAfterTutorial;
    [SerializeField] TutorialGUI tutGUI = null;

    private void Start()
    {
        if (actionList.Count <= 0)
            Debug.LogWarning("empty tutorial " + gameObject.name);


        for (int i = 0; i < actionList.Count; i++)
            actionList[i].startNumTimes = actionList[i].numTimes;

        SetGUI();
        UpdateGUI();
    }

    private void Update()
    {
        CheckOutOfOrder();
    }


    private void CheckOutOfOrder()
    {
        int actionsRemaining = 0;

        foreach(ActionElem a in actionList)
        {
            if (OVRInput.GetDown(a.input) || Input.GetKeyDown(a.debugKey))
            {
                if(a.numTimes > 0)
                {
                    a.numTimes -= 1;
                    if(a.numTimes == 0)
                    {
                        var audio = GetComponent<AudioSource>();
                        audio.Play();
                    }
                }

            }
            actionsRemaining += a.numTimes;
        }

        UpdateGUI();
        if (actionsRemaining <= 0)
            Done();
    }

    private void Done()
    {
        doAfterTutorial.Invoke();
        enabled = false;
    }

    private void OnValidate()
    {
        SetGUI();
        tutGUI.UpdateVariables();
    }

    //set max targets for actions
    private void SetGUI()
    {
        if (tutGUI == null)
            tutGUI = GetComponentInChildren<TutorialGUI>();

        foreach (ActionElem a in actionList)
            switch (a.input)
            {
                case OVRInput.Button.SecondaryThumbstickLeft:
                    tutGUI.toDo_L = a.startNumTimes;
                    break;


                case OVRInput.Button.SecondaryThumbstickRight:
                    tutGUI.toDo_R = a.startNumTimes;
                    break;
            }
    }

    //update current actions done towards target
    private void UpdateGUI()
    {
        foreach (ActionElem a in actionList)
            switch (a.input)
            {
                case OVRInput.Button.SecondaryThumbstickLeft:
                    tutGUI.complete_L = a.startNumTimes - a.numTimes;
                    break;


                case OVRInput.Button.SecondaryThumbstickRight:
                    tutGUI.complete_R = a.startNumTimes - a.numTimes;
                    break;
            }

        tutGUI.UpdateVariables();
    }
}
