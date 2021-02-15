using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ChangeCameraAxis : MonoBehaviour
{
    private bool isEnabled = false;
    //[SerializeField] GameObject axes;
    [SerializeField] GameObject pivotController;
    [SerializeField] GameObject pivot;
    void Start()
    {
        subscribeToEvents();
    }

    private void subscribeToEvents(){
        SelectionManager.current.onCameraButtonPressed += otherEvent;
        SelectionManager.current.onTButtonPressed += otherEvent;
        SelectionManager.current.onRButtonPressed += otherEvent;
        SelectionManager.current.onReButtonPressed += otherEvent;
        SelectionManager.current.onVAnnotationButtonPressed += otherEvent;
        SelectionManager.current.onAnnotationButton += otherEvent;
        /*Remember to do this for the other classes later*/
        SelectionManager.current.onChangePivot += SelectionManager_OnChangePivot;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeCameraAxis(){
        //isEnabled = false;
    }
    public void SelectionManager_OnChangePivot(object sender, EventArgs e){
        Debug.Log("Here I am");
        pivot.transform.position = CameraMovement.target.position;
        pivotController.SetActive(true);
        foreach(GameObject g in ModelHandler.organ.segments){
            Renderer r = g.GetComponent<Renderer>();
            float op = (r.material.color.a < 0.4f) ? r.material.color.a : 0.4f;
            r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, op);
        }
        //sphere.AddComponent<MoveAxis>();
        //sphere.transform.localScale = new Vector3(20f,20f,20f);
    }    
    public void otherEvent(object sender, EventArgs e){
        Debug.Log("disabling axes");
        isEnabled = false;
        pivotController.SetActive(false);
        pivot.SetActive(false);
    }
}
