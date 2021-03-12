using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
///<summary>This class provides the settings controller with interactivity, allowing other UI elements to be enabled/disabled and allowing the
///colour of the background to be changed.</summary>
public class SettingsController : MonoBehaviour
{   
   private readonly String startSkyBoxColour = "3C3C3C";
   private Toggle colourPaletteToggle;
   private Toggle navigationBarToggle;
   private Toggle segmentSelectToggle;
   private Toggle logoToggle;
   private Toggle opacitySliderToggle;
   private Slider backgroundColourSlider;
   private Color skyBoxColour;
   private Button hideButton;
   private Button about;
   private GameObject aboutPanel;
   private Button hideAbout;
   void Awake(){
       /*initialise variables*/
       skyBoxColour = MaterialAssigner.hexToColour(startSkyBoxColour);
       RenderSettings.skybox.SetColor("_Tint", skyBoxColour);
       colourPaletteToggle = this.transform.Find("Colour Palette Toggle").GetComponent<Toggle>();
       navigationBarToggle = this.transform.Find("Navigation Bar Toggle").GetComponent<Toggle>();
       segmentSelectToggle = this.transform.Find("Segment Select Toggle").GetComponent<Toggle>();
       logoToggle = this.transform.Find("Logo Toggle").GetComponent<Toggle>();
       opacitySliderToggle = this.transform.Find("Opacity Slider Toggle").GetComponent<Toggle>();
       backgroundColourSlider = this.transform.Find("Background Brightness Slider").GetComponent<Slider>();
       backgroundColourSlider.minValue = 0;
       backgroundColourSlider.maxValue = 1;
       backgroundColourSlider.value = 1;
       hideButton = this.transform.Find("Hide Settings").GetComponent<Button>();
       about = this.transform.Find("Open about page").GetComponent<Button>();
       aboutPanel = this.transform.Find("About Page").gameObject;
       hideAbout = aboutPanel.transform.Find("Hide About").GetComponent<Button>();
       
       /*Add callback actions to the events of each interactable element*/
       initToggle(colourPaletteToggle, EventManager.current.onToggleColourPalette);
       initToggle(navigationBarToggle, EventManager.current.onToggleNavigationBar);
       initToggle(segmentSelectToggle, EventManager.current.onToggleSegmentSelect);
       initToggle(logoToggle, EventManager.current.onToggleLogos);
       initToggle(opacitySliderToggle, EventManager.current.onToggleOpacitySlider);
       backgroundColourSlider.onValueChanged.AddListener(adjustBackgroundBrightness);
       hideButton.onClick.AddListener(hide);
       about.onClick.AddListener(enablePanel);
       hideAbout.onClick.AddListener(disablePanel);
   }
    private void adjustBackgroundBrightness(float tint){
         Color newCol = skyBoxColour * tint;
         RenderSettings.skybox.SetColor("_Tint",newCol);
    }
    private void hide(){
        this.gameObject.SetActive(false);
        ToolTip.current.gameObject.SetActive(false);
        EventManager.current.onEnableCamera();
    }
    private void enablePanel(){
        aboutPanel.SetActive(true);
    }
    private void disablePanel(){
        aboutPanel.SetActive(false);
        ToolTip.current.gameObject.SetActive(false);
    }
    void OnDestroy(){
       skyBoxColour = MaterialAssigner.hexToColour(startSkyBoxColour);
       RenderSettings.skybox.SetColor("_Tint", skyBoxColour);
    }
    
    /*Helper function to initialise a toggle with a callback*/
    private void initToggle(Toggle toggle, Action myMethodName){
        toggle.onValueChanged.AddListener((bool isOn) => {
            myMethodName();
        });
    }




}
