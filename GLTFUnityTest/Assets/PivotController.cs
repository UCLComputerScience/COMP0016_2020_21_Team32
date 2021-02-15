using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PivotController : MonoBehaviour
{
    public GameObject pivot;
    [SerializeField] GameObject axes;

    [SerializeField] GameObject UIBlocker;
    //public SliceMesh sliceMesh;
    public Vector3 startPos;

    public Slider xPosSlider;
    public Slider yPosSlider;
    public Slider zPosSlider;
    private float startXPos;
    private float startYPos;
    private float startZPos;

    public Button confirmButton;
    public Button cancelButton;
    public Button resetButton;
    // public Button confirmButton;
    // public Button cancelButton;

    void Awake(){
        pivot.transform.position = startPos = CameraMovement.target.position;
        // confirmButton.onClick.AddListener(this.onConfirm);
        // resetButton.onClick.RemoveAllListeners();
        // confirmButton.onClick.RemoveAllListeners();
        // cancelButton.onClick.RemoveAllListeners();

        // xPosSlider.onValueChanged.RemoveAllListeners();
        // yPosSlider.onValueChanged.RemoveAllListeners();
        // zPosSlider.onValueChanged.RemoveAllListeners();

        // resetButton.onClick.AddListener(resetSlider);
        // confirmButton.onClick.AddListener(onConfirm);
        // cancelButton.onClick.AddListener(onCancel);

        // xPosSlider.onValueChanged.AddListener(changeXPos);
        // yPosSlider.onValueChanged.AddListener(changeYPos);
        // zPosSlider.onValueChanged.AddListener(changeZPos);
        // startXPos = xPosSlider.value;
        // startYPos = yPosSlider.value;
        // startZPos = zPosSlider.value;
    }

    private IEnumerator enableBlocker(){
        yield return new WaitForSeconds(0.1f);
        UIBlocker.SetActive(true);
    }
    
    void OnEnable(){
        StartCoroutine(enableBlocker());
        pivot.transform.position = startPos;
        pivot.SetActive(true);
        axes.SetActive(true);
    }
    void OnDisable(){
        startPos = pivot.transform.position;
        pivot.SetActive(false);
        UIBlocker.SetActive(false);
        CameraMovement.isEnabled = true;
        axes.SetActive(false);
    }
    public void changeXPos(float newXPos){
        pivot.transform.position = new Vector3(newXPos, pivot.transform.position.y, pivot.transform.position.z);
    }
    public void changeYPos(float newYPos){
        pivot.transform.position = new Vector3(pivot.transform.position.x, newYPos, pivot.transform.position.z);
    }
    public void changeZPos(float newZPos){
        pivot.transform.position = new Vector3(pivot.transform.position.x, pivot.transform.position.y, newZPos);
    }
    public void resetSlider(){
        xPosSlider.value = startXPos;
        yPosSlider.value = startYPos;
        zPosSlider.value = startZPos;
        pivot.transform.position = CameraMovement.target.position;
    }
    public void onConfirm(){
        startPos = pivot.transform.position;
        CameraMovement.target.position = pivot.transform.position;
        pivot.SetActive(false);
        UIBlocker.SetActive(false);
        this.gameObject.SetActive(false);
    }
    public void onCancel(){
        resetSlider();
        pivot.SetActive(false);
        UIBlocker.SetActive(false);
        this.gameObject.SetActive(false);
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
