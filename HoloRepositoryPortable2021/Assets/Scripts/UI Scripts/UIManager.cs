using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

///<summary>This class manages the top level components of the UI and whether they are active or not.</summary>
public class UIManager : MonoBehaviour
{
   public GameObject UIBlocker;
   public GameObject controllerHandler; 
   public GameObject planeController;
   public GameObject pivotController;
   public GameObject colourPalette;
   public GameObject navigationBar;
   public GameObject segmentSelect;
   public GameObject opacitySlider;
   public GameObject logos;
   public GameObject annotationPin;
   public GameObject mainPage; 
   public GameObject dicomController;
   public GameObject settingsController;

    void Start(){
        subscribeToEvents();
    }
    private void subscribeToEvents(){
        EventManager.current.OnToggleColourPalette+=EventManager_onToggleColourPalette;
        EventManager.current.OnToggleLogos+=EventManager_onToggleLogos;
        EventManager.current.OnToggleNavigationBar+=EventManager_onToggleNavigationBar;
        EventManager.current.OnToggleOpacitySlider+=EventManager_onToggleOpacitySlider;
        EventManager.current.OnToggleSegmentSelect+=EventManager_onToggleSegmentSelect;
        EventManager.current.OnEnableUIBlocker+=EventManager_onEnableUIBlocker;
        EventManager.current.OnDisableUIBlocker+=EventManager_onDisableUIBlocker;
        EventManager.current.OnAddAnnotations+=EventManager_onAddAnnotation;
        EventManager.current.OnEnableCrossSection += EventManager_OnCrossSectionEnabled;
        EventManager.current.OnEnableDicom += EventManager_OnDICOMView;
        EventManager.current.OnEnablePivot += EventManager_OnChangePivot;
        EventManager.current.OnChangeSettings += EventManager_OnChangeSettings;
        EventManager.current.OnToggleFullScreen+=EventManager_OnToggleFullScreen;
    }
    public void EventManager_onToggleColourPalette(object o, EventArgs e){
        colourPalette.SetActive(!colourPalette.activeInHierarchy);
    }
    public void EventManager_onToggleNavigationBar(object o, EventArgs e){
        navigationBar.SetActive(!navigationBar.activeInHierarchy);
    }
    public void EventManager_onToggleLogos(object o, EventArgs e){
        logos.SetActive(!logos.activeInHierarchy);
    }
    public void EventManager_onToggleOpacitySlider(object o, EventArgs e){
        opacitySlider.SetActive(!opacitySlider.activeInHierarchy);
    }
    public void EventManager_onToggleSegmentSelect(object o, EventArgs e){
        segmentSelect.SetActive(!segmentSelect.activeInHierarchy);
    }
    public void EventManager_onEnableUIBlocker(object o, EventArgs e){
        UIBlocker.SetActive(true);
    }
    public void EventManager_onDisableUIBlocker(object o, EventArgs e){
        UIBlocker.SetActive(false);
    }
    public void EventManager_onAddAnnotation(object o, EventArgs e){
        annotationPin.SetActive(true);
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
    public void EventManager_OnToggleFullScreen(object sender, EventArgs e){
        mainPage.SetActive(!mainPage.activeInHierarchy);
    }

}
