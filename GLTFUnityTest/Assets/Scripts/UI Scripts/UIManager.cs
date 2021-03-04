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
   private List<GameObject> UIElements;
   private GameObject planeController;
   private GameObject pivotController;
   private GameObject DICOMController;
   private GameObject SettingsController;
   private GameObject colourPalette;
   private GameObject navigationBar;
   private GameObject segmentSelect;
   private GameObject opacitySlider;
   private GameObject logos;
   private Toggle fullScreen;
   private Button settings;
   [SerializeField]private GameObject annotationPin;
   private GameObject mainPage; 

    void Awake(){
       mainPage = GameObject.Find("Main Page");
       colourPalette = GameObject.Find("Colour Palette");
       navigationBar = GameObject.Find("Navigation Bar");
       segmentSelect = GameObject.Find("Toggle Selected Segment");
       opacitySlider = GameObject.Find("Opacity Slider");
       logos = GameObject.Find("Logos");
       settings = GameObject.Find("Settings").GetComponent<Button>();
       fullScreen = GameObject.Find("Toggle full screen").GetComponent<Toggle>();
       UIElements = GetComponentsInChildren<Transform>().Select((t) => t.gameObject).ToList<GameObject>();
       
       settings.onClick.AddListener(EventManager.current.onChangeSettings);
       fullScreen.onValueChanged.AddListener(toggleFullScreen);
    }

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
    public void toggleFullScreen(bool isOn){
        mainPage.SetActive(isOn);
    } 
}
