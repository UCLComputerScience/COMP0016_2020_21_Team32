using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CameraMovement : MonoBehaviour
{
    //public static Vector3 startPosition = new Vector3(864.715f, 198.6647f, -1475.485f);
    //ResetPosition positionResetter;
    public static Vector3 startPos;
    public static Quaternion startRot;
    private Vector3 prevPosition;
    private float cameraRatio = 350/178; //experimentally discovered value to move the camera in the z plane relative to the radius of the model loaded
    public static Transform target;
    private Vector3 xTranslationCache = Vector3.zero;
    private Vector3 yTranslationCache = Vector3.zero;
    public Vector3 displacement; 
    private Vector3 initialDirVec;
    private float cameraDistance;
    private float scrollSpeed; 
    public static bool isEnabled = true;
    private Transform prevTransform;
    [SerializeField] GameObject pivot;
    float timeElapsed;
    public float travelTime = 2.0f;

    void Start() 
    {
        
        Camera.main.ScreenToViewportPoint(Input.mousePosition);
        StartCoroutine(setCameraDistance());
        target = pivot.transform; //The pivot of camera rotation is initialised to the same position as the model's parent gameobject (ie, this.gameObject) at (0,0,0)
        subscribeToEvents();
        initialDirVec = pivot.transform.position - Camera.main.transform.position;
        Camera.main.enabled =true;
        // Camera.main.gameObject.transform.position = startPosition;
        // Camera.main.gameObject.transform.rotation = startRotation;
        prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        prevTransform = Camera.main.transform;
        Camera.main.transparencySortMode = TransparencySortMode.Orthographic;
    }
    void LateUpdate()
    {
        if(!isEnabled) return;
        prevTransform = Camera.main.transform;

        if(Input.GetMouseButtonDown(0)){ 
            if(!EventSystem.current.IsPointerOverGameObject()){
                Camera.main.transform.Translate(displacement);
                Camera.main.transform.Translate(xTranslationCache);
                Camera.main.transform.Translate(yTranslationCache);
                prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition); // cam position set to normalised version of screen coordinates 
            } 
        }
        if(Input.GetMouseButton(0)){
            if(!EventSystem.current.IsPointerOverGameObject()){ 
                Vector3 dir = prevPosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);
                Camera.main.transform.position = target.transform.position;
                Camera.main.transform.Rotate(new Vector3(1f, 0f, 0f), dir.y *180);
                Camera.main.transform.Rotate(new Vector3(0f, 1f, 0f), -dir.x * 180, Space.World);
                Camera.main.transform.Translate(displacement);
                Camera.main.transform.Translate(xTranslationCache);
                Camera.main.transform.Translate(yTranslationCache);
                prevPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }

        }else if(Input.GetAxis("Mouse ScrollWheel") != 0){
            Camera.main.transform.position = target.transform.position;
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
            displacement -= new Vector3(0, 0, scrollAmount);
            Camera.main.transform.Translate(displacement);
            Camera.main.transform.Translate(xTranslationCache);
            Camera.main.transform.Translate(yTranslationCache);
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
        //Debug.Log(ModelHandler.organ.parent);
        cameraDistance = -cameraRatio * ModelHandler.modelRadius; //ratio * radius of renderer
        scrollSpeed = -cameraDistance;
        displacement = new Vector3(0f,0f,cameraDistance);
        Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Camera.main.transform.Translate(displacement);
        startPos = Camera.main.transform.position;
        startRot = Camera.main.transform.rotation;
        
    }
    private void subscribeToEvents(){
        EventManager.current.OnEnableCamera += EventManager_enableCamera;
        EventManager.current.OnEnablePivot += EventManager_otherEvent;
        EventManager.current.OnEnableCrossSection += EventManager_otherEvent;
        EventManager.current.OnReset += EventManager_resetPosition;
        EventManager.current.OnViewAnnotations += EventManager_otherEvent;
        EventManager.current.OnAddAnnotations += EventManager_otherEvent;
    }

    /*When the hand UI button is pressed, Selection manager fires off an event. This enables this script, so the user can
     start rotating the camera.*/

    public void EventManager_enableCamera(object sender, EventArgs e){
        isEnabled = true;
    }
    public void EventManager_otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }
    public void EventManager_resetPosition(object sender, EventArgs e){
        isEnabled = false;
        xTranslationCache = Vector3.zero;
        yTranslationCache = Vector3.zero;
        displacement.z =  cameraDistance;
    }
    private IEnumerator resetPosition(Vector3 currentPos, Vector3 startPos, Quaternion currentRot, Quaternion startRot){
        isEnabled = false;
        timeElapsed +=Time.deltaTime; 
        while(timeElapsed < travelTime){
            float ratio = timeElapsed/travelTime;
            Camera.main.gameObject.transform.position = Vector3.Lerp(currentPos, startPos, ratio);
            Camera.main.gameObject.transform.rotation = Quaternion.Lerp(currentRot, startRot, ratio);
            yield return null;

        }
        EventManager.current.onEnableCamera();
    }


}
 