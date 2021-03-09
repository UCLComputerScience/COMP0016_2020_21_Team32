using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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
    private Quaternion startRot = Quaternion.identity;
    // public Button confirmButton;
    // public Button cancelButton;


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
        cancelButton.onClick.AddListener(cancelSlice);
        cancelButton.onClick.AddListener(resetSlider);
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
    None of its code is executed until the model is loaded in and the radius of the sphere that bounds its mesh calculated (ModelHandler.modelRadius). 
    Using this radius, a maximum and minimum height that the plane can reach is calculated, and the
    minimum and maximum values of the slider that enables the y position of the plane to be changed are initialised to these values. */
    private IEnumerator initialisePlane(){
        yield return new WaitUntil(() => ModelHandler.modelRadius != 0); //wait until model is loaded
        startYPos = yPosSlider.value = yPosSlider.maxValue = maxPlaneHeight = ModelHandler.modelCentre.y + ModelHandler.modelRadius; //set max (and initial) value of slider
        yPosSlider.minValue = minPlaneHeight = ModelHandler.modelCentre.y - ModelHandler.modelRadius; //set min value of slider
        plane.transform.position = Vector3.up * maxPlaneHeight;//move the plane to the max position
        plane.transform.rotation = Quaternion.identity; //set the rotation of the plane to zero.
    }

    /*When the controller is enabled, the different colour shader is applied to the model. With this shader applied, 
    the volume of the model to be removed when the confirm button is pressed is coloured black. 
    */
    void OnEnable(){
        EventManager.current.onEnableUIBlocker();
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, differentColourShader);
    }
    /*When the controller is disabled, fire an onEnableCamera event so the user can start rotating the camera again immediately without having to click the icon.
    Also disable the tooltip that appears when the cancel button is hovered over.*/
    void OnDisable(){
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
    /*Passed as a callback to the onClick event of the reset button. When pressed, all slider values are re-initialised*/
    public void resetSlider(){
        yPosSlider.value = startYPos;
        xRotSlider.value = startXRot;
        yRotSlider.value = startYRot;
        zRotSlider.value = startZRot;
    }
    /*Passed as a callback to the onClick event of the confirm button. When pressed, the CrossSection shader is reapplied to all segments of the model. 
    This has the effect of removing all of the volume of the model that was coloured black before the button was pressed. The controller is
    also set to inactive so that the user can continue to use the UI and view/manipulate the cross section of the model.*/
    public void confirmSlice(){
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, crossSectionalShader);
        plane.SetActive(false);
        this.gameObject.SetActive(false);
    }
    /*Passed as a callback to the onClick event of the cancel button. When pressed, the position of the plane and slider values are reset and the
    CrossSection shader is reapplied to all segments of the model. This has the effect of viewing the model as normal again .
    The controller is set to inactive to return the user to the UI as normal.*/
    public void cancelSlice(){
        resetPlane();
        resetSlider();
        plane.SetActive(false);
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, crossSectionalShader);
        this.gameObject.SetActive(false);
    }


    /*Resets the position of the plane. It is done in a strange way, as simply attempting to reset its rotation once did not work.
    This is due to the way in which Unity represents the orientations of objects and I haven't been able to find a better way to solve the
    problem than this one.*/
    public void resetPlane(){
        for(int i = 0; i < 100; i++){
            plane.transform.position = new Vector3(0, maxPlaneHeight, 0);
            plane.transform.rotation = startRot;
        }
        StartCoroutine(resetPlaneHelper());
    }

    private IEnumerator resetPlaneHelper(){
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < 100; i++){
            plane.transform.position = new Vector3(0,maxPlaneHeight,0);
            plane.transform.rotation = startRot;
        }
    }
    /*Every frame, the material on the model is updated with the new position and normal of the plane. This is passed as input to the shader, which determines
    which parts of the model will be drawn to the screen*/
    void Update()
    {
        foreach(GameObject g in ModelHandler.segments){
            MaterialAssigner.updatePlanePos(g, plane);
        }
    }
}
