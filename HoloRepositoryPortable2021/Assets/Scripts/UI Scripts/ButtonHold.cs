using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

///<summary>Script that invokes a method whenever the pointer is held down. Used to implement press and hold buttons, rather
/// than the standard Unity OnClick buttons.</summary>
public class ButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown; 
    public UnityEvent method; //function to be called can be passed in the Unity Inspector

    public void OnPointerDown(PointerEventData data){
        pointerDown = true;
    }
    public void OnPointerUp(PointerEventData data){
        pointerDown = false;
    }
    void Update(){
        if(pointerDown){
            method?.Invoke();
        }
    }
}
