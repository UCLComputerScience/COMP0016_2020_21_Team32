using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CameraController : MonoBehaviour
{
    private Vector3 curMouse;
    private Vector3 prevMouse;
    private Vector3 mouseDelta;
    public bool activeRotate;
    private Vector3 pivot = new Vector3(0f,0f,0f);
    private float scrollSpeed = 5f;
    private Vector3 displacement = new Vector3(0f,0f,-250f);
    private Vector3 farTarget;
    private Vector3 prevPos;
    private Vector3 translationCache = Vector3.zero;

    void Update(){
        if(Input.GetMouseButton(0))
        {
            activeRotate = true;
        }
        else activeRotate = false;

        curMouse = Input.mousePosition;
        mouseDelta = (prevMouse - curMouse).normalized;
        Debug.Log(mouseDelta);
        
    }
    void LateUpdate(){
        prevPos = Camera.main.transform.position;
        if(activeRotate){
            farTarget = getFarTarget2(Vector3.zero, Camera.main.transform.position - translationCache, 1000f);
            Camera.main.transform.position = pivot + translationCache;
            Camera.main.transform.Rotate(new Vector3(1f,0f,0f), mouseDelta.y);
            Camera.main.transform.Rotate(new Vector3(0f,1f,0f), -mouseDelta.x, Space.World);
            Camera.main.transform.Translate(displacement);
            //Camera.main.transform.position += translationCache;
        }else if(Input.GetAxis("Mouse ScrollWheel") != 0){
            //Camera.main.transform.position = pivot;
            Camera.main.transform.position = pivot;
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
            displacement -= new Vector3(0, 0, scrollAmount);
            Camera.main.transform.Translate(displacement);
        }else if(Input.GetKey(KeyCode.D)){
            StartCoroutine(cameraDist());
        }

        prevMouse = Input.mousePosition;
    }
    private IEnumerator cameraDist(){
        Camera.main.transform.RotateAround(farTarget, Camera.main.transform.up, -0.1f);
        Vector3 curPos = Camera.main.transform.position;
        yield return new WaitUntil(() => prevPos != curPos);
        Vector3 diff = curPos - prevPos;
        Debug.Log("The difference isssss: "+ diff);
        translationCache+= new Vector3(diff.x, 0f, 0f);
        Debug.Log(translationCache);
    }

    private Vector3 getFarTarget2(Vector3 oPiv, Vector3 cameraPos, float distance){
        Vector3 dirVec = (oPiv - cameraPos).normalized;
        Debug.DrawRay(Camera.main.transform.position, dirVec*distance, Color.red, 1f);
        return dirVec * distance;
    }
    
}
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using System;

// public class CameraMovement : MonoBehaviour
// {
//     //public static Vector3 startPosition = new Vector3(864.715f, 198.6647f, -1475.485f);

//     public static Vector3 startPosition = new Vector3(169.1957f, 93.43023f, -514.921f);
//     public static Quaternion startRotation = Quaternion.Euler(9.78f, -18.19f, 0f);
//     public Camera cam;
//     private Vector3 prevPosition;
//     private Vector3 farTarget;

//     private Vector3 cameraVelocity;
//     private float curRotation = 0f;

//     private Quaternion targetRot;

//     private float cameraRatio = 350/178; //experimentally discovered value to move the camera in the z plane relative to the radius of the model loaded
//     public Transform startTransform;
//     public static Transform target;

//     private Vector3 xTranslationCache = Vector3.zero;
//     private Vector3 yTranslationCache = Vector3.zero;
//     private Quaternion xRotationCache = Quaternion.identity;
//     private Quaternion yRotationCache = Quaternion.identity;
//     public Vector3 displacement; 
//     private Vector3 initialDirVec;
//     private Transform startTarget;  
//     private float cameraDistance;
//     private float scrollSpeed; 
//     public static bool isEnabled = true;
//     private Transform prevTransform;
//     Quaternion prevRot;
//     float TimeCounter = 0;

//     Transform lastRot;
//     private bool annotationJustViewed = false;
//     [SerializeField] GameObject pivot;

//     void Start() 
//     {
        
//         Camera.main.ScreenToViewportPoint(Input.mousePosition);
//         StartCoroutine(setCameraDistance());
//         target = pivot.transform; //The pivot of camera rotation is initialised to the same position as the model's parent gameobject (ie, this.gameObject) at (0,0,0)
//         // farTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//         // farTarget.transform.position = pivot.transform.position + Vector3.forward*1000f;
//         farTarget = pivot.transform.position + Vector3.forward*1000f;
//         //Camera.main.transform.SetParent(farTarget.transform);
//         subscribeToEvents();
//         //displacement = new Vector3(0, 0, cameraDistance);
//         initialDirVec = pivot.transform.position - Camera.main.transform.position;
//         Camera.main.enabled =true;
//         Camera.main.gameObject.transform.position = startPosition;
//         Camera.main.gameObject.transform.rotation = startRotation;
//         prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
//         prevTransform = Camera.main.transform;
//         startPosition = prevPosition;
//         Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
//     }
//     void LateUpdate()
//     {
//         if(!isEnabled) return;
//         //Debug.Log(xTranslationCache);
//         prevTransform = Camera.main.transform;

//         if(Input.GetMouseButtonDown(0)){ 
//                 //farTarget = pivot.transform.position + Vector3.forward*1000f;//getFarPivotPos(pivot.transform.position, Camera.main.transform.position, 1000f);
//                 //Camera.main.transform.LookAt(target);
//             if(!EventSystem.current.IsPointerOverGameObject()){
//                 //Camera.main.transform.Rotate(xRotationCache);
//                 Camera.main.transform.Translate(displacement);
//                 Camera.main.transform.Translate(xTranslationCache);
//                 Camera.main.transform.Translate(yTranslationCache);
//                 prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition); // cam position set to normalised version of screen coordinates 
//             } 
//         }
//         if(Input.GetMouseButton(0)){
//             if(!EventSystem.current.IsPointerOverGameObject()){ 
//                 //lastRot = Camera.main.transform.rotation;
//                 //farTarget = getFarTarget2(pivot.transform.position, Camera.main.transform.position, 1000f);//getFarPivotPos(pivot.transform.position, Camera.main.transform.position - xTranslationCache, 10000f);
//                 Vector3 dir = prevPosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);
//                 Camera.main.transform.position = target.transform.position;
//                 Camera.main.transform.Rotate(new Vector3(1f, 0f, 0f), dir.y *180);
//                 Camera.main.transform.Rotate(new Vector3(0f, 1f, 0f), -dir.x * 180, Space.World);
//                 prevRot = Camera.main.transform.rotation;
//                 //Camera.main.transform.Rotate(new Vector3(0f, 0f, 1f), dir.z * 180);
//                 Camera.main.transform.Translate(displacement);
//                 //Camera.main.transform.position += xTranslationCache;
//                 Camera.main.transform.Translate(xTranslationCache);
//                 Camera.main.transform.Translate(yTranslationCache);
//                 //Camera.main.transform.Translate(xTranslationCache);
//                 //Camera.main.transform.Translate(yTranslationCache);
//                 prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
//                 // Camera.main.transform.RotateAround(pivot.transform.position, Vector3.up, Input.GetAxisRaw("Mouse X") * 9);
//                 // Camera.main.transform.RotateAround(pivot.transform.position, Camera.main.transform.right, -Input.GetAxisRaw("Mouse Y") * 9);
//             }

//         }else if(Input.GetAxis("Mouse ScrollWheel") != 0){
//             Camera.main.transform.position = target.transform.position;
//             float scrollAmount = Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
//             displacement -= new Vector3(0, 0, scrollAmount);
//             //print(displacement);
//             Camera.main.transform.Translate(displacement);
//             Camera.main.transform.Translate(xTranslationCache);
//             Camera.main.transform.Translate(yTranslationCache);
//             //Camera.main.transform.Translate(yTranslationCache);
//         }
//         //     Debug.DrawLine(Camera.main.transform.position, farTarget, new Color(1f,0f,0f,1f), 20);
//         //     Camera.main.transform.RotateAround(farTarget, Vector3.up, -0.1f);
//         //     Vector3 curPos = Camera.main.transform.position;
//         //     Debug.Log("Help!");
//         //     // yield return new WaitUntil(() => prevTransform.position != curPos);
//         //     Debug.Log("Is this never reached?");
//         //     Vector3 diff = curPos - prevTransform.position;
//         //     Debug.Log("Difference: "+ diff);
//         //     xTranslationCache+= new Vector3(diff.x, 0f,0f);
//         // }else if(Input.GetKey(KeyCode.G)){
//         //     Camera.main.transform.RotateAround(farTarget, Vector3.up, 0.1f);
//         //     Vector3 curPos = Camera.main.transform.position;
//         //     Debug.Log("Help!");
//         //     // yield return new WaitUntil(() => prevTransform.position != curPos);
//         //     Debug.Log("Is this never reached?");
//         //     Vector3 diff = curPos - prevTransform.position;
//         //     xTranslationCache+= new Vector3(diff.x, 0f,0f);
//         // }else if(Input.GetKey(KeyCode.Y)){
//         //     Camera.main.transform.RotateAround(farTarget, Vector3.right, 0.1f);
//         //     Vector3 curPos = Camera.main.transform.position;
//         //     Debug.Log("Help!");
//         //     // yield return new WaitUntil(() => prevTransform.position != curPos);
//         //     Debug.Log("Is this never reached?");
//         //     Vector3 diff = curPos - prevTransform.position;
//         //     yTranslationCache+= new Vector3(0f, diff.y,0f);
//         // }else if(Input.GetKey(KeyCode.B)){
//         //     Camera.main.transform.RotateAround(farTarget, Vector3.right, 0.1f);
//         //     Vector3 curPos = Camera.main.transform.position;
//         //     Debug.Log("Help!");
//         //     // yield return new WaitUntil(() => prevTransform.position != curPos);
//         //     Debug.Log("Is this never reached?");
//         //     Vector3 diff = curPos - prevTransform.position;
//         //     xTranslationCache+= new Vector3(0f,diff.y,0f);
//         // }
//         else if(Input.GetKey(KeyCode.K)){
//             setCameraDistance2();
//         }
//         else if(Input.GetKey(KeyCode.J)){
//             StartCoroutine(cameraDist());
//         }else if(Input.GetKey(KeyCode.H)){
//             Camera.main.transform.position -= Camera.main.transform.right * 1.0f;
//             //Camera.main.transform.rotation = targetRot;
//             xTranslationCache += Vector3.right*1.0f;
//             //Camera.main.transform.RotateAround(farTarget, Camera.main.transform.up, 0.1f);
//             // Debug.DrawLine(this.gameObject.transform.position, Camera.main.transform.position, new Color(1f,0f,0f,1f), 20);
//             // // //Debug.Log(Camera.main.transform.right);
//             // xTranslationCache += Vector3.right*1.0f;
//             // Vector3 tempPos = Camera.main.transform.position;
//             // Camera.main.transform.LookAt(farTarget);
//             // Camera.main.transform.position = tempPos;
//             //Camera.main.transform.LookAt(farTarget);
//             //Camera.main.transform.LookAt(farTarget, Camera.main.transform.up);
//         }else if (Input.GetKey(KeyCode.G)){
//             Camera.main.transform.position += Camera.main.transform.right * 1.0f;
//             xTranslationCache += Vector3.left*1.0f;
//             //Camera.main.transform.LookAt(farTarget);
//             //Camera.main.transform.LookAt(pivot.transform);
//         }else if(Input.GetKey(KeyCode.Y)){
//             Camera.main.transform.position -= Camera.main.transform.up * 1.0f;
//             yTranslationCache += Vector3.up*1.0f;
//             //Camera.main.transform.LookAt(farTarget);
//             //Camera.main.transform.LookAt(pivot.transform);
//         }else if(Input.GetKey(KeyCode.B)){
//             Camera.main.transform.position += Camera.main.transform.up * 1.0f;
//             yTranslationCache += Vector3.down*1.0f;
//             //Camera.main.transform.LookAt(farTarget);
//             //Camera.main.transform.LookAt(pivot.transform);
//         // }else if(Input.GetKey(KeyCode.D)){
//         //     Camera.main.transform.position += Vector3.right*0.5f;
//         // }else if(Input.GetKey(KeyCode.A)){
//         //     Camera.main.transform.position -= Vector3.right*0.5f;
//         }else if(Input.GetKey(KeyCode.D)){
//             StartCoroutine(cameraDist());
//             //moveWithRigidBody();
//             //circularMotion();
//             //moveWithKey(1, Camera.main.transform.up);
//         }else if(Input.GetKey(KeyCode.A)){  
//             moveWithKey(-1, Camera.main.transform.up);
//         }else if(Input.GetKey(KeyCode.W)){  
//             moveWithKey(-1, Camera.main.transform.right);
//         }else if(Input.GetKey(KeyCode.S)){
//             moveWithKey(1, Camera.main.transform.right);
//         }
        
//     }

//     private void circularMotion(){
//         TimeCounter += Time.deltaTime;
//         float x = Mathf.Cos(TimeCounter) *5f;
//         float y = 0f;//Mathf.Sin(TimeCounter);
//         float z = Mathf.Sin(TimeCounter) *5f;
//         xTranslationCache += new Vector3(x,y,z);
//         Camera.main.transform.position += new Vector3(x,y,z);
//     }
//     /*Coroutine that initialises the camera distance dynamically based on the radius of the sphere that bounds the renderer of the model loaded into the application. This enables 
//     models of any physical size to be viewed when loaded into the application.*/
//     private IEnumerator setCameraDistance(){
//         yield return new WaitUntil(() => ModelHandler.organ.segments != null); //waits until the model has been loaded in - prevents nullReferencEexceptions being thrown
//         //Debug.Log(ModelHandler.organ.parent);
//         cameraDistance = -cameraRatio * ModelHandler.modelRadius; //ratio * radius of renderer
//         scrollSpeed = -cameraDistance;
//         displacement = new Vector3(0f,0f,cameraDistance);
//         Camera.main.ScreenToViewportPoint(Input.mousePosition);
//     }
//     private void setCameraDistance2(){
//         Vector3 dirVec = pivot.transform.position - Camera.main.transform.position;
//         cameraVelocity = Vector3.Cross(Camera.main.transform.right, dirVec).normalized;
//         Camera.main.transform.position += cameraVelocity;
//     }
//     private IEnumerator rotateParent(){
//         Transform parent = Camera.main.GetComponentInParent<Transform>();
//         parent.Rotate(Camera.main.transform.up, 0.5f);
//         Camera.main.transform.LookAt(lastRot);
//         Vector3 curPos = Camera.main.transform.position;
//         yield return new WaitUntil(() => prevTransform.position != curPos);
//         Vector3 diff = curPos - prevTransform.position;
//         xTranslationCache+= diff;
//     }

//     private void moveWithKey(int dir, Vector3 crosslhs){
//         Vector3 prevPos = Camera.main.transform.position;
//         Vector3 dirVec = farTarget - Camera.main.transform.position;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   
//         cameraVelocity = Vector3.Cross(dir * crosslhs, dirVec).normalized * 5f;
//         Camera.main.transform.Translate(cameraVelocity);
//         Camera.main.transform.forward = dirVec;
//         Debug.DrawRay(Camera.main.transform.position, dirVec, Color.red, 20);
//         Debug.DrawRay(Camera.main.transform.position, crosslhs, Color.blue, 20);
//         Debug.DrawRay(Camera.main.transform.position, cameraVelocity, Color.green, 20);
//         xTranslationCache += cameraVelocity;
//     }
//     private void moveWithRigidBody(){
//         Vector3 dirVec = farTarget - Camera.main.transform.position;
//         //rigidbody.AddForce(-dirVec);
//         rigidbody.velocity = Vector3.Cross(dirVec, Camera.main.transform.up).normalized;
//         xTranslationCache+=rigidbody.velocity;
//     }

//     //subscribes this script to events that are fired by the EventManager
//     private void subscribeToEvents(){
//         EventManager.current.OnEnableCamera += EventManager_enableCamera;
//         EventManager.current.OnEnablePivot += EventManager_otherEvent;
//         EventManager.current.OnEnableCrossSection += EventManager_otherEvent;
//         //EventManager.current.OnEnableDicom += EventManager_otherEvent;
//         EventManager.current.OnReset += EventManager_otherEvent;
//         EventManager.current.OnViewAnnotations += EventManager_otherEvent;
//         EventManager.current.OnAddAnnotations += EventManager_otherEvent;
//     }

//     /*When the hand UI button is pressed, Selection manager fires off an event. This enables this script, so the user can
//      start rotating the camera.*/

//     public void EventManager_enableCamera(object sender, EventArgs e){
//         isEnabled = true;
//     }
//     public void EventManager_otherEvent(object sender, EventArgs e){
//         isEnabled = false;
//     }
//     private IEnumerator cameraDist(){
//         Camera.main.transform.RotateAround(farTarget, Camera.main.transform.up, -0.1f);
//         pivot.transform.LookAt(Camera.main.transform);
//         Vector3 curPos = Camera.main.transform.position;
//         yield return new WaitUntil(() => prevTransform.position != curPos);
//         Vector3 diff = Vector3.Project(curPos - prevTransform.position, prevTransform.position);
//         xTranslationCache+= diff;
//     }
//     private void rotateAroundFarPoint(){
//         Camera.main.transform.RotateAround(farTarget, Camera.main.transform.up, -0.1f);
//         pivot.transform.LookAt(Camera.main.transform);
//         Vector3 curPos = Camera.main.transform.position;
//         Vector3 diff = curPos - prevTransform.position;
//         xTranslationCache+= diff;
//     }

//     private Vector3 getFarPivotPos(Vector3 originalPivot, Vector3 cameraPos, float distance){
//         Vector3 dirVec = (originalPivot - cameraPos).normalized;
//         return originalPivot + dirVec * distance;
//     }
//     private Vector3 getFarTarget2(Vector3 oPiv, Vector3 cameraPos, float distance){
//         Vector3 dirVec = (oPiv - cameraPos).normalized;
//         Debug.DrawRay(Camera.main.transform.position, dirVec*distance, Color.red, 1f);
//         return dirVec * distance;
//     }
//     private void lookRotation(){
//         Vector3 prevPos = Camera.main.transform.position;
//         Camera.main.transform.position += Camera.main.transform.right * 0.5f;
//         Quaternion rot = Quaternion.LookRotation((farTarget - Camera.main.transform.position), Vector3.up);
//         Camera.main.transform.rotation = rot;
//         xTranslationCache = Camera.main.transform.position - prevPos;
//     }

// //     public void otherEvent(object sender, EventArgs e){
// //         //Debug.Log("The otherEvent for cameraMovement");
// //         isEnabled = false;
// //     }
// //     public void SelectionMangager_onCameraButtonPressed(object sender, EventArgs e){
// //         isEnabled = true;
// //         //Debug.Log(isEnabled);
// //     }
// //     public void SelectionManager_OnAnnotationViewButtonPressed(object sender, EventArgs e){
// //         //displacement = new Vector3(0,0,Camera.main.gameObject.transform.position.z);
// //         annotationJustViewed = true;
// //         isEnabled = false;
// //     }


// //    //When the reset button is pressed, it's assumed the user will want to go back to rotating the camera.  
// //     public void SelectionManager_onReButtonPressed(object sender, EventArgs e){
// //         prevPosition = startPosition;
// //         displacement = new Vector3(0, 0, cameraDistance);
// //         dirVec = new Vector3();
// //         isEnabled = true; // should really wait until the camera has finished its Slerp
// //         //Debug.Log(isEnabled);
// //     }

//     //If any other button is pressed, this script is disabled

// }
 