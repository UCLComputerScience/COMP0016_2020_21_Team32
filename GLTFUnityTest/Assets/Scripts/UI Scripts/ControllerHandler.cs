using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

///<summary>
///This class maintains a reference to all the controllers in the UI and activates them when the appropriate event is received.
///Attatched to an empty gameObject.
///</summary>

public class ControllerHandler : MonoBehaviour
{
    private GameObject pivotController;
    private GameObject planeController;
    private GameObject dicomController;
    private GameObject settingsController;
    void Awake(){
        pivotController = this.transform.Find("Pivot Controller").gameObject;
        planeController = this.transform.Find("Plane Controller").gameObject;
        dicomController = this.transform.Find("Dicom Controller").gameObject;
        settingsController = this.transform.Find("Settings Controller").gameObject;
    }
    void Start(){
        subscribeToEvents();
    }
    ///<summary>
    ///Subscribes to the relevant events defined in the EventManager class
    ///</summary>
    private void subscribeToEvents(){
        EventManager.current.OnEnableCrossSection += EventManager_OnCrossSectionEnabled;
        EventManager.current.OnEnableDicom += EventManager_OnDICOMView;
        EventManager.current.OnEnablePivot += EventManager_OnChangePivot;
        EventManager.current.OnChangeSettings += EventManager_OnChangeSettings;
    }

    //Enables the pivot controller
    public void EventManager_OnChangePivot(object sender, EventArgs e){ //enables the pivot controller
        pivotController.SetActive(true);
    }
    //Enables the plane controller
    public void EventManager_OnCrossSectionEnabled(object sender, EventArgs e){ 
        planeController.SetActive(true);
    }
    //Enables the dicom controller
    public void EventManager_OnDICOMView(object sender, EventArgs e){ //enables the DICOM controller
        dicomController.SetActive(true);
    }
    //Enables the settings controller
    public void EventManager_OnChangeSettings(object sender, EventArgs e){ //enables the settings controller
        settingsController.SetActive(true);
    } 

}
