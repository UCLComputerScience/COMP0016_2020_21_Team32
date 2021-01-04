using Siccity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//loads the model at runtime

//ONE ADDITIONAL COMMENT
public class LoadBrain : MonoBehaviour
{
    List<Segment> segments = new List<Segment>();
    //public Stack<Renderer> enabledRenderers = new Stack<Renderer>();
    public List<Renderer> allRenderers = new List<Renderer>();
    public List<Renderer> disabledMeshes = new List<Renderer>();
    //public Stack<Renderer> disabledRenderers = new Stack<Renderer>();

    
    private string relativeFilepath = "\\brain.glb";
    private string path;
    GameObject brain;

    Slider opacitySlider;
    private float segOpacity;
    private int currentlySelected = -1;


    void Start()
    {
        print(Application.streamingAssetsPath);
        path = Application.streamingAssetsPath + relativeFilepath;
        brain = Siccity.GLTFUtility.Importer.LoadFromFile(path);
        brain.transform.SetParent(this.transform);
        brain.transform.localPosition = new Vector3(-94.2f, -99.23f, -93.6f);
        brain.transform.localRotation = Quaternion.Euler(0.453f, -288.9f, 1.323f);
        foreach(Transform child in brain.transform){
            if(child.gameObject.GetComponent<Renderer>() != null)segments.Add(new Segment(child.gameObject));
        }
        for(int i = segments.Count - 1; i !=-1; i--){
            if(segments[i].seg.GetComponent<Renderer>() != null)
            {
                allRenderers.Add(segments[i].seg.GetComponent<Renderer>());
                //enabledRenderers.Push((segments[i].GetComponent<Renderer>()));
            }
        }
        opacitySlider = GameObject.Find("Opacity Slider").GetComponent<Slider>();
        segOpacity = opacitySlider.value;
        opacitySlider.gameObject.SetActive(false);

    }
    private void displayOpacitySlider(){
        opacitySlider.enabled = true;
        opacitySlider.gameObject.SetActive(true);
    }
    private void hideOpacitySlider(){
        opacitySlider.enabled = false;
        opacitySlider.gameObject.SetActive(false);
    }

    
    private void selectRenderer(Renderer rend){
        foreach(Renderer r in allRenderers){
            if(!Object.Equals(r, rend))r.GetComponent<Renderer>().enabled = false;
        }
    }
    private void clickSegmentButton(Segment segment){
        segment.selected = !segment.selected;
        if(segment.selected){
            segment.seg.GetComponent<Renderer>().enabled = true;
            foreach(Segment s in segments){
                if(!Object.Equals(segment.seg, s.seg)){
                    s.selected = false;
                    s.seg.GetComponent<Renderer>().enabled = false;
                }else{
                }
            }
        }else{
            foreach(Segment s in segments){
                s.selected = false;
                s.seg.GetComponent<Renderer>().enabled = true;
            }
        }
    }
    // public void selectSegment(int n){
    //     if(currentlySelected != n){
    //         currentlySelected = -1;
    //         hideOpacitySlider();
    //     }else{
    //         currentlySelected = n;
    //         displayOpacitySlider();
    //     }
    //     clickSegmentButton(segments[currentlySelected]);
    // }


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
    


    public void AdjustOpacity(float newOp) {
        if(currentlySelected != -1){
            segOpacity = newOp;
            Color color = segments[currentlySelected].seg.GetComponent<MeshRenderer>().material.color;
            color.a = segOpacity;
            segments[currentlySelected].seg.GetComponent<MeshRenderer>().material.color = color;
        }
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