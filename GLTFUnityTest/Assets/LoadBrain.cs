using Siccity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
//loads the model at runtime

//ONE ADDITIONAL COMMENT
public class LoadBrain : MonoBehaviour
{
    public static LoadBrain current; //singleton pattern again - probably not ideal
    [SerializeField] Shader shader;
    [SerializeField] Button segmentSelectButton;
    int currentlySelected = 0;
   // [SerializeField] private Material baseMaterial; 
    [SerializeField]Slider opacitySlider;
    [SerializeField] TMP_Dropdown dropdown;

    List<GameObject> segments = new List<GameObject>();
    List<TMP_Text> segmentNames = new List<TMP_Text>();
    //public Stack<Renderer> enabledRenderers = new Stack<Renderer>();
    public List<Renderer> allRenderers = new List<Renderer>();
    public List<Renderer> disabledMeshes = new List<Renderer>();
    //public Stack<Renderer> disabledRenderers = new Stack<Renderer>();

    
    private string relativeFilepath = Path.DirectorySeparatorChar + "brain.glb";
    private string path;
    GameObject brain;

    
    private float segOpacity = 1.0f;
    private float minOpacity;
    GameObject curSegment = null;

    //This makes the transparency work correctly
    void assignNewMaterial(GameObject child, int i){
        Color col = child.GetComponent<MeshRenderer>().material.GetColor("_Color");
        col.a = 1.0f;
        Material mat = new Material(shader);
        mat.SetColor("_Color", col);
        mat.renderQueue = 3000 + i*20;
        child.GetComponent<MeshRenderer>().material = mat;
    }
    void Awake(){ //singleton pattern at work
        current = this;
    }
    void Start()
    {
        colourSelect.current.onColourSelect += Pallete_onColourSelect;
        dropdown.ClearOptions();
        print(Application.streamingAssetsPath);
        path = Application.streamingAssetsPath + relativeFilepath;
        brain = Siccity.GLTFUtility.Importer.LoadFromFile(path);
        brain.transform.SetParent(this.transform);
        brain.transform.localPosition = new Vector3(-94.2f, -99.23f, -93.6f);
        brain.transform.localRotation = Quaternion.Euler(0.453f, -288.9f, 1.323f);
        int count = 0;
        foreach(Transform child in brain.transform){
            if(child.gameObject.GetComponent<Renderer>() != null && count != 1)
            {
                // Color col = child.gameObject.GetComponent<MeshRenderer>().material.color;
                // Material mat = baseMaterial;
                // mat.color = col;
                // child.gameObject.GetComponent<MeshRenderer>().material = mat;  
                segments.Add(child.gameObject);

            }
            if(count ==1)child.gameObject.GetComponent<Renderer>().enabled = false;
            count++;
            
        }
        for(int i = segments.Count - 1; i !=-1; i--){
            if(segments[i].GetComponent<Renderer>() != null)
            {
                assignNewMaterial(segments[i], segments.Count - i);  
                allRenderers.Add(segments[i].GetComponent<Renderer>());

                dropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString())); 
                //enabledRenderers.Push((segments[i].GetComponent<Renderer>()));
            }
        }
        curSegment = segments[0]; 
        dropdown.onValueChanged.AddListener(delegate{dropdownSegmentSelected(dropdown); });
        //opacitySlider = GameObject.Find("Opacity Slider").GetComponent<Slider>();
        //segOpacity = opacitySlider.value;
        //opacitySlider.gameObject.SetActive(false);

    }
    private void dropdownSegmentSelected(TMP_Dropdown dropdown){
        int newSelection = dropdown.value;
        print(newSelection);
        //clickSegmentButton(segments[newSelection]);
        curSegment = segments[newSelection];
        if(curSegment == null)print("Ye");
    }

    private void displayOpacitySlider(){
        opacitySlider.enabled = true;
        opacitySlider.gameObject.SetActive(true);
    }
    private void hideOpacitySlider(){
        opacitySlider.enabled = false;
        opacitySlider.gameObject.SetActive(false);
    }








    //Some event stuff - should probably be done in a separate class 
    //Gonna be used to let other scripts know which segment is currently selected. (Only AddAnnotation is currently subscribed to this event)

    public void SelectionManager_onColourSelect(object sender, EventArgs e){
        curSegment.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(1, 0.2f, 0.2f, segOpacity));
    }
    public void Pallete_onColourSelect(object sender, EventArgsColourData e){
        Color col = e.col;
        col.a = segOpacity;
        curSegment.GetComponent<MeshRenderer>().material.SetColor("_Color", col);
    }
    public event EventHandler<onSegmentSelectEventArgs> onSegmentSelect;
    public class onSegmentSelectEventArgs : EventArgs{
        public GameObject curSegment;
        public onSegmentSelectEventArgs(GameObject curSegment){
            this.curSegment = curSegment;
        }
        
    }
    private void clickSegmentButton(GameObject segment){
        curSegment = segment;
        onSegmentSelectEventArgs e = new onSegmentSelectEventArgs(curSegment);
        onSegmentSelect?.Invoke(this, e);
        //curSegment.GetComponent<MeshRenderer>().material.color = new Color(255,0,0);
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
        print("banana");
        print(curSegment);
        if(curSegment != null){
            segOpacity = newOp;
            Color color = curSegment.GetComponent<MeshRenderer>().material.color;
            color.a = segOpacity;
            if(color.a < minOpacity)curSegment.GetComponent<MeshRenderer>().enabled = false;
            else{
                if(curSegment.GetComponent<MeshRenderer>().enabled == false)curSegment.GetComponent<MeshRenderer>().enabled = true;
                print(color);
                print("funanana");
                curSegment.GetComponent<MeshRenderer>().material.color = color;
            }
        }
    }

    public void selectSegment(){
        if(currentlySelected == segments.Count-1)currentlySelected = 0;
        else currentlySelected++;
        clickSegmentButton(segments[currentlySelected]);
    }



















    public void selectSegment1(){
        if(currentlySelected == 0)
        {
            currentlySelected = -1;
            hideOpacitySlider();
        }else{
            currentlySelected = 0;
            displayOpacitySlider();
        } 
        clickSegmentButton(segments[0]);
    }
    public void selectSegment2(){
        if(currentlySelected == 1)
        {
            currentlySelected = -1;
            hideOpacitySlider();
        }else{
            currentlySelected = 1;
            displayOpacitySlider();
        } 
        clickSegmentButton(segments[1]);
    }
    public void selectSegment3(){
        if(currentlySelected == 2)
        {
            currentlySelected = -1;
            hideOpacitySlider();
        }else{
            currentlySelected = 2;
            displayOpacitySlider();
        } 
        clickSegmentButton(segments[2]);
    }
    public void selectSegment4(){
        if(currentlySelected == 3)
        {
            currentlySelected = -1;
            hideOpacitySlider();
        }else{
            currentlySelected = 3;
            displayOpacitySlider();
        } 
        clickSegmentButton(segments[3]);
    }
    public void selectSegment5(){
        if(currentlySelected == 4)
        {
            currentlySelected = -1;
            hideOpacitySlider();
        }else{
            currentlySelected = 4;
            displayOpacitySlider();
        } 
        clickSegmentButton(segments[4]);
    }
    
}
    // public void selectSegment1(){
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