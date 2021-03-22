using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
///<summary>Small helper script that can be added to a toggle. Ensures that the colour of the toggle stays in its selected state
///while the value of its isOn variable is true, rather than until any other part of the scene is clicked.</summary>
public class ToggleColourChange : MonoBehaviour
{
    private Toggle toggle;
    ColorBlock colourBlock;
    Color originalCol;
    void Start()
    {
        /*initialise variables and pass callback to the onValueChanged event of the toggle*/
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(changeColour);
        originalCol = Color.white; //normal colour is white
        colourBlock = toggle.colors;
        changeColour(toggle.isOn);
    }

    /*The toggle has its selected colour if isOn is true, otherwise has its normal colour*/
    public void changeColour(bool isOn){
        if(isOn){
            colourBlock.normalColor = toggle.colors.selectedColor;
        }else{
            colourBlock.normalColor = originalCol;
        }
        toggle.colors = colourBlock;
    }
}
