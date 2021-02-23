using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*Class listens for events and enables the relevant controller*/

public class ControllerHandler : MonoBehaviour
{
    [SerializeField] GameObject pivotController;
    [SerializeField] GameObject planeController;
    [SerializeField] GameObject dicomController;

    void Start(){
        subscribeToEvents();
    }
    private void subscribeToEvents(){
        EventManager.current.OnEnableCrossSection += EventManager_OnCrossSectionEnabled;
        EventManager.current.OnEnableDicom += EventManager_OnDICOMView;
        EventManager.current.OnEnablePivot += EventManager_OnChangePivot;
    }
    public void EventManager_OnChangePivot(object sender, EventArgs e){
        pivotController.SetActive(true);
    }
    public void EventManager_OnCrossSectionEnabled(object sender, EventArgs e){ 
        planeController.SetActive(true);
    }
    public void EventManager_OnDICOMView(object sender, EventArgs e){
        dicomController.SetActive(true);
    } 

    // private void EventManager_DicomView(object sender, EventArgs e){
    //     Debug.Log("Hey hey hey");
    //     var extension = new [] {new ExtensionFilter("DICOM", "dcm")};
    //     paths = StandaloneFileBrowser.OpenFilePanel("Select one or multiple dcm files", "", extension, true);
    //     foreach(String path in paths){
    //         images.Add(new DicomImage(path).RenderImage().AsTexture2D());
    //     }
    //     image.texture = images[0];
    //     Debug.Log("This is the width of the first image: "+ images[0].width);
    //     StartCoroutine(initialiseSliderMaxValue());
    // }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
