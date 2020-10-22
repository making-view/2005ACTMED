using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TutorialGUI : MonoBehaviour
{
    [SerializeField] bool tryGetElementsOnClick = true;


    [SerializeField] private Text titleText = null;
    [HideInInspector] public string title = "Introduksjon";

    [SerializeField] private Text rotationText_R = null;
    [SerializeField] private Image rotationIMG_R = null;
    [HideInInspector] public string rotation_R = "Roter til høyre: ";
    public int complete_R = 0;
    public int toDo_R = 0;

    [SerializeField] private Text rotationText_L = null;
    [SerializeField] private Image rotationIMG_L = null;
    [HideInInspector] public string rotation_L = "Roter til venstre: ";
    public int complete_L = 0;
    public int toDo_L = 0;

    [SerializeField] private Text movementText = null;
    [SerializeField] private Image movementIMG = null;
    [HideInInspector] public string movement = "Gå mot båten";

    [SerializeField] private Text rotationText_Tip = null;
    [HideInInspector] public string rotation_Tip = "*bruk høyre stikke for å rotere deg";

    [SerializeField] private Text movementText_Tip = null;
    [HideInInspector] public string movement_Tip = "*bruk venstre stikke for å bevege deg";


    private void OnValidate()
    {
        if (tryGetElementsOnClick && Selection.Contains(gameObject))
        {
            foreach (Text t in GetComponentsInChildren<Text>())
            {
                //get text objects
                switch (t.gameObject.name)
                {
                    case "Title":
                        titleText = t;
                        break;

                    case "Rotation_L":
                        rotationText_L = t;
                        break;

                    case "Rotation_R":
                        rotationText_R = t;
                        break;

                    case "Movement":
                        movementText = t;
                        break;

                    case "Rotation_Tip":
                        rotationText_Tip = t;
                        break;

                    case "Movement_Tip":
                        movementText_Tip = t;
                        break;
                }
            }
            foreach (Image i in GetComponentsInChildren<Image>())
            {
                //get image checkboxes
                switch (i.gameObject.name)
                {
                    case "Rotation_L_Image":
                        rotationIMG_L = i;
                        break;

                    case "Rotation_R_Image":
                        rotationIMG_R = i;
                        break;

                    case "Movement_Image":
                        movementIMG = i;
                        break;
                }
            }
        }

        UpdateVariables();
    }

    public void UpdateVariables()
    {

        //update variables
        titleText.text = title;

        rotationText_R.text = rotation_R + "(" + complete_R + "/" + toDo_R + ")";
        rotationIMG_R.enabled = complete_R == toDo_R;

        rotationText_L.text = rotation_L + "(" + complete_L + "/" + toDo_L + ")";
        rotationIMG_L.enabled = complete_L == toDo_L;


        rotationText_Tip.text = rotation_Tip;

        movementText_Tip.text = movement_Tip;

        movementText.text = movement;
    }
}
