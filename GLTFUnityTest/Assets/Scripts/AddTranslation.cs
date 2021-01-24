using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AddTranslation : MonoBehaviour
{
    public LineRenderer xAxis;
    public LineRenderer yAxis;

    public LineRenderer zAxis;

    [SerializeField] GameObject translate;
    
    bool isEnabled = false;

    void Start()
    {
        subscribeToEvents();
        xAxis = new LineRenderer();
        yAxis = new LineRenderer();
        zAxis = new LineRenderer();
        translate.SetActive(false);
        
        
    }


    // Update is called once per frame
    void Update()
    {
        if(!isEnabled)return;

        
    }
    public void subscribeToEvents(){
        SelectionManager.current.onCameraButtonPressed += otherEvent;
        SelectionManager.current.onTButtonPressed += SelectionManager_OnTButtonPressed;
        SelectionManager.current.onRButtonPressed += otherEvent;
        SelectionManager.current.onReButtonPressed += otherEvent;
        SelectionManager.current.onVAnnotationButtonPressed += otherEvent;
        SelectionManager.current.onAnnotationButton += otherEvent;
    }

    private void SelectionManager_OnTButtonPressed(object sender, EventArgs e){
        print("Here!");
        translate.SetActive(true);
        translate.transform.localScale = new Vector3(10, 10, 10);
    }
    private void otherEvent(object sender, EventArgs e){
        translate.SetActive(false);
        isEnabled = false;
    }

}
