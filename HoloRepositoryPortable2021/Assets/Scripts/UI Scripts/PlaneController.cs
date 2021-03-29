using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
///<summary> This class is attatched as a component to the PlaneController prefab, and allows the user to create cross sectional views of the 
///model they're viewing by controlling the position of a plane. The position of this plane relative to the model determines which of the polygons
///making up the model should be drawn to the screen by the GPU.
///</summary>
public class PlaneController : MonoBehaviour
{
    public GameObject plane;
    public Shader crossSectionalShader;
    public Shader differentColourShader;
    private Slider yPosSlider;
    private Slider xRotSlider;
    private Slider yRotSlider;
    private Slider zRotSlider;
    private float startYPos;
    private float startXRot;
    private float prevXRot;
    private float startYRot;
    private float prevYRot;
    private float startZRot;
    private float prevZRot;
    private Button resetButton;
    private Button confirmButton;
    private Button cancelButton;
    private float maxPlaneHeight;
    private float minPlaneHeight;

/*Initialise all interactable elements attatched to the PlaneController with callback actions, and initialise the position of the plane*/
    void Awake(){
        crossSectionalShader = Shader.Find("Custom/Clipping");
        differentColourShader = Shader.Find("Custom/DifferentColour");
        yPosSlider = transform.Find("Plane yPos").GetComponent<Slider>();
        xRotSlider = transform.Find("Plane xRot").GetComponent<Slider>();
        yRotSlider = transform.Find("Plane yRot").GetComponent<Slider>();
        zRotSlider = transform.Find("Plane zRot").GetComponent<Slider>();
        resetButton = transform.Find("Reset Plane Position").GetComponent<Button>();
        confirmButton = transform.Find("Confirm").GetComponent<Button>();
        cancelButton = transform.Find("Cancel").GetComponent<Button>();

        confirmButton.onClick.AddListener(confirmSlice);

        cancelButton.onClick.AddListener(resetSlider);
        cancelButton.onClick.AddListener(resetPlane);
        cancelButton.onClick.AddListener(cancelSlice);
        
        resetButton.onClick.AddListener(resetSlider);
        resetButton.onClick.AddListener(resetPlane);

        yPosSlider.onValueChanged.AddListener(changeYPos);
        
        xRotSlider.onValueChanged.AddListener(changeXRot);
        yRotSlider.onValueChanged.AddListener(changeYRot);
        zRotSlider.onValueChanged.AddListener(changeZRot);

        prevXRot = startXRot = xRotSlider.value = 0;
        prevYRot = startYRot = yRotSlider.value = 0;
        prevZRot = startZRot = zRotSlider.value = 0;

        StartCoroutine(initialisePlane());
    }

    /*This coroutine enables this controller to be used on any model regardless of the size of the model. 
    None of its code is executed until the model is loaded in and its radius (ModelHandler.current.modelRadius) is set. 
    Using this radius, a maximum and minimum height that the plane can reach is calculated, and the
    minimum and maximum values of the slider that enable the y position of the plane to be changed are initialised to these values. */
    private IEnumerator initialisePlane(){
        yield return new WaitUntil(() => ModelHandler.current.modelRadius != 0); //wait until model is loaded
        startYPos = yPosSlider.value = yPosSlider.maxValue = maxPlaneHeight = ModelHandler.current.modelCentre.y + ModelHandler.current.modelRadius; //set max (and initial) value of slider
        yPosSlider.minValue = minPlaneHeight = ModelHandler.current.modelCentre.y - ModelHandler.current.modelRadius; //set min value of slider
        plane.transform.position = ModelHandler.current.modelCentre + Vector3.up * maxPlaneHeight;
        plane.transform.rotation = Quaternion.identity; //set the rotation of the plane to zero.
    }

    /*When the controller is enabled, the "differentColour" shader is applied to the model. With this shader applied, 
    the volume of the model to be removed when the confirm button is pressed is coloured black. 
    */
    void OnEnable(){
        EventManager.current.onEnableUIBlocker();
        MaterialAssigner.assignToAllChildren(plane, ModelHandler.current.segments, differentColourShader);
    }
    /*When the controller is disabled, an onEnableCamera event is fired so the user can start rotating the camera again immediately without having to click the icon.
    The crossSectionalShader is also reapplied to create the cross sectional view (or not, if the plane is above the model).
    Also disable the tooltip that appears when the cancel button is hovered over.*/
    void OnDisable(){
        MaterialAssigner.assignToAllChildren(plane, ModelHandler.current.segments, crossSectionalShader);
        EventManager.current.onDisableUIBlocker();
        EventManager.current.onEnableCamera();
        ToolTip.current.gameObject.SetActive(false);
    }
    /*Passed as a callback to the onValueChanged event of yPosSlider. Sets the y position of the plane to newYPos when the slider is moved.
    The position of the slider determines newYPos*/
    public void changeYPos(float newYPos){
        plane.transform.position = new Vector3(plane.transform.position.x, newYPos, plane.transform.position.z);
    }
    
    /*Passed as a callback to the onValueChanged event of xRotSlider. Rotates the plane about the x axis when the slider is moved.*/
    public void changeXRot(float newXRot){
        float delta = newXRot - this.prevXRot;
        this.plane.transform.Rotate(Vector3.right * delta);
        this.prevXRot = newXRot;
    }

    /*Passed as a callback to the onValueChanged event of yRotSlider. Rotates the slider about the y axis when the slider is moved.*/
    public void changeYRot(float newYRot){
        plane.transform.rotation = Quaternion.Euler(plane.transform.eulerAngles.x, newYRot, plane.transform.eulerAngles.z);
    }

    /*Passed as a callback to the onValueChanged event of zRotSlider. Rotates the slider about the z axis when the slider is moved*/
    public void changeZRot(float newZRot){
        float delta = newZRot - this.prevZRot;
        this.plane.transform.Rotate(Vector3.forward * delta);
        this.prevZRot = newZRot;
    }
    /*Both functions below are passed as a callback to the onClick event of the reset button. When pressed, all slider values are re-initialised 
    and the plane's position and rotation is reset.*/
    public void resetSlider(){
        yPosSlider.value = startYPos;
        xRotSlider.value = startXRot;
        yRotSlider.value = startYRot;
        zRotSlider.value = startZRot;
    }
    public void resetPlane(){
        plane.transform.position = ModelHandler.current.modelCentre + Vector3.up * maxPlaneHeight;
        plane.transform.localRotation = Quaternion.Euler(0f,0f,0f);
    }
    /*Passed as a callback to the onClick event of the confirm button. When pressed, the controller is disabled so the CrossSectional shader is reapplied to all #
    segments of the model.*/
    public void confirmSlice(){
        plane.SetActive(false);
        this.gameObject.SetActive(false);
    }
    /*Passed as a callback to the onClick event of the cancel button. When pressed, the position of the plane and slider values are reset and the
    controller is disabled. This has the effect of viewing the model as normal again .
    The controller is set to inactive to return the user to the UI as normal.*/
    public void cancelSlice(){
        plane.SetActive(false);
        this.gameObject.SetActive(false);
    }



    /*Sets the correct slider position values for the position of the plane stored in the annotation being viewed.*/
    /*Every frame, the material on the model is updated with the new position and normal of the plane. This is passed as input to the shader, which determines
    which parts of the model will be drawn to the screen*/
    void Update()
    {
        foreach(GameObject g in ModelHandler.current.segments){
            MaterialAssigner.updatePlanePos(g, plane);
        }
    }

}
