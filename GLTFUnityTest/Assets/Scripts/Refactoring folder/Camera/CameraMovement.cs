using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CameraMovement : MonoBehaviour
{
    //public static Vector3 startPosition = new Vector3(864.715f, 198.6647f, -1475.485f);
    public static Vector3 startPosition = new Vector3(169.1957f, 93.43023f, -514.921f);
    public static Quaternion startRotation = Quaternion.Euler(9.78f, -18.19f, 0f);
    public Camera cam;
    private Vector3 prevPosition;
    private Vector3 farTarget;

    private Quaternion targetRot;

    private float cameraRatio = 350/178; //experimentally discovered value to move the camera in the z plane relative to the radius of the model loaded
    public Transform startTransform;
    public static Transform target;

    private Vector3 xTranslationCache = Vector3.zero;
    private Vector3 yTranslationCache = Vector3.zero;
    private Quaternion xRotationCache = Quaternion.identity;
    private Quaternion yRotationCache = Quaternion.identity;
    public Vector3 displacement; 
    private Vector3 dirVec;
    private Transform startTarget;  
    private float cameraDistance;
    private float scrollSpeed; //= 250f;
    public static bool isEnabled = true;
    private Transform prevTransform;
    Quaternion prevRot;
    private bool annotationJustViewed = false;
    [SerializeField] GameObject pivot;

    void Start() 
    {
        Camera.main.ScreenToViewportPoint(Input.mousePosition);
        StartCoroutine(setCameraDistance());
        target = pivot.transform; //The pivot of camera rotation is initialised to the same position as the model's parent gameobject (ie, this.gameObject) at (0,0,0)
        // farTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // farTarget.transform.position = pivot.transform.position + Vector3.forward*1000f;
        farTarget = pivot.transform.position + Vector3.forward*1000f;
        //Camera.main.transform.SetParent(farTarget.transform);
        subscribeToEvents();
        //displacement = new Vector3(0, 0, cameraDistance);
        dirVec = new Vector3();
        Camera.main.enabled =true;
        Camera.main.gameObject.transform.position = startPosition;
        Camera.main.gameObject.transform.rotation = startRotation;
        prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        prevTransform = Camera.main.transform;
        startPosition = prevPosition;
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
    }
    void LateUpdate()
    {
        if(!isEnabled) return;
        //Debug.Log(xTranslationCache);
        prevTransform = Camera.main.transform;

        if(Input.GetMouseButtonDown(0)){ 
                //farTarget = pivot.transform.position + Vector3.forward*1000f;//getFarPivotPos(pivot.transform.position, Camera.main.transform.position, 1000f);
                //Camera.main.transform.LookAt(target);
            if(!EventSystem.current.IsPointerOverGameObject()){
                Camera.main.transform.Translate(xTranslationCache);
                Camera.main.transform.Translate(yTranslationCache);
                //Camera.main.transform.Rotate(xRotationCache);
                prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition); // cam position set to normalised version of screen coordinates 
            } 
        }
        if(Input.GetMouseButton(0)){
            if(!EventSystem.current.IsPointerOverGameObject()){ //Ensures camera doesn't move when interacting with UI (THIS IS STILL BUGGY)
                farTarget = getFarPivotPos(pivot.transform.position, Camera.main.transform.position - xTranslationCache, 1000f);
                Vector3 dir = prevPosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

                Camera.main.transform.position = target.transform.position;
                //Camera.main.transform.rotation *= xRotationCache;
                Camera.main.transform.Rotate(new Vector3(1f, 0f, 0f), dir.y *180);
                Camera.main.transform.Rotate(new Vector3(0f, 1f, 0f), -dir.x * 180, Space.World);
                prevRot = Camera.main.transform.rotation;
                //Camera.main.transform.Rotate(new Vector3(0f, 0f, 1f), dir.z * 180);
                Camera.main.transform.Translate(displacement);
                Camera.main.transform.Translate(xTranslationCache);
                Camera.main.transform.Translate(yTranslationCache);
                prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                // Camera.main.transform.RotateAround(pivot.transform.position, Vector3.up, Input.GetAxisRaw("Mouse X") * 9);
                // Camera.main.transform.RotateAround(pivot.transform.position, Camera.main.transform.right, -Input.GetAxisRaw("Mouse Y") * 9);
            }

        }else if(Input.GetAxis("Mouse ScrollWheel") != 0){
            Camera.main.transform.position = target.transform.position;
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
            displacement -= new Vector3(0, 0, scrollAmount);
            //print(displacement);
            Camera.main.transform.Translate(displacement);
            Camera.main.transform.Translate(xTranslationCache);
            Camera.main.transform.Translate(yTranslationCache);
        //     Debug.DrawLine(Camera.main.transform.position, farTarget, new Color(1f,0f,0f,1f), 20);
        //     Camera.main.transform.RotateAround(farTarget, Vector3.up, -0.1f);
        //     Vector3 curPos = Camera.main.transform.position;
        //     Debug.Log("Help!");
        //     // yield return new WaitUntil(() => prevTransform.position != curPos);
        //     Debug.Log("Is this never reached?");
        //     Vector3 diff = curPos - prevTransform.position;
        //     Debug.Log("Difference: "+ diff);
        //     xTranslationCache+= new Vector3(diff.x, 0f,0f);
        // }else if(Input.GetKey(KeyCode.G)){
        //     Camera.main.transform.RotateAround(farTarget, Vector3.up, 0.1f);
        //     Vector3 curPos = Camera.main.transform.position;
        //     Debug.Log("Help!");
        //     // yield return new WaitUntil(() => prevTransform.position != curPos);
        //     Debug.Log("Is this never reached?");
        //     Vector3 diff = curPos - prevTransform.position;
        //     xTranslationCache+= new Vector3(diff.x, 0f,0f);
        // }else if(Input.GetKey(KeyCode.Y)){
        //     Camera.main.transform.RotateAround(farTarget, Vector3.right, 0.1f);
        //     Vector3 curPos = Camera.main.transform.position;
        //     Debug.Log("Help!");
        //     // yield return new WaitUntil(() => prevTransform.position != curPos);
        //     Debug.Log("Is this never reached?");
        //     Vector3 diff = curPos - prevTransform.position;
        //     yTranslationCache+= new Vector3(0f, diff.y,0f);
        // }else if(Input.GetKey(KeyCode.B)){
        //     Camera.main.transform.RotateAround(farTarget, Vector3.right, 0.1f);
        //     Vector3 curPos = Camera.main.transform.position;
        //     Debug.Log("Help!");
        //     // yield return new WaitUntil(() => prevTransform.position != curPos);
        //     Debug.Log("Is this never reached?");
        //     Vector3 diff = curPos - prevTransform.position;
        //     xTranslationCache+= new Vector3(0f,diff.y,0f);
        // }
        }else if(Input.GetKey(KeyCode.H)){
            Camera.main.transform.position += Camera.main.transform.right * 1.0f;
            //Camera.main.transform.rotation = targetRot;
            xTranslationCache += Vector3.right*1.0f;
            //Camera.main.transform.RotateAround(farTarget, Camera.main.transform.up, 0.1f);
            // Debug.DrawLine(this.gameObject.transform.position, Camera.main.transform.position, new Color(1f,0f,0f,1f), 20);
            // // //Debug.Log(Camera.main.transform.right);
            // xTranslationCache += Vector3.right*1.0f;
            // Vector3 tempPos = Camera.main.transform.position;
            // Camera.main.transform.LookAt(farTarget);
            // Camera.main.transform.position = tempPos;
            //Camera.main.transform.LookAt(farTarget);
            //Camera.main.transform.LookAt(farTarget, Camera.main.transform.up);
        }else if (Input.GetKey(KeyCode.G)){
            Camera.main.transform.position -= Camera.main.transform.right * 1.0f;
            xTranslationCache += Vector3.left*1.0f;
            Debug.Log(Quaternion.Angle(this.transform.rotation, Camera.main.transform.rotation));
            //Camera.main.transform.LookAt(farTarget);
            //Camera.main.transform.LookAt(pivot.transform);
        }else if(Input.GetKey(KeyCode.Y)){
            Camera.main.transform.position += Camera.main.transform.up * 1.0f;
            yTranslationCache += Vector3.up*1.0f;
            //Camera.main.transform.LookAt(farTarget);
            //Camera.main.transform.LookAt(pivot.transform);
        }else if(Input.GetKey(KeyCode.B)){
            Camera.main.transform.position -= Camera.main.transform.up * 1.0f;
            yTranslationCache += Vector3.down*1.0f;
            //Camera.main.transform.LookAt(farTarget);
            //Camera.main.transform.LookAt(pivot.transform);
        }else if(Input.GetKey(KeyCode.D)){
            Camera.main.transform.position += Vector3.right*0.5f;
        }else if(Input.GetKey(KeyCode.A)){
            Camera.main.transform.position -= Vector3.right*0.5f;
        }
        
    }


    /* Coroutine that initialises the camera distance dynamically based on the radius of the sphere that bounds the renderer of the model loaded into the application. This enables 
       models of any physical size to be viewed when loaded into the application.*/
    private IEnumerator setCameraDistance(){
        yield return new WaitUntil(() => ModelHandler.organ.segments != null); //waits until the model has been loaded in - prevents nullReferencEexceptions being thrown
        Debug.Log(ModelHandler.organ.parent);
        cameraDistance = -cameraRatio * ModelHandler.modelRadius; //ratio * radius of renderer
        scrollSpeed = -cameraDistance;
        displacement = new Vector3(0f,0f,cameraDistance);
        Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }

    //subscribes this script to events that are fired by the EventManager
    private void subscribeToEvents(){
        EventManager.current.OnEnableCamera += EventManager_enableCamera;
        EventManager.current.OnEnablePivot += EventManager_otherEvent;
        EventManager.current.OnEnableCrossSection += EventManager_otherEvent;
        EventManager.current.OnEnableDicom += EventManager_otherEvent;
        EventManager.current.OnReset += EventManager_otherEvent;
        EventManager.current.OnViewAnnotations += EventManager_otherEvent;
        EventManager.current.OnAddAnnotations += EventManager_otherEvent;
    }

    /*When the hand UI button is pressed, Selection manager fires off an event. This enables this script, so the user can
     start rotating the camera.*/

    public void EventManager_enableCamera(object sender, EventArgs e){
        Debug.Log("Camera enabled");
        isEnabled = true;
    }
    public void EventManager_otherEvent(object sender, EventArgs e){
        Debug.Log("Camera disabled");
        isEnabled = false;
    }
    private IEnumerator cameraDist(){
        Camera.main.transform.RotateAround(farTarget, Vector3.up, -0.1f);
        Vector3 curPos = Camera.main.transform.position;
        yield return new WaitUntil(() => prevTransform.position != curPos);
        Vector3 diff = curPos - prevTransform.position;
        xTranslationCache+= new Vector3(diff.x, 0f,0f);
    }

    // private IEnumerator changeDirection(){
    //     Camera.main.gameObject.transform.rotation = Quaternion.Lerp(startRot, targetRot, ratio);
    // }
    private Vector3 getFarPivotPos(Vector3 originalPivot, Vector3 cameraPos, float distance){
        Vector3 dirVec = (originalPivot - cameraPos).normalized;
        return originalPivot + dirVec * distance;
    }
    private void lookRotation(){
        Vector3 prevPos = Camera.main.transform.position;
        Camera.main.transform.position += Camera.main.transform.right * 0.5f;
        Quaternion rot = Quaternion.LookRotation((farTarget - Camera.main.transform.position), Vector3.up);
        Camera.main.transform.rotation = rot;
        xTranslationCache = Camera.main.transform.position - prevPos;
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
 