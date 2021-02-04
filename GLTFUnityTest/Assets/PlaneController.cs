using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlaneController : MonoBehaviour
{
    public GameObject plane;

    public SliceMesh sliceMesh;
    public Slider yPosSlider;
    public Slider xRotSlider;
    public Slider yRotSlider;
    public Slider zRotSlider;
    private float startYPos;
    private float startXRot;
    private float startYRot;
    private float startZRot;
    public Button resetButton;
    // public Button cancelButton;

    void Awake(){
        // confirmButton.onClick.AddListener(this.onConfirm);
        plane.transform.position = new Vector3(0f, 500f, 0f);
        resetButton.onClick.AddListener(resetSlider);
        yPosSlider.onValueChanged.AddListener(changeYPos);
        xRotSlider.onValueChanged.AddListener(changeXRot);
        yRotSlider.onValueChanged.AddListener(changeYRot);
        zRotSlider.onValueChanged.AddListener(changeZRot);
        startYPos = yPosSlider.value;
        startXRot = xRotSlider.value;
        startYRot = yRotSlider.value;
        startZRot = zRotSlider.value;
    }
    
    void OnEnable(){
        //plane.transform.position = new Vector3(0f, 100f, 0f);
    }
    // void OnDisable(){
    //     plane.transform.position = new Vector3(0f, 500f, 0f);
    // }
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
        plane.transform.eulerAngles = new Vector3(newXRot, plane.transform.eulerAngles.y, plane.transform.eulerAngles.z);
    }
    public void changeYRot(float newYRot){
        plane.transform.eulerAngles = new Vector3(plane.transform.eulerAngles.x, newYRot, plane.transform.eulerAngles.z);
    }
    public void changeZRot(float newZRot){
        plane.transform.eulerAngles = new Vector3(plane.transform.rotation.eulerAngles.x, plane.transform.rotation.eulerAngles.y, newZRot);
    }
    public void resetSlider(){
        plane.transform.position = new Vector3(0, 100, 0);
        plane.transform.rotation = Quaternion.identity;
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
