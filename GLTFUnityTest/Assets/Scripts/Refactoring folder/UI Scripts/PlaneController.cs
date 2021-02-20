using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaneController : MonoBehaviour
{
    public GameObject plane;

    [SerializeField] CrossSectionView crossSectionView;
    [SerializeField] GameObject UIBlocker;
    [SerializeField] Slider yPosSlider;
    [SerializeField] Slider xRotSlider;
    [SerializeField] Slider yRotSlider;
    [SerializeField] Slider zRotSlider;
    private float startYPos;
    private float startXRot;

    private float prevXRot;
    private float startYRot;

    private float prevYRot;
    private float startZRot;
    private float prevZRot;
    [SerializeField] Button resetButton;
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;

    private float maxPlaneHeight;
    private float minPlaneHeight;
    private Quaternion startRot = Quaternion.identity;
    // public Button confirmButton;
    // public Button cancelButton;

    void Awake(){

        confirmButton.onClick.AddListener(crossSectionView.confirmSlice);
        cancelButton.onClick.AddListener(crossSectionView.cancelSlice);
        cancelButton.onClick.AddListener(resetSlider);
        resetButton.onClick.AddListener(resetSlider);
        resetButton.onClick.AddListener(crossSectionView.resetPlane);

        yPosSlider.onValueChanged.AddListener(changeYPos);
        
        xRotSlider.onValueChanged.AddListener(changeXRot);
        yRotSlider.onValueChanged.AddListener(changeYRot);
        zRotSlider.onValueChanged.AddListener(changeZRot);
        prevXRot = startXRot = xRotSlider.value = 0;
        prevYRot = startYRot = yRotSlider.value = 0;
        prevZRot = startZRot = zRotSlider.value = 0;
        StartCoroutine(initialisePlane());
    }
    void start(){
        plane.transform.position = new Vector3(0,100,0);
        plane.transform.rotation = startRot;
    }
    private IEnumerator initialisePlane(){
        yield return new WaitUntil(() => ModelHandler.modelRadius != 0);
        startYPos = yPosSlider.value = yPosSlider.maxValue =maxPlaneHeight = ModelHandler.modelCentre.y + ModelHandler.modelRadius;
        yPosSlider.minValue = minPlaneHeight = ModelHandler.modelCentre.y - ModelHandler.modelRadius;
        plane.transform.position = Vector3.up * maxPlaneHeight;
        plane.transform.rotation = Quaternion.identity;
    }

    private IEnumerator enableBlocker(){
        yield return new WaitForSeconds(0.1f);
        UIBlocker.SetActive(true);
    }      
    
    void OnEnable(){
        StartCoroutine(enableBlocker());
    }
    void OnDisable(){
        UIBlocker.SetActive(false);
        EventManager.current.onEnableCamera();
    }
    public void changeXPos(float newXPos){
        plane.transform.position = new Vector3(newXPos, plane.transform.position.y, plane.transform.position.z);
    }
    public void changeYPos(float newYPos){
        plane.transform.position = new Vector3(plane.transform.position.x, newYPos, plane.transform.position.z);
    }
    public void changeZPos(float newZPos){
        plane.transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y, newZPos);
    }

    public void changeXRot(float newXRot){
        float delta = newXRot - this.prevXRot;
        this.plane.transform.Rotate(Vector3.right * delta);
        this.prevXRot = newXRot;
        //plane.transform.eulerAngles = new Vector3(newXRot, plane.transform.eulerAngles.y, plane.transform.eulerAngles.z);
    }
    public void changeYRot(float newYRot){
        // float delta = newYRot - this.prevYRot;
        // this.plane.transform.Rotate(Vector3.up * delta);
        // this.prevYRot = newYRot;
        // plane.transform.eulerAngles = new Vector3(plane.transform.eulerAngles.x, newYRot, plane.transform.eulerAngles.z);
        plane.transform.rotation = Quaternion.Euler(plane.transform.eulerAngles.x, newYRot, plane.transform.eulerAngles.z);
    }
    public void changeZRot(float newZRot){
        float delta = newZRot - this.prevZRot;
        this.plane.transform.Rotate(Vector3.forward * delta);
        this.prevZRot = newZRot;
        //plane.transform.eulerAngles = new Vector3(plane.transform.rotation.eulerAngles.x, plane.transform.rotation.eulerAngles.y, newZRot);
    }
    public void resetSlider(){
        yPosSlider.value = startYPos;
        xRotSlider.value = startXRot;
        yRotSlider.value = startYRot;
        zRotSlider.value = startZRot;
    }
    // public void onConfirm(){
    //     plane.SetActive(false);
    //     this.gameObject.SetActive(false);
    //     sliceMesh.sliceOrgan();
    // }
    // public void onCancel(){
    //     plane.SetActive(false);
    //     this.gameObject.SetActive(false);
    // }
    
}
