using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResetPosition : MonoBehaviour
{
    public float travelTime = 2;
    Vector3 startPos;
    Quaternion startRot;
    Vector3 endPos;
    Vector3 targetPos;
    Quaternion targetRot;
    float timeElapsed;
    bool isEnabled = false;
    void Start(){
        subscribeToEvents();    
    }

    // Update is called once per frame
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

    public void subscribeToEvents(){
        EventManager.current.OnEnableCamera += otherEvent;
        EventManager.current.OnEnablePivot += otherEvent;
        EventManager.current.OnEnableCrossSection += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
        EventManager.current.OnReset += EventManager_OnReset;
        EventManager.current.OnViewAnnotations += otherEvent;
        EventManager.current.OnAddAnnotations += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
    }


    public void EventManager_OnReset(object sender, EventArgs e){
        isEnabled = true;
        timeElapsed = 0.0f;
        startPos = Camera.main.gameObject.transform.position; 
        startRot = Camera.main.gameObject.transform.rotation;
        targetPos = CameraMovement.startPos;
        targetRot = CameraMovement.startRot;  
    }
    public void otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }
}
