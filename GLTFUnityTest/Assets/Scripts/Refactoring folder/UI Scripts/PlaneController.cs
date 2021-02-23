using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaneController : MonoBehaviour
{
    public GameObject plane;
    [SerializeField] Shader otherShader;
    [SerializeField] Shader redBlueShader;
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

        confirmButton.onClick.AddListener(confirmSlice);
        cancelButton.onClick.AddListener(cancelSlice);
        cancelButton.onClick.AddListener(resetSlider);
        resetButton.onClick.AddListener(resetSlider);
        resetButton.onClick.AddListener(resetPlane);

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
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, redBlueShader);
        StartCoroutine(enableBlocker());
    }
    void OnDisable(){
        UIBlocker.SetActive(false);
        EventManager.current.onEnableCamera();
        ToolTip.current.gameObject.SetActive(false);
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
    }
    public void changeYRot(float newYRot){
        plane.transform.rotation = Quaternion.Euler(plane.transform.eulerAngles.x, newYRot, plane.transform.eulerAngles.z);
    }
    public void changeZRot(float newZRot){
        float delta = newZRot - this.prevZRot;
        this.plane.transform.Rotate(Vector3.forward * delta);
        this.prevZRot = newZRot;
    }
    public void resetSlider(){
        yPosSlider.value = startYPos;
        xRotSlider.value = startXRot;
        yRotSlider.value = startYRot;
        zRotSlider.value = startZRot;
    }
    public void confirmSlice(){
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, otherShader);
        plane.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void cancelSlice(){
        resetPlane();
        plane.SetActive(false);
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, otherShader);
        this.gameObject.SetActive(false);
    }
    public void resetPlane(){
        for(int i = 0; i < 100; i++){
            plane.transform.position = new Vector3(0, 100, 0);
            plane.transform.rotation = startRot;
        }
        StartCoroutine(resetPlaneHelper());
    }

    private IEnumerator resetPlaneHelper(){
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < 100; i++){
            plane.transform.position = new Vector3(0,100,0);
            plane.transform.rotation = startRot;
        }
    }
    void Update()
    {
        foreach(GameObject g in ModelHandler.segments){
            g.GetComponent<MeshRenderer>().material.SetVector("_PlanePosition", plane.transform.position);
            g.GetComponent<MeshRenderer>().material.SetVector("_PlaneNormal", plane.transform.up);
            
        }
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
