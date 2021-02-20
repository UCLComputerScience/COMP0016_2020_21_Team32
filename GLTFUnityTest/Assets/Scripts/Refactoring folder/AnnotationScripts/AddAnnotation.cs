using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AddAnnotation : MonoBehaviour
{
    [SerializeField] Annotation annotation;
    [SerializeField] GameObject annotationPin;
    String title;
    void Start()
    {
        annotation.hide();
        annotationPin.SetActive(false);
        subscribeToEvents();
    }
    private void subscribeToEvents(){
        EventManager.current.OnAddAnnotations += EventManager_OnAddAnnotation;
        EventManager.current.OnEnableCamera += otherEvent;
        EventManager.current.OnEnablePivot += otherEvent;
        EventManager.current.OnEnableCrossSection += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
        EventManager.current.OnReset += otherEvent;
        EventManager.current.OnViewAnnotations += otherEvent;
    }

    public void EventManager_OnAddAnnotation(object sender, EventArgs e){
        annotationPin.SetActive(true); 
    }
    public void otherEvent(object sender, EventArgs e){
        annotationPin.SetActive(false);
    }

}
