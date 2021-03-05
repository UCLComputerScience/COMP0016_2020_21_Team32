using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ChangeCameraAxis : MonoBehaviour
{
    private bool isEnabled = false;
    [SerializeField] GameObject pivotController;
    [SerializeField] GameObject pivot;
    void Start()
    {
        subscribeToEvents();
    }

    private void subscribeToEvents(){
        EventManager.current.OnEnableCamera += otherEvent;
        EventManager.current.OnEnablePivot += EventManager_OnChangePivot;
        EventManager.current.OnEnableCrossSection += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
        EventManager.current.OnReset += otherEvent;
        EventManager.current.OnViewAnnotations += otherEvent;
        EventManager.current.OnAddAnnotations += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
        /*Remember to do this for the other classes later*/
    }
    public void changeCameraAxis(){
        //isEnabled = false;
    }

    /*Sets the pivot controller to active and */
    public void EventManager_OnChangePivot(object sender, EventArgs e){
        //pivot.transform.position = CameraMovement.target.position;
        pivotController.SetActive(true);
        // MaterialAssigner.reduceOpacityAll(0.4f, ModelHandler.segments);
    }    
    public void otherEvent(object sender, EventArgs e){
        Debug.Log("disabling axes");
        isEnabled = false;
        pivotController.SetActive(false);
        pivot.SetActive(false);
        // MaterialAssigner.resetOpacities(ModelHandler.segments);
    }
}
