using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    
    private Transform cameraTransform;
    private Transform pivotTransform;

    private Vector3 localRot; //stores ongoing rotation of our camera pivot every frame.
    private float cameraDistance = 10.0f; //distance of camera's z coordinate from pivot's z coordinate.
    // Start is called before the first frame update
    public float mouseSensitivity = 4.0f; 
    public float scrollSensitivity = 4.0f;
    public float orbitDampening =10.0f; //Control how long it takes the camera to reach it's destination (in terms of rotation). Bigger number = fewer frames.
    public float scrollDampening = 10.0f; //Controls how long it  takes the camera to reach its zoom destination.

    public bool cameraDisabled = false; //if true, we can move the camera, if false, we can do other things with the mouse.
    void Start()
    {
        cameraTransform = this.transform;
        pivotTransform = this.transform.parent;
    }

    // Camera needs to render after everything else - called after Update is called for all other objects in our scene
    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift)) cameraDisabled = !cameraDisabled;

        if(!cameraDisabled){
            if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") !=0){
                localRot.x += Input.GetAxis("Mouse X");
                localRot.y -= Input.GetAxis("Mouse Y");
            }
            if(Input.GetAxis("Mouse ScrollWheel") != 0f){
                print(Input.GetAxis("Mouse ScrollWheel"));
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity;
                scrollAmount *= cameraDistance*0.3f;
                cameraDistance -= scrollAmount;
                cameraDistance = Mathf.Clamp(cameraDistance, 1.5f, 100f);
            }
        }
        Quaternion qt = Quaternion.Euler(localRot.y, localRot.x, 0);
        cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, qt, Time.deltaTime *orbitDampening);
        if(cameraTransform.position.z != cameraDistance * -1){
            cameraTransform.localPosition = new Vector3(cameraTransform.localPosition.x, cameraTransform.localPosition.y, Mathf.Lerp(cameraTransform.localPosition.z, cameraDistance*-1, Time.deltaTime * scrollDampening));
        }
        
    }
}
