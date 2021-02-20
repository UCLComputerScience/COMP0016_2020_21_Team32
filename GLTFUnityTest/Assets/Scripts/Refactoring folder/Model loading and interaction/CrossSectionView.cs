using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*Class handles the cross-sectional view feature by applying different shaders to the model and updating its material
every frame with the current normal position and transform position of the plane. The position and orientation of the plane
determine which polygons are drawn to the screen by the GPU each frame. 
*/

/*THIS CLASS NEEDS TO BE REFACTORED 
The confirm, cancel and reset plane functions should probably be in the PlaneController class.
The assign material functions are very similar to those in the ModelHandler class and should probably
be moved to a new class.
*/
public class CrossSectionView : MonoBehaviour
{
    [SerializeField] GameObject planeController;
    [SerializeField] GameObject plane;
    private bool isEnabled = false;
    private bool cut = false;
    [SerializeField] Shader otherShader;
    [SerializeField] Shader redBlueShader;
    Quaternion startRot = Quaternion.identity;

    void Start()
    {
        subscribeToEvents();
        plane.SetActive(false);
        /*Plane pos should be based on the bounding box of the renderer of the model*/

    }

    /*Invoked  by plane controller when the confirm button is pressed. Assigns the standard shader to all segments of the model*/
    public void confirmSlice(){
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, otherShader);
        planeController.SetActive(false);
        plane.SetActive(false);
    }
    public void cancelSlice(){
        resetPlane();
        planeController.SetActive(false);
        plane.SetActive(false);
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, otherShader);
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
        if(!isEnabled)return;
        foreach(GameObject g in ModelHandler.segments){
            g.GetComponent<MeshRenderer>().material.SetVector("_PlanePosition", plane.transform.position);
            g.GetComponent<MeshRenderer>().material.SetVector("_PlaneNormal", plane.transform.up);
            
        }
    }
    private void subscribeToEvents(){
        EventManager.current.OnEnableCamera += otherEvent;
        EventManager.current.OnEnablePivot += otherEvent;
        EventManager.current.OnEnableCrossSection += EventManager_OnCrossSectionEnabled;
        EventManager.current.OnEnableDicom += otherEvent;
        EventManager.current.OnReset += otherEvent;
        EventManager.current.OnViewAnnotations += otherEvent;
        EventManager.current.OnAddAnnotations += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
    }
    public void EventManager_OnCrossSectionEnabled(object sender, EventArgs e){
        isEnabled = true;
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments, redBlueShader);
        planeController.SetActive(true);
    }
    private void otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }

    // private static void drawPlane(Vector3 pos, Vector3 normal, int mag){
    //     Vector3 v3 = new Vector3(0,0,0);
    //     if(normal.normalized != Vector3.forward){
    //         v3 = Vector3.Cross(normal, Vector3.forward).normalized * mag;
    //     }
    //     Vector3 c0 = pos - v3;
    //     Vector3 c2 = pos + v3;
    //     Quaternion q = Quaternion.AngleAxis(90.0f, normal);
    //     v3 = q * v3;
    //     Vector3 c1 = pos - v3;
    //     Vector3 c3 = pos + v3;

    //     Debug.DrawLine(c0,c2, Color.blue,20.0f);
    //     Debug.DrawLine(c1,c3, Color.blue,20.0f);
    //     Debug.DrawLine(c0,c1, Color.blue,20.0f);
    //     Debug.DrawLine(c1,c2, Color.blue,20.0f);
    //     Debug.DrawLine(c2,c3, Color.blue,20.0f);
    //     Debug.DrawLine(c3,c0, Color.blue, 20.0f);
    //     Debug.DrawLine(pos, normal, Color.green, 20.0f);
    // }

    // void getClicks(){
    //     if(Input.GetMouseButtonDown(0)){
    //         if(!cut){
    //             cutStartPos = screenSpaceToWorldSpace(Input.mousePosition, rayCastPlane);
    //             Debug.Log(cutStartPos);
    //             cut = true;
    //         }
    //     }
    //     if(Input.GetMouseButtonUp(0)){
    //         cutEndPos = screenSpaceToWorldSpace(Input.mousePosition, rayCastPlane);
    //         Debug.Log(cutEndPos);
    //         Vector3 cutDir = Vector3.Normalize(cutStartPos-cutEndPos);
    //         Vector3 cutNorm = Vector3.Cross(cutDir, Vector3.up);
    //         drawPlane(Vector3.zero, cutNorm, 10000);
    //         planepos = Vector3.zero;
    //         normal = cutNorm;
    //         cut = false;
    //     }
    // }
    // private Vector3 screenSpaceToWorldSpace(Vector3 mousePos, Plane plane){
    //     Ray ray = Camera.main.ScreenPointToRay(mousePos);
    //     float distanceToPlane;
    //     Vector3 click;
    //     if(plane.Raycast(ray, out distanceToPlane)){
    //         click = ray.GetPoint(distanceToPlane);
    //     }else{
    //         click = Vector3.zero;
    //     }
    //     Debug.Log(click);
    //     return click;
    // }


}
