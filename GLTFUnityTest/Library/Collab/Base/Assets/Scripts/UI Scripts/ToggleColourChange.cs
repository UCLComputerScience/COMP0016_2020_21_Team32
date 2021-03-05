using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColourChange : MonoBehaviour
{
    // Start is called before the first frame update
    private Toggle toggle;
    ColorBlock colourBlock;
    Color originalCol;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(changeColour);
        originalCol = new Color(255f, 255f, 255f);
        colourBlock = toggle.colors;
        changeColour(toggle.isOn);
    }

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
