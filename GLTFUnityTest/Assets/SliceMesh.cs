using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SliceMesh : MonoBehaviour
{
    [SerializeField] GameObject organToSlice;
    GameObject pieceOne;
    GameObject pieceTwo;
    [SerializeField] GameObject plane;
    [SerializeField] Material capMaterial;
    private GameObject[] pieces = new GameObject[2];
    private bool isEnabled = false;
    private bool cut = true;
    [SerializeField] Shader crossSectionShader;

    [SerializeField] Shader otherShader;

    [SerializeField] Shader redBlueShader;
    Material mat;
    void assignNewMaterial(GameObject child, int i, Shader shader){
        Color col = child.GetComponent<MeshRenderer>().material.GetColor("_Color");
        col.a = 1.0f;
        mat = new Material(shader);
        mat.SetColor("_Color", col);
        mat.SetColor("_CrossColor", col);
        mat.renderQueue = 3100 - i*20;
        child.GetComponent<MeshRenderer>().material = mat;
    }
    void Start()
    {
        subscribeToEvents();

        // pieceOne.transform.position = gameObject.transform.position;
        // pieceTwo.transform.position = gameObject.transform.position;
        // pieces[0] = pieceOne;
        // pieces[1] = pieceTwo;

    }

    // Update is called once per frame
    void Update()
    {
        if(!isEnabled)return;
        mat.SetVector("_PlanePosition", plane.transform.position);
        mat.SetVector("_PlaneNormal", plane.transform.up);
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
        if(Input.GetMouseButtonDown(1)){
            cut = true;
            assignMaterialToAllChildren(otherShader);
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


}
