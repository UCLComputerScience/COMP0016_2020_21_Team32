using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaneController : MonoBehaviour
{
    public GameObject plane;

    [SerializeField] GameObject UIBlocker;
    public SliceMesh sliceMesh;
    public Slider yPosSlider;
    public Slider xRotSlider;
    public Slider yRotSlider;
    public Slider zRotSlider;
    private float startYPos;
    private float startXRot;

    private float prevXRot;
    private float startYRot;

    private float prevYRot;
    private float startZRot;
    private float prevZRot;
    public Button resetButton;
    // public Button confirmButton;
    // public Button cancelButton;

    void Awake(){
        // confirmButton.onClick.AddListener(this.onConfirm);
        resetButton.onClick.AddListener(resetSlider);

        yPosSlider.onValueChanged.AddListener(changeYPos);
        

        xRotSlider.onValueChanged.AddListener(changeXRot);
        yRotSlider.onValueChanged.AddListener(changeYRot);
        zRotSlider.onValueChanged.AddListener(changeZRot);
        startYPos = yPosSlider.value;
        prevXRot = startXRot = xRotSlider.value = 0;
        prevYRot = startYRot = yRotSlider.value = 0;
        prevZRot = startZRot = zRotSlider.value = 0;
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
