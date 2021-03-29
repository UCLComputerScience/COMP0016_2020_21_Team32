using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

///<summary>This class allows the user to rotate the camera about the model by clicking and dragging with the mouse. 
/// Also allows the user to zoom in and out by using the scroll wheel/pinching and dragging the trackpad.
///
///Tutorial used to help make this class: https://www.youtube.com/watch?v=rDJOilo4Xrg&t=306s
///</summary>
public class CameraController : MonoBehaviour, IEventManagerListener
{
    private const float CAMERA_TO_MODEL_RADIUS_RATIO = 350/178; 
    private const float DISPLACEMENT_MULTIPLIER = 0.01f; 
    public static Vector3 startPos;
    public static Quaternion startRot;
    public static Vector3 displacement; 
    private Vector3 prevPosition;
    public GameObject pivot;
    private float cameraDistance;
    private float scrollSpeed;
    private float displacementMagnitude; 
    public bool isEnabled = true;

    /*Subscribes to certain events to determine when the camera controls should and shouldn't be enabled.*/
    public void subscribeToEvents(){
        EventManager.current.OnEnableCamera += EventManager_enableCamera;
        EventManager.current.OnEnablePivot += EventManager_otherEvent;
        EventManager.current.OnEnableCrossSection += EventManager_otherEvent;
        EventManager.current.OnReset += EventManager_resetPosition;
        EventManager.current.OnViewAnnotations += EventManager_otherEvent;
        EventManager.current.OnAddAnnotations += EventManager_otherEvent;
        EventManager.current.OnSelectAnnotation += EventManager_onSelectAnnotation;
        EventManager.current.OnZoomIn += EventManager_onZoomIn;
        EventManager.current.OnZoomOut += EventManager_onZoomOut;
    }
    void Start(){   
        StartCoroutine(setCameraDistance()); //sets the position of the camera based on the size of the model loaded in 
        subscribeToEvents();
        Camera.main.enabled =true;
        prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition); //used to determine the direction the camera should be rotated
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
    }

    private void startRotation(){
        Camera.main.transform.Translate(displacement);
        prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }

    /*Uses the direction moved by the pointer between frames to determine how to rotate the camera about the pivot*/
    private void rotateCamera(){ 
        Vector3 dir = prevPosition - Camera.main.ScreenToViewportPoint(Input.mousePosition); //direction to rotate the camera
        Camera.main.transform.position = pivot.transform.position; 
        Camera.main.transform.Rotate(Vector3.right, dir.y *180); //rotate the camera about its local x axis
        Camera.main.transform.Rotate(Vector3.up, -dir.x * 180, Space.World);//rotate the camera about the world y axis 
        Camera.main.transform.Translate(displacement); 
        prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
    /*Displaces the camera by the vector dir * magnitude*/
    private void move(Vector3 dir, float magnitude){
        Camera.main.transform.position = pivot.transform.position;
        displacement += dir * magnitude;
        Camera.main.transform.Translate(displacement);
    }
    void LateUpdate()
    {
        if(!isEnabled) return; //Camera is disabled when certain events are received
        if(!EventSystem.current.IsPointerOverGameObject()){ //if the pointer is not over a UI element
            if(Input.GetMouseButtonDown(0)){
                startRotation();
            }
            if(Input.GetMouseButton(0)){
                rotateCamera();
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") != 0){
            move(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"), scrollSpeed);
        }else if (Input.GetKey(KeyCode.H)){
            move(Vector3.right, displacementMagnitude);
        }else if(Input.GetKey(KeyCode.G)){
            move(Vector3.left, displacementMagnitude);
        }else if(Input.GetKey(KeyCode.Y)){
            move(Vector3.up, displacementMagnitude);
        }else if(Input.GetKey(KeyCode.B)){
            move(Vector3.down, displacementMagnitude);
        }else if(Input.GetKey(KeyCode.W)){
            move(Vector3.forward, displacementMagnitude);
        }else if(Input.GetKey(KeyCode.S)){
            move(Vector3.back, displacementMagnitude);
        }
    }

    /*Coroutine that initialises the camera distance dynamically based on the radius of the model loaded into the application, 
    so that models of any physical size can be viewed when loaded into the application.*/
    private IEnumerator setCameraDistance(){
        yield return new WaitUntil(() => ModelHandler.current.modelRadius != 0); //waits until the model has been loaded in - prevents nullReferencEexceptions being thrown
        cameraDistance = -CAMERA_TO_MODEL_RADIUS_RATIO * ModelHandler.current.modelRadius; //ratio * radius of model
        displacementMagnitude = DISPLACEMENT_MULTIPLIER * ModelHandler.current.modelRadius;
        scrollSpeed = -cameraDistance;

        
        pivot.transform.position = ModelHandler.current.modelCentre;
    
        Camera.main.transform.position = pivot.transform.position;
        displacement = new Vector3(0f,0f,cameraDistance);
        Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Camera.main.transform.Translate(displacement);
        startPos = Camera.main.transform.position;
        startRot = Camera.main.transform.rotation;
        
    }

    /*Allow the camera to be rotated again*/
    public void EventManager_enableCamera(object sender, EventArgs e){
        isEnabled = true;
    }

    /*Restore the state of the camera encapsulated by the received AnnotationData object*/
    public void EventManager_onSelectAnnotation(object sender, EventArgsAnnotation e){
        Camera.main.transform.position = e.data.cameraCoordinates;
        Camera.main.transform.rotation = e.data.cameraRotation;
        displacement = e.data.cameraDisplacement;
    }    
    /*Reinitialise displacement whenever the reset button is pressed*/
    public void EventManager_resetPosition(object sender, EventArgs e){
        isEnabled = false;
        displacement = new Vector3(0f,0f,cameraDistance);
    }

    /*Zoom in on button press*/
    public void EventManager_onZoomIn(object sender, EventArgs e){
        move(Vector3.forward, displacementMagnitude);
    }
    /*Zoom out on button press*/
    public void EventManager_onZoomOut(object sender, EventArgs e){
        move(Vector3.back, displacementMagnitude);
    }
    
    /*camera is disabled whenever another event is received*/
    public void EventManager_otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }
}
 