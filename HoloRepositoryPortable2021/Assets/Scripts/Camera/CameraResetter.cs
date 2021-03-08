using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

///<Summary>This class handles resetting the position of the camera via means of linear interpolation</summary>
public class CameraResetter : MonoBehaviour
{
    public float travelTime = 2;
    Vector3 startPos;
    Quaternion startRot;
    Vector3 targetPos;
    Quaternion targetRot;
    float timeElapsed;
    public bool isEnabled = false;
    void Start(){
        subscribeToEvents();    
    }

    /*When the reset toggle is pressed in the navigation bar, the camera is linearly interpolated from its current position to its original position.
    The time taken for this reset to occur is determined by the travelTime variable*/
    void Update()
    {
        if(!isEnabled)return;
        timeElapsed +=Time.deltaTime; 
        float ratio = timeElapsed/travelTime;
        Camera.main.gameObject.transform.position = Vector3.Lerp(startPos, targetPos, ratio);
        Camera.main.gameObject.transform.rotation = Quaternion.Lerp(startRot, targetRot, ratio);
        if(ratio >= 1){
            isEnabled = false;
            timeElapsed = 0;
            ratio = 0;
            EventManager.current.onEnableCamera();
        }
    }

    public void EventManager_OnReset(object sender, EventArgs e){
        isEnabled = true;
        timeElapsed = 0.0f;
        startPos = Camera.main.gameObject.transform.position; 
        startRot = Camera.main.gameObject.transform.rotation;
        targetPos = CameraController.startPos;
        targetRot = CameraController.startRot;  
    }
    public void otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }
    /*Ensures this script is only enabled when the relevant event is received.*/
    private void subscribeToEvents(){
        EventManager.current.OnEnableCamera += otherEvent;
        EventManager.current.OnEnablePivot += otherEvent;
        EventManager.current.OnEnableCrossSection += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
        EventManager.current.OnReset += EventManager_OnReset;
        EventManager.current.OnViewAnnotations += otherEvent;
        EventManager.current.OnAddAnnotations += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
    }
}
