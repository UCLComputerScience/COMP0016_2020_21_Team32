using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System;
///<summary>This class initialises and manages the state of all the toggles on the navigation bar.
///Adds listeners to these toggles and ensures that the correct event is fired when a toggle is selected. This ensures 
///that the buttons trigger the correct scripts to be enabled.
///Also ensures that the enableCamera button is selected if nothing else is. For example, once an annotation has
///been confirmed by the user, the annotation prefab is disabled and an enableCamera event is fired. This class listens
///for that event and changes the state of the enableCamera toggle accordingly.
///</summary>
public class NavigationBarManager : MonoBehaviour, IEventManagerListener
{
    [SerializeField] Toggle enableCamera;
    [SerializeField] Toggle enablePivot;
    [SerializeField] Toggle enableCrossSection;
    [SerializeField] Toggle enableDicom;
    [SerializeField] Toggle reset;
    [SerializeField] Toggle viewAnnotation;
    [SerializeField] Toggle addAnnotation;

    public void subscribeToEvents(){
        EventManager.current.OnEnableCamera += OnEnableCamera_EventManager;
    }
    void Awake()
    {
        initButton(enableCamera, EventManager.current.onEnableCamera);
        initButton(enablePivot, EventManager.current.onEnablePivot);
        initButton(enableDicom, EventManager.current.onEnableDicom);
        initButton(enableCrossSection, EventManager.current.onEnableCrossSection);
        initButton(reset, EventManager.current.onReset);
        initButton(viewAnnotation, EventManager.current.onViewAnnotations);
        initButton(addAnnotation, EventManager.current.onAddAnnotations);
        subscribeToEvents();
    }

    /*Initialises the listener of the toggle such that it executes the callback() function when selected*/
    private void initButton(Toggle toggle, Action callback){
        toggle.onValueChanged.AddListener((bool isOn) => {
            if(isOn) callback();
        });
    }

/*Sets the enableCamera button to be selected whenever an enableCamera event is fired*/
    public void OnEnableCamera_EventManager(object sender, EventArgs e){
        EventSystem.current.SetSelectedGameObject(enableCamera.gameObject);
        enableCamera.isOn = true;
    }
}
