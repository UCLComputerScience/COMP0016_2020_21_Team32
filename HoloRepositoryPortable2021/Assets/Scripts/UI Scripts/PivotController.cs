using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
///<summary>This class is attatched to the PivotController prefab to provide it with interactivity. It allows the user to adjust 
///the position of the point about which the camera rotates (represented by a sphere) using 3 sliders; one for each axis
///in 3D space.</summary>
public class PivotController : MonoBehaviour
{
    [SerializeField] const float MODEL_TO_PIVOT_RADIUS_RATIO = 20f; //ratio used to scale the size of the pivot based on the size of the model
    [SerializeField] const float X_SLIDER_MULTIPLIER = 1f;
    [SerializeField] const float Y_SLIDER_MULTIPLIER = 1f;
    [SerializeField] const float Z_SLIDER_MULTIPLER = 1f;
    [SerializeField] const float TARGET_OPACITY = 0.4f; //opacity to reduce the segments of the model to so the pivot can be seen through the model.
    public GameObject pivot;
    public GameObject axes;

    public Vector3 startPos;

    private Slider xPosSlider;
    private Slider yPosSlider;
    private Slider zPosSlider;
    private float startXPos;
    private float startYPos;
    private float startZPos;

    private Button confirmButton;
    private Button cancelButton;
    private Button resetButton;

    void Awake(){
        /*Initialise all interactive elements of the controller and add callbacks to their events*/
        xPosSlider = transform.Find("Pivot xPos").GetComponent<Slider>();
        yPosSlider = transform.Find("Pivot yPos").GetComponent<Slider>();
        zPosSlider = transform.Find("Pivot zPos ").GetComponent<Slider>();
        confirmButton = transform.Find("Confirm").GetComponent<Button>();
        cancelButton = transform.Find("Cancel").GetComponent<Button>();
        resetButton = transform.Find("Reset Pivot Position").GetComponent<Button>();

        confirmButton.onClick.AddListener(onConfirm);
        resetButton.onClick.AddListener(resetSlider);
        confirmButton.onClick.AddListener(onConfirm);
        cancelButton.onClick.AddListener(onCancel);

        /*Set the position and size of the pivot. Its size is determined by the radius of the sphere that bounds the mesh of the loaded model (ModelHandler.current.modelRadius)*/
        StartCoroutine(initialisePivot());
        StartCoroutine(initialiseSliders());

    }

    /*Automatically called when the gameobject the script is attached to is enabled. Reduces the opacity of the segments of the model (if they're above a certain threshold)
    so that the user can see the pivot they're controlling, as it will usually be within the model. Enable the UIBlocker, pivot and the axes.*/
    private IEnumerator initialisePivot(){
        yield return new WaitUntil(()=> ModelHandler.current.modelCentre != null);
        pivot.transform.position = ModelHandler.current.modelCentre;
        axes.transform.position = pivot.transform.position;
        float pRadius = ModelHandler.current.modelRadius / MODEL_TO_PIVOT_RADIUS_RATIO;
        pivot.transform.localScale = new Vector3(pRadius,pRadius,pRadius); 
    }
    private IEnumerator initialiseSliders(){
        yield return new WaitUntil(()=>ModelHandler.current.modelCentre != null);
        initialiseSlider(xPosSlider, changeXPos, X_SLIDER_MULTIPLIER, ModelHandler.current.modelCentre.x);
        initialiseSlider(yPosSlider, changeYPos, Y_SLIDER_MULTIPLIER, ModelHandler.current.modelCentre.y);
        initialiseSlider(zPosSlider, changeZPos, Z_SLIDER_MULTIPLER, ModelHandler.current.modelCentre.z);
        startXPos = xPosSlider.value = pivot.transform.position.x;
        startYPos = yPosSlider.value = pivot.transform.position.y;
        startZPos = zPosSlider.value = pivot.transform.position.z;
        startPos = new Vector3(startXPos, startYPos, startZPos);
    }
    void OnEnable(){
        MaterialAssigner.reduceOpacityAll(TARGET_OPACITY, ModelHandler.current.segments);
        EventManager.current.onEnableUIBlocker();
        pivot.SetActive(true);
        axes.SetActive(true);
    }

    /*Automatically called when the game object this script is attached to is disabled. Resets the opacities of the segments of the model to their values before the controller
    was enabled. Disable the UIBlocker, pivot, axes and ToolTip, and fire an OnEnableCamera event so the user immediately has control of the camera again*/
    void OnDisable(){
        MaterialAssigner.resetOpacities(ModelHandler.current.segments);
        EventManager.current.onDisableUIBlocker();
        axes.SetActive(false);
        pivot.SetActive(false);
        ToolTip.current.gameObject.SetActive(false);
        EventManager.current.onEnableCamera();
    }

    /*Passed as a callback to the onValueChanged event of the x position slider.*/
    private void changeXPos(float newXPos){
        pivot.transform.position = new Vector3(newXPos, pivot.transform.position.y, pivot.transform.position.z);
    }
    /*Passed as a callback to the onValueChanged event of the y position slider.*/
    private void changeYPos(float newYPos){
        pivot.transform.position = new Vector3(pivot.transform.position.x, newYPos, pivot.transform.position.z);
    }
    /*Passed as a callback to the onValueChanged event of the z position slider.*/
    private void changeZPos(float newZPos){
        pivot.transform.position = new Vector3(pivot.transform.position.x, pivot.transform.position.y, newZPos);
    }
    /*Resets the slider values and position of the pivot (to (0,0,0))*/
    private void resetSlider(){
        xPosSlider.value = startXPos;
        yPosSlider.value = startYPos;
        zPosSlider.value = startZPos;
        pivot.transform.position = startPos;
    }
    /*Passed as a callback to the onClick event of the confirm button. Reassigns the camera's target to the new position of the pivot and disables the controller.*/
    private void onConfirm(){
        pivot.SetActive(false);
        this.gameObject.SetActive(false);
    }

    /*Passed as a callback to the onClick event of the cancel button. Resets the slider and disables the controller*/
    private void onCancel(){
        resetSlider();
        pivot.SetActive(false);
        this.gameObject.SetActive(false);
    }
    /*Helper function that initialises the minimum and maximum values of a slider based on the radius of the model, and passes a callback to the
    onValue changed event of the slider*/
    private void initialiseSlider(Slider slider,  UnityAction<float> methodToCall, float multiplier, float modelCentreCoord){
        slider.onValueChanged.AddListener(methodToCall);
        slider.minValue = modelCentreCoord -ModelHandler.current.modelRadius * multiplier;
        slider.maxValue = modelCentreCoord + ModelHandler.current.modelRadius * multiplier;
    }
}
