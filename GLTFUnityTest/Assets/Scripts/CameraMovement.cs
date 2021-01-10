using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CameraMovement : MonoBehaviour
{
    public static Vector3 startPosition = new Vector3(864.715f, 198.6647f, -1475.485f);
    public static Quaternion startRotation = Quaternion.Euler(14.21f, -14.114f, 0f);
    public Camera cam;
    private Vector3 prevPosition;

    public Transform startTransform;
    public Transform target;
    private Vector3 displacement; 
    private Vector3 dirVec;
    private float cameraDistance = -250f;
    private float scrollSpeed = 250f;
    private bool isEnabled = true;


    void Start() 
    {

        SelectionManager.current.onCameraButtonPressed += SelectionMangager_onCameraButtonPressed;
        SelectionManager.current.onTButtonPressed += otherEvent;
        SelectionManager.current.onRButtonPressed += otherEvent;
        SelectionManager.current.onReButtonPressed += SelectionManager_onReButtonPressed;
        SelectionManager.current.onVAnnotationButtonPressed += otherEvent;
        SelectionManager.current.onAnnotationButton += otherEvent;


        displacement = new Vector3(0, 0, cameraDistance);
        dirVec = new Vector3();
        startTransform = cam.transform;
        cam.transform.position = startPosition;
        cam.transform.rotation = startRotation;
        prevPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        cam.transparencySortMode = TransparencySortMode.Orthographic;
    }
    public void SelectionMangager_onCameraButtonPressed(object sender, EventArgs e){
        isEnabled = true;
        Debug.Log(isEnabled);
    }

    public void SelectionManager_onReButtonPressed(object sender, EventArgs e){
        prevPosition = startPosition;
        displacement = new Vector3(0, 0, cameraDistance);
        dirVec = new Vector3();
        isEnabled = true; // should really wait until the camera has finished its Slerp
        Debug.Log(isEnabled);
    }
    public void otherEvent(object sender, EventArgs e){
        print("The other guy is being fired");
        isEnabled = false;
    }
    //WORKING
    //Zoom controls inverted and still a bit slow but other than that working nicely.
    
    // Update is called once per frame

    void LateUpdate()
    {
        if(!isEnabled) return;
        //target.transform.position = cam.ScreenToViewportPoint(Input.mousePosition);
        

        if(Input.GetMouseButtonDown(0)){ 
            // if(!EventSystem.current.IsPointerOverGameObject()){
                prevPosition = cam.ScreenToViewportPoint(Input.mousePosition); // cam position set to normalised version of mouse coord
            // }
        }
        if(Input.GetMouseButton(0)){
            if(!EventSystem.current.IsPointerOverGameObject()){ //Ensures camera doesn't move when interacting with UI (THIS IS STILL BUGGY)
                Vector3 dir = prevPosition - cam.ScreenToViewportPoint(Input.mousePosition);

                cam.transform.position = target.transform.position;

                cam.transform.Rotate(new Vector3(1f, 0f, 0f), dir.y *180);
                cam.transform.Rotate(new Vector3(0f, 1f, 0f), -dir.x * 180, Space.World);
                cam.transform.Translate(displacement);
                prevPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }

        }else if(Input.GetAxis("Mouse ScrollWheel") != 0){
            cam.transform.position = target.transform.position;
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
            displacement -= new Vector3(0, 0, scrollAmount);
            print(displacement);
            cam.transform.Translate(displacement);
        }
        
    }

}
 