using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
