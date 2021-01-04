using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    private Vector3 prevPosition;
    public Transform target;
    private Vector3 displacement; 
    private Vector3 dirVec;
    private float cameraDistance = -250f;
    private float scrollSpeed = 250f;
    void Start() 
    {
        displacement = new Vector3(0, 0, cameraDistance);
        dirVec = new Vector3();
        prevPosition = cam.ScreenToViewportPoint(Input.mousePosition);
        cam.transparencySortMode = TransparencySortMode.Orthographic;
    }
    //WORKING
    //Zoom controls inverted and still a bit slow but other than that working nicely.
    
    // Update is called once per frame

    void LateUpdate()
    {
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
 