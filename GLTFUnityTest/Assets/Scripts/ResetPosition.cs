using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        SelectionManager.current.onCameraButtonPressed += otherEvent;
        SelectionManager.current.onTButtonPressed += otherEvent;
        SelectionManager.current.onRButtonPressed += otherEvent;
        SelectionManager.current.onReButtonPressed += SelectionMangager_onReButtonPressed;
        SelectionManager.current.onVAnnotationButtonPressed += otherEvent;
        SelectionManager.current.onAnnotationButton += otherEvent;

        targetPos = CameraMovement.startPosition;
        targetRot = CameraMovement.startRotation;
        timeElapsed = 0.0f;    
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEnabled)return;
        print("Alright let's elapse some mother. fucking. TIIIMME");
        timeElapsed +=Time.deltaTime; 
        float ratio = timeElapsed/travelTime;
        Camera.main.gameObject.transform.position = Vector3.Lerp(startPos, targetPos, ratio);
        Camera.main.gameObject.transform.rotation = Quaternion.Lerp(startRot, targetRot, ratio);
        if(ratio >= 1){
            isEnabled = false;
            timeElapsed = 0;
        }
    }

    public void SelectionMangager_onReButtonPressed(object sender, EventArgs e){
        print("I am being received");
        isEnabled = true;
        startPos = Camera.main.gameObject.transform.position; 
        startRot = Camera.main.gameObject.transform.rotation;  
    }
    public void otherEvent(object sender, EventArgs e){
        print("The other guy is being received");
        isEnabled = false;
    }
}
