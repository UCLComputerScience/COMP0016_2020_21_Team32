using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AddAnnotation : MonoBehaviour
{
    private bool isEnabled = false;

    GameObject prevSeg; //used to get rid of the meshcollider since it's resource intensive BOY

    GameObject selectedSegment; //maintains a reference to the object whose mesh we'll be annotating.
    // Start is called before the first frame update
    Mesh mesh;
    int count;
    [SerializeField] Camera cam;
    [SerializeField] Button button; 
    Ray ray;
    // [SerializeField] Annotation annotation;
    [SerializeField] Annotation annotation;
    RaycastHit hit;
    String title;
    void Start()
    {
        annotation.hide();
        title = "Annotation #" + annotation.annotationId;
 
        
        SelectionManager.current.onCameraButtonPressed += otherEvent;
        SelectionManager.current.onTButtonPressed += otherEvent;
        SelectionManager.current.onRButtonPressed += otherEvent;
        SelectionManager.current.onReButtonPressed += otherEvent;
        SelectionManager.current.onVAnnotationButtonPressed += otherEvent;
        SelectionManager.current.onAnnotationButton += SelectionManager_onAnnotationButtonPressed;
        LoadBrain.current.onSegmentSelect += LoadBrain_OnSegmentSelect;
    }

    public void SelectionManager_onAnnotationButtonPressed(object sender, EventArgs e){
        isEnabled = true;
        //annotation.show(new Vector3(0f,0f,0f)); 
    }
    public void LoadBrain_OnSegmentSelect(object sender, LoadBrain.onSegmentSelectEventArgs e){
        //if(prevSeg!=null) Destroy(prevSeg.GetComponent<MeshCollider>());
        selectedSegment = e.curSegment;
        mesh = selectedSegment.GetComponent<MeshFilter>().mesh;
        // selectedSegment.AddComponent<MeshCollider>();
        // prevSeg = selectedSegment;
    }
    public void otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }

    // Update is called once per frame
    // void FixedUpdate()
    // {
    //     if(!isEnabled || mesh == null)return;

    //     }
    // }

}
