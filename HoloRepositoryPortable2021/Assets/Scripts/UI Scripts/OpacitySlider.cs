using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>Defines the behaviour of the opacity slider</summary>
public class OpacitySlider : MonoBehaviour
{
    Slider slider;
    void Awake(){
        slider = this.GetComponent<Slider>();
        slider.onValueChanged.AddListener(adjustOpacity);
    }
    public void adjustOpacity(float newOpacity){
        EventManager.current.onChangeOpacity(newOpacity);
    }
}
