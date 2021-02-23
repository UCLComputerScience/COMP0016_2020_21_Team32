using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class test : MonoBehaviour
{
    //public static Vector3 startPosition = new Vector3(864.715f, 198.6647f, -1475.485f);

    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;
     void Update() {
         float h = horizontalSpeed * Input.GetAxis("Mouse X");
         float v = verticalSpeed * Input.GetAxis("Mouse Y");
         ModelHandler.current.gameObject.transform.Rotate(v, h, 0);
     }
//     public void otherEvent(object sender, EventArgs e){
//         //Debug.Log("The otherEvent for cameraMovement");
//         isEnabled = false;
//     }
//     public void SelectionMangager_onCameraButtonPressed(object sender, EventArgs e){
//         isEnabled = true;
//         //Debug.Log(isEnabled);
//     }
//     public void SelectionManager_OnAnnotationViewButtonPressed(object sender, EventArgs e){
//         //displacement = new Vector3(0,0,Camera.main.gameObject.transform.position.z);
//         annotationJustViewed = true;
//         isEnabled = false;
//     }


//    //When the reset button is pressed, it's assumed the user will want to go back to rotating the camera.  
//     public void SelectionManager_onReButtonPressed(object sender, EventArgs e){
//         prevPosition = startPosition;
//         displacement = new Vector3(0, 0, cameraDistance);
//         dirVec = new Vector3();
//         isEnabled = true; // should really wait until the camera has finished its Slerp
//         //Debug.Log(isEnabled);
//     }

    //If any other button is pressed, this script is disabled

}
 