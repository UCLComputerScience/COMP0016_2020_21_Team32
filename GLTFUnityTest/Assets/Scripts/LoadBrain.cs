using Siccity;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.Networking;

public class LoadBrain : MonoBehaviour
{
    public static LoadBrain current; //singleton pattern again - probably not ideal
    public GameObject plane;
    [SerializeField] Shader shader;
    [SerializeField] Shader otherShader;

    [SerializeField] Shader opaqueShader;
    [SerializeField] Button segmentSelectButton;
    int currentlySelected = 0;
   // [SerializeField] private Material baseMaterial; 
    [SerializeField]Slider opacitySlider;
    [SerializeField] TMP_Dropdown dropdown;

    List<GameObject> segments = new List<GameObject>();
    GameObject brain;

    
    private float segOpacity = 1.0f;
    private float minOpacity;
    GameObject curSegment = null;


    void Awake(){ //singleton pattern at work
        current = this;
        gameObject.AddComponent<LoadModel>();
    }

    //This makes the transparency work correctly
    void assignNewMaterial(GameObject child, int i, Shader shader){
        Color col = child.GetComponent<MeshRenderer>().material.GetColor("_Color");
        col.a = 1.0f;
        Material mat = new Material(shader);
        mat.enableInstancing = true; //should improve performance
        mat.SetColor("_Color", col);
        mat.SetVector("_PlanePosition", plane.transform.position); //Just sets the plane WAY away from the model (for the time being)
        mat.SetVector("_PlaneNormal", plane.transform.up);
        mat.renderQueue = 3000 + i*20;
        child.GetComponent<MeshRenderer>().material = mat;
    }
    void assignNewMaterial2(GameObject child, int i, Shader shader){
        Color col = child.GetComponent<MeshRenderer>().material.GetColor("_Color");
        col.a = 1.0f;
        Material mat = new Material(shader);
        mat.enableInstancing = true; //should improve performance
        mat.SetColor("_Color", col);
        mat.SetVector("_PlanePosition", plane.transform.position); //Just sets the plane WAY away from the model (for the time being)
        mat.SetVector("_PlaneNormal", plane.transform.up);
        //mat.renderQueue = 3000 + i*20;
        child.GetComponent<MeshRenderer>().material = mat;
    }

    private void assignMaterialToAllChildrenBelowIndex(int index, Shader shader){
        for(int i = segments.Count - 1 -index; i !=-1; i--){
            Debug.Log(i +" "+ segments[i].name);
            if(segments[i].GetComponent<Renderer>() != null)
            {
                Debug.Log(segments[i].name);
                assignNewMaterial(segments[i], segments.Count - i, otherShader);

            }
        }
    }

    void Start()
    {
        LoadModel load = GetComponent<LoadModel>();
        load.LoadInModel(this.gameObject, brain, segments, "brain.glb");
        assignMaterialToAllChildrenBelowIndex(0, otherShader);
        // dropdown.onValueChanged.AddListener(delegate{dropdownSegmentSelected(dropdown); });
        // //opacitySlider = GameObject.Find("Opacity Slider").GetComponent<Slider>();
        // //segOpacity = opacitySlider.value;
        // //opacitySlider.gameObject.SetActive(false);
        ColourSelect.current.onColourSelect += Pallete_onColourSelect;
    }
    // private void dropdownSegmentSelected(TMP_Dropdown dropdown){
    //     int newSelection = dropdown.value;
    //     print(newSelection);
    //     //clickSegmentButton(segments[newSelection]);
    //     curSegment = segments[newSelection];
    //     if(curSegment == null)print("Ye");
    // }
    // private IEnumerator whileColSelectGone(float waitTime)
    // {
    //     while (ColourSelect.current == null)
    //     {
    //         yield return new WaitForSeconds(waitTime);
    //         print("WaitAndPrint " + Time.time);
    //     }
    //     ColourSelect.current.onColourSelect += Pallete_onColourSelect;
    // }
    // private void displayOpacitySlider(){
    //     opacitySlider.enabled = true;
    //     opacitySlider.gameObject.SetActive(true);
    // }
    // private void hideOpacitySlider(){
    //     opacitySlider.enabled = false;
    //     opacitySlider.gameObject.SetActive(false);
    // }





    //Receives event: Changes segment colour 
   public void Pallete_onColourSelect(object sender, EventArgsColourData e){
        Color col = e.col;
        col.a = segOpacity;
        segments[currentlySelected].GetComponent<MeshRenderer>().material.SetColor("_Color", col);
    }

    //Sends events 
    //Gonna be used to let other scripts know which segment is currently selected. (Only AddAnnotation is currently subscribed to this event)
 
    public event EventHandler<onSegmentSelectEventArgs> onSegmentSelect;
    public class onSegmentSelectEventArgs : EventArgs{
        public GameObject curSegment;
        public onSegmentSelectEventArgs(GameObject curSegment){
            this.curSegment = curSegment;
        }
        
    }
    // public v oid selectSegment(int n){
    //     if(currentlySelected != n){
    //         currentlySelected = -1;
    //         hideOpacitySlider();
    //     }else{
    //         currentlySelected = n;
    //         displayOpacitySlider();
    //     }
    //     clickSegmentButton(segments[currentlySelected]);
    // }


    public void AdjustOpacity(float newOp) {
        if(segments[currentlySelected] != null){
            Debug.Log(currentlySelected);
            // for(int i = segments.Count -1; i != -1; i--){
            //     if((i < currentlySelected) && (segments[i].GetComponent<MeshRenderer>().material.color.a > 0.99f)){
            //         assignNewMaterial(segments[i], i, opaqueShader);
            //         Debug.Log("Here we're assignin': "+ segments[i].name);
            //     } 
            //     else{
            //         assignNewMaterial(segments[i], i, otherShader);
            //         Debug.Log("Here we're assignin da transparent: "+segments[i]);
            //     } 
            // }
            Debug.Log("WE'RE HERE");
            segOpacity = newOp;
            Color color = segments[currentlySelected].GetComponent<MeshRenderer>().material.color;
            color.a = segOpacity;
            if(color.a < minOpacity)segments[currentlySelected].GetComponent<MeshRenderer>().enabled = false;
            else{
                if(segments[currentlySelected].GetComponent<MeshRenderer>().enabled == false)segments[currentlySelected].GetComponent<MeshRenderer>().enabled = true;
                segments[currentlySelected].GetComponent<MeshRenderer>().material.SetColor("_Color", color);
            }
        }
    }

    public void selectSegment(){
        if(currentlySelected == segments.Count-1)currentlySelected = 0;
        else currentlySelected++;
        // clickSegmentButton(segments[currentlySelected]);
        onSegmentSelectEventArgs e = new onSegmentSelectEventArgs(segments[currentlySelected]);
        onSegmentSelect?.Invoke(this, e);
    }
}


















    // private void clickSegmentButton(GameObject segment){
    //     curSegment = segment;
    //     onSegmentSelectEventArgs e = new onSegmentSelectEventArgs(curSegment);
    //     onSegmentSelect?.Invoke(this, e);
    //     //curSegment.GetComponent<MeshRenderer>().material.color = new Color(255,0,0);
    // }
//     public void selectSegment1(){
//         if(currentlySelected == 0)
//         {
//             currentlySelected = -1;
//             hideOpacitySlider();
//         }else{
//             currentlySelected = 0;
//             displayOpacitySlider();
//         } 
//         clickSegmentButton(segments[0]);
//     }
//     public void selectSegment2(){
//         if(currentlySelected == 1)
//         {
//             currentlySelected = -1;
//             hideOpacitySlider();
//         }else{
//             currentlySelected = 1;
//             displayOpacitySlider();
//         } 
//         clickSegmentButton(segments[1]);
//     }
//     public void selectSegment3(){
//         if(currentlySelected == 2)
//         {
//             currentlySelected = -1;
//             hideOpacitySlider();
//         }else{
//             currentlySelected = 2;
//             displayOpacitySlider();
//         } 
//         clickSegmentButton(segments[2]);
//     }
//     public void selectSegment4(){
//         if(currentlySelected == 3)
//         {
//             currentlySelected = -1;
//             hideOpacitySlider();
//         }else{
//             currentlySelected = 3;
//             displayOpacitySlider();
//         } 
//         clickSegmentButton(segments[3]);
//     }
//     public void selectSegment5(){
//         if(currentlySelected == 4)
//         {
//             currentlySelected = -1;
//             hideOpacitySlider();
//         }else{
//             currentlySelected = 4;
//             displayOpacitySlider();
//         } 
//         clickSegmentButton(segments[4]);
//     }
    
// }
//     // public void selectSegment1(){
    //     selectRenderer(allRenderers[0]);
    // }
    // public void selectSegment2(){
    //     selectRenderer(allRenderers[1]);
    // }
    // public void selectSegment3(){
    //     selectRenderer(allRenderers[2]);
    // }
    // public void selectSegment4(){
    //     selectRenderer(allRenderers[3]);
    // }
    // public void selectSegment5(){
    //     selectRenderer(allRenderers[4]);
    // }