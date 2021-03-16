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
    private const float CAMERA_TO_MODEL_RADIUS_RATIO = 350/178; //experimentally discovered value to move the camera in the z plane relative to the radius of the model loaded
    public static Vector3 startPos;
    public static Quaternion startRot;
    public static Vector3 displacement; 
    private Vector3 prevPosition;
    public GameObject pivot;
    private float cameraDistance;
    private float scrollSpeed; 
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
    }
    void Start() 
    {   
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
    /*Displaces the camera in the z axis*/
    private void zoom(){
        Camera.main.transform.position = pivot.transform.position;
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
        displacement -= new Vector3(0, 0, scrollAmount);
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
            zoom();
        }else if(Input.GetKey(KeyCode.H)){//Translate using keybinds.
            Camera.main.transform.position += Camera.main.transform.right * 1.0f;
            displacement += Vector3.right*1.0f;
        }else if (Input.GetKey(KeyCode.G)){
            Camera.main.transform.position -= Camera.main.transform.right * 1.0f;
            displacement -= Vector3.right*1.0f;
        }else if(Input.GetKey(KeyCode.Y)){
            Camera.main.transform.position += Camera.main.transform.up * 1.0f;
            displacement += Vector3.up*1.0f;
        }else if(Input.GetKey(KeyCode.B)){
            Camera.main.transform.position -= Camera.main.transform.up * 1.0f;
            displacement -= Vector3.up*1.0f;
        }
        
    }

    /*Coroutine that initialises the camera distance dynamically based on the radius of the sphere that bounds the renderer of the model loaded into the application, 
    so that models of any physical size can be viewed when loaded into the application.*/
    private IEnumerator setCameraDistance(){
        yield return new WaitUntil(() => ModelHandler.current.modelRadius != 0); //waits until the model has been loaded in - prevents nullReferencEexceptions being thrown
        cameraDistance = -CAMERA_TO_MODEL_RADIUS_RATIO * ModelHandler.current.modelRadius; //ratio * radius of renderer
        scrollSpeed = -cameraDistance;
        displacement = new Vector3(0f,0f,cameraDistance);
        Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Camera.main.transform.Translate(displacement);
        startPos = Camera.main.transform.position;
        startRot = Camera.main.transform.rotation;
    }

    public void EventManager_enableCamera(object sender, EventArgs e){
        isEnabled = true;
    }
    public void EventManager_onSelectAnnotation(object sender, EventArgsAnnotation e){
        Camera.main.transform.position = e.data.cameraCoordinates;
        Camera.main.transform.rotation = e.data.cameraRotation;
        displacement = e.data.cameraDisplacement;
    }

    /*camera is disabled whenever another event is received*/
    public void EventManager_otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }
    
    /*Reinitialise displacement whenever the reset button is pressed*/
    public void EventManager_resetPosition(object sender, EventArgs e){
        isEnabled = false;
        displacement = new Vector3(0f,0f,cameraDistance);
    }
}
 