<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
///<summary>Small helper script that can be added to a toggle. Ensures that the colour of the toggle stays in its selected state
///while the value of its isOn variable is true, rather than until any other part of the scene is clicked.</summary>
public class ToggleColourChange : MonoBehaviour
{
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColourChange : MonoBehaviour
{
    // Start is called before the first frame update
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    private Toggle toggle;
    ColorBlock colourBlock;
    Color originalCol;
    void Start()
    {
<<<<<<< HEAD
        /*initialise variables and pass callback to the onValueChanged event of the toggle*/
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(changeColour);
        originalCol = new Color(255f, 255f, 255f); //normal colour is white
=======
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(changeColour);
        originalCol = new Color(255f, 255f, 255f);
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
        colourBlock = toggle.colors;
        changeColour(toggle.isOn);
    }

<<<<<<< HEAD
    /*The toggle has its selected colour if isOn is true, otherwise has its normal colour*/
=======
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    public void changeColour(bool isOn){
        if(isOn){
            colourBlock.normalColor = toggle.colors.selectedColor;
        }else{
            Debug.Log("Here we go");
            colourBlock.normalColor = originalCol;
        }
        toggle.colors = colourBlock;
    }
}
