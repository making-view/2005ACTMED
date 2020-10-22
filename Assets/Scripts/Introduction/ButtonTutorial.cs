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

    int i = 0;

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
        CheckInOrder();
    }


    private void CheckInOrder()
    {
        if (OVRInput.GetDown(actionList[i].input) || Input.GetKeyDown(actionList[i].debugKey))
        {
            //play celebratory sound, update GUI etc, idk
            actionList[i].numTimes -= 1;

            UpdateGUI();

            if (actionList[i].numTimes <= 0) //if done with task
            {
                i++;
                if (i >= actionList.Count) //if done with entire list
                {
                    Done();
                }
            }
        }
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
