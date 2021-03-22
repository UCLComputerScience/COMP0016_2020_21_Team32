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
       colourPaletteToggle = this.transform.Find("Toggle Colour Palette").GetComponent<Toggle>();
       navigationBarToggle = this.transform.Find("Toggle Navigation Bar").GetComponent<Toggle>();
       segmentSelectToggle = this.transform.Find("Toggle Segment Select").GetComponent<Toggle>();
       logoToggle = this.transform.Find("Toggle Logos").GetComponent<Toggle>();
       opacitySliderToggle = this.transform.Find("Toggle Opacity Slider").GetComponent<Toggle>();
       backgroundColourSlider = this.transform.Find("Background Brightness Slider").GetComponent<Slider>();
       backgroundColourSlider.minValue = 0;
       backgroundColourSlider.maxValue = 1;
       backgroundColourSlider.value = 1;
       hideButton = this.transform.Find("Hide Settings").GetComponent<Button>();
       about = this.transform.Find("Open About Page").GetComponent<Button>();
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

   /*Passed as a callback action to the backgroundColour slider. Adjusts the colour of the background by linearly interpolating
   between black and white.*/
    private void adjustBackgroundBrightness(float tint){
        float rgbVal = Mathf.Lerp(0, 1, tint);
        Camera.main.backgroundColor = new Color(rgbVal, rgbVal,rgbVal, 1);
    }
    /*Passed as a callback to the hideButton. Hides this panel.*/
    private void hide(){
        this.gameObject.SetActive(false);
        ToolTip.current.gameObject.SetActive(false);
        EventManager.current.onEnableCamera();
    }
    /*Passed as a callback to the about button. Enables the about panel.*/
    private void enablePanel(){
        aboutPanel.SetActive(true);
    }
    /*Passed as a callback to the hideAbout button. Disables the about panel.*/
    private void disablePanel(){
        aboutPanel.SetActive(false);
        ToolTip.current.gameObject.SetActive(false);
    }
    
    /*Helper function to initialise a toggle with a callback*/
    private void initToggle(Toggle toggle, Action myMethodName){
        toggle.onValueChanged.AddListener((bool isOn) => {
            myMethodName();
        });
    }




}
