using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

///<Summary>This class handles resetting the position of the camera via means of linear interpolation</summary>
public class CameraResetter : MonoBehaviour, IEventManagerListener
{
    public const float RESET_TIME = 2f;
    Vector3 startPos;
    Quaternion startRot;
    Vector3 targetPos;
    Quaternion targetRot;
    float timeElapsed;
    public bool isEnabled = false;


    /*Ensures this script is only enabled when the relevant event is received.*/
    public void subscribeToEvents(){
        EventManager.current.OnReset += EventManager_OnReset;
    }
    void Start(){
        subscribeToEvents();    
    }

    /*When the reset toggle is pressed in the navigation bar, the camera is linearly interpolated from its current position to its original position.
    The time taken for this reset to occur is determined by RESET_TIME*/
    void Update()
    {
        if(!isEnabled)return;
        timeElapsed +=Time.deltaTime; 
        float ratio = timeElapsed/RESET_TIME;
        Camera.main.gameObject.transform.position = Vector3.Lerp(startPos, targetPos, ratio);
        Camera.main.gameObject.transform.rotation = Quaternion.Lerp(startRot, targetRot, ratio);
        if(ratio >= 1){
            isEnabled = false;
            timeElapsed = 0;
            ratio = 0;
            EventManager.current.onEnableCamera(); //re-enable camera controls after the LERP.
            isEnabled = false;
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
}
