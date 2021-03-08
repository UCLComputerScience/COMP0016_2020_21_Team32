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
public class CameraMovement : MonoBehaviour
{
    public static Vector3 startPos;
    public static Quaternion startRot;
    private Vector3 prevPosition;
    private float cameraRatio = 350/178; //experimentally discovered value to move the camera in the z plane relative to the radius of the model loaded
    public static Transform target;
    private Vector3 xTranslationCache = Vector3.zero; //amount the camera has been translated in its local x axis
    private Vector3 yTranslationCache = Vector3.zero; //amount the camera has been translated in its local y axis
    public Vector3 displacement; 
    private float cameraDistance;
    private float scrollSpeed; 
    private bool isEnabled = true;
    [SerializeField] GameObject pivot;

    void Start() 
    {
        StartCoroutine(setCameraDistance()); //sets the position of the camera based on the size of the model loaded in 
        target = pivot.transform;
        subscribeToEvents();
        Camera.main.enabled =true;
        prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition); //used to determine the direction the camera should be rotated
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
    }
    void LateUpdate()
    {
        if(!isEnabled) return; //Camera is disabled when certain events are received

        if(Input.GetMouseButtonDown(0)){ //called on the frame when the left mouse button is clicked
            if(!EventSystem.current.IsPointerOverGameObject()){
                Camera.main.transform.Translate(displacement);
                Camera.main.transform.Translate(xTranslationCache); 
                Camera.main.transform.Translate(yTranslationCache);
                prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition); // cam position set to normalised version of screen coordinates 
            } 
        }
        if(Input.GetMouseButton(0)){ //called for as long as the left mouse button is held down
            if(!EventSystem.current.IsPointerOverGameObject()){ 
                Vector3 dir = prevPosition - Camera.main.ScreenToViewportPoint(Input.mousePosition); //direction to rotate the camera
                Camera.main.transform.position = target.transform.position; //set the camera's position to the target
                Camera.main.transform.Rotate(Vector3.right, dir.y *180); //rotate the camera based on dir
                Camera.main.transform.Rotate(Vector3.up, -dir.x * 180, Space.World);
                Camera.main.transform.Translate(displacement); //move the camera back in the z axis so its not directly on top of the target
                Camera.main.transform.Translate(xTranslationCache);
                Camera.main.transform.Translate(yTranslationCache);
                prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
        /*zoom in/out by changing the value of displacement whenever the scrollwheel is used*/
        }else if(Input.GetAxis("Mouse ScrollWheel") != 0){
            Camera.main.transform.position = target.transform.position;
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
            displacement -= new Vector3(0, 0, scrollAmount);
            Camera.main.transform.Translate(displacement);
            Camera.main.transform.Translate(xTranslationCache);
            Camera.main.transform.Translate(yTranslationCache);
        /*Translate using keybinds. The amount translated is cached and reapplied each frame, offsetting the camera from its target position whenever the user rotates*/
        }else if(Input.GetKey(KeyCode.H)){
            Camera.main.transform.position -= Camera.main.transform.right * 1.0f;
            xTranslationCache -= Vector3.right*1.0f;
        }else if (Input.GetKey(KeyCode.G)){
            Camera.main.transform.position += Camera.main.transform.right * 1.0f;
            xTranslationCache += Vector3.right*1.0f;
        }else if(Input.GetKey(KeyCode.Y)){
            Camera.main.transform.position -= Camera.main.transform.up * 1.0f;
            yTranslationCache -= Vector3.up*1.0f;
        }else if(Input.GetKey(KeyCode.B)){
            Camera.main.transform.position += Camera.main.transform.up * 1.0f;
            yTranslationCache += Vector3.up*1.0f;
        }
        
    }

    /*Coroutine that initialises the camera distance dynamically based on the radius of the sphere that bounds the renderer of the model loaded into the application. This enables 
    models of any physical size to be viewed when loaded into the application.*/
    private IEnumerator setCameraDistance(){
        yield return new WaitUntil(() => ModelHandler.organ.segments != null); //waits until the model has been loaded in - prevents nullReferencEexceptions being thrown
        cameraDistance = -cameraRatio * ModelHandler.modelRadius; //ratio * radius of renderer
        scrollSpeed = -cameraDistance;
        displacement = new Vector3(0f,0f,cameraDistance);
        Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Camera.main.transform.Translate(displacement);
        startPos = Camera.main.transform.position;
        startRot = Camera.main.transform.rotation;
        
    }
    /*Subscribes to certain events to determine when the camera controls should and shouldn't be enabled.*/
    private void subscribeToEvents(){
        EventManager.current.OnEnableCamera += EventManager_enableCamera;
        EventManager.current.OnEnablePivot += EventManager_otherEvent;
        EventManager.current.OnEnableCrossSection += EventManager_otherEvent;
        EventManager.current.OnReset += EventManager_resetPosition;
        EventManager.current.OnViewAnnotations += EventManager_onViewAnnotation;
        EventManager.current.OnAddAnnotations += EventManager_otherEvent;
    }
    public void EventManager_enableCamera(object sender, EventArgs e){
        isEnabled = true;
    }


    /*camera is disabled whenever another event is received*/
    public void EventManager_otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }
    public void EventManager_onViewAnnotation(object sender, EventArgs e){
        isEnabled = false;
        // xTranslationCache = Vector3.zero;
        // yTranslationCache = Vector3.zero;
        // displacement.z = cameraDistance;
    }

    /*Reinitialise displacement and caches whenever the reset button is pressed*/
    public void EventManager_resetPosition(object sender, EventArgs e){
        isEnabled = false;
        xTranslationCache = Vector3.zero;
        yTranslationCache = Vector3.zero;
        displacement.z =  cameraDistance;
    }
}
 