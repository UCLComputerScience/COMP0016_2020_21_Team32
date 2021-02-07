using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SliceMesh : MonoBehaviour
{
    [SerializeField] GameObject planeController;
    [SerializeField] GameObject organToSlice;
    [SerializeField] GameObject plane;
    private bool isEnabled = false;
    private bool cut = false;
    [SerializeField] Shader otherShader;

    [SerializeField] Shader redBlueShader;
    Material mat;

    Vector3 cutStartPos;
    Vector3 cutEndPos;
    Plane rayCastPlane;

    Vector3 planepos;
    Vector3 normal;
    void assignNewMaterial(GameObject child, int i, Shader shader){
        Color col = child.GetComponent<MeshRenderer>().material.GetColor("_Color");
        col.a = 1.0f;
        mat = new Material(shader);
        mat.SetVector("_PlanePosition", planepos);
        mat.SetVector("_PlaneNormal", normal);
        mat.SetColor("_Color", col);
        mat.SetColor("_CrossColor", col);
        mat.renderQueue = 3100 - i*20;
        child.GetComponent<MeshRenderer>().material = mat;
    }
    void Start()
    {
        subscribeToEvents();
        plane.SetActive(false);
        plane.transform.position = new Vector3(0,100,0);
        plane.transform.rotation = Quaternion.identity;
        // rayCastPlane = new Plane(Vector3.up, 0f);
        // planepos = plane.transform.position;
        // normal = plane.transform.up;


    }
    public void confirmSlice(){
        assignMaterialToAllChildren(otherShader);
        //resetPlane();
        planeController.SetActive(false);
        plane.SetActive(false);
        cut = true;
    }
    public void cancelSlice(){
        resetPlane();
        planeController.SetActive(false);
        plane.SetActive(false);
        assignMaterialToAllChildren(otherShader);
    }

    public void resetPlane(){
        plane.transform.position = new Vector3(0, 100, 0);
        plane.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isEnabled)return;
        mat.SetVector("_PlanePosition", planepos);
        mat.SetVector("_PlaneNormal", normal);
        int count = 0;
        foreach(Transform t in organToSlice.GetComponentsInChildren<Transform>()){
            if(t.gameObject.GetComponent<MeshFilter>() != null && count != 1){
                Debug.Log(t.gameObject.name);
                t.GetComponent<MeshRenderer>().material.SetVector("_PlanePosition", plane.transform.position);
                t.GetComponent<MeshRenderer>().material.SetVector("_PlaneNormal", plane.transform.up);
                if(!cut){
                    Color col = new Color(1,0,0,0);
                    t.GetComponent<MeshRenderer>().material.SetVector("Color", col);
                }
            }
            count++;
        }
    }
    private void subscribeToEvents(){
        SelectionManager.current.onCameraButtonPressed += otherEvent;
        SelectionManager.current.onTButtonPressed += SelectionManager_onSliceButtonPressed;
        SelectionManager.current.onRButtonPressed += otherEvent;
        SelectionManager.current.onReButtonPressed += otherEvent;
        SelectionManager.current.onVAnnotationButtonPressed += otherEvent;
        SelectionManager.current.onAnnotationButton += otherEvent;
    }
    public void SelectionManager_onSliceButtonPressed(object sender, EventArgs e){
        isEnabled = true;
        assignMaterialToAllChildren(redBlueShader);
    }
    private void otherEvent(object sender, EventArgs e){
        isEnabled = false;

    }
    private void assignMaterialToAllChildren(Shader shader){
        int count = 0;
        foreach(Transform t in organToSlice.GetComponentsInChildren<Transform>()){
            if(t.gameObject.GetComponent<MeshFilter>() != null && count != 1){
                Debug.Log(t.name);
                assignNewMaterial(t.gameObject, count, shader);
            }
        count++;
        }
    }
    private Vector3 screenSpaceToWorldSpace(Vector3 mousePos, Plane plane){
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        float distanceToPlane;
        Vector3 click;
        if(plane.Raycast(ray, out distanceToPlane)){
            click = ray.GetPoint(distanceToPlane);
        }else{
            click = Vector3.zero;
        }
        Debug.Log(click);
        return click;
    }
    private static void drawPlane(Vector3 pos, Vector3 normal, int mag){
        Vector3 v3 = new Vector3(0,0,0);
        if(normal.normalized != Vector3.forward){
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * mag;
        }
        Vector3 c0 = pos - v3;
        Vector3 c2 = pos + v3;
        Quaternion q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        Vector3 c1 = pos - v3;
        Vector3 c3 = pos + v3;

        Debug.DrawLine(c0,c2, Color.blue,20.0f);
        Debug.DrawLine(c1,c3, Color.blue,20.0f);
        Debug.DrawLine(c0,c1, Color.blue,20.0f);
        Debug.DrawLine(c1,c2, Color.blue,20.0f);
        Debug.DrawLine(c2,c3, Color.blue,20.0f);
        Debug.DrawLine(c3,c0, Color.blue, 20.0f);
        Debug.DrawLine(pos, normal, Color.green, 20.0f);
    }

    void getClicks(){
        if(Input.GetMouseButtonDown(0)){
            if(!cut){
                cutStartPos = screenSpaceToWorldSpace(Input.mousePosition, rayCastPlane);
                Debug.Log(cutStartPos);
                cut = true;
            }
        }
        if(Input.GetMouseButtonUp(0)){
            cutEndPos = screenSpaceToWorldSpace(Input.mousePosition, rayCastPlane);
            Debug.Log(cutEndPos);
            Vector3 cutDir = Vector3.Normalize(cutStartPos-cutEndPos);
            Vector3 cutNorm = Vector3.Cross(cutDir, Vector3.up);
            drawPlane(Vector3.zero, cutNorm, 10000);
            planepos = Vector3.zero;
            normal = cutNorm;
            cut = false;
        }
    }


}
