using Siccity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//loads the model at runtime


public class LoadBrain : MonoBehaviour
{
    List<Segment> segments = new List<Segment>();
    //public Stack<Renderer> enabledRenderers = new Stack<Renderer>();
    public List<Renderer> allRenderers = new List<Renderer>();
    public List<Renderer> disabledMeshes = new List<Renderer>();
    //public Stack<Renderer> disabledRenderers = new Stack<Renderer>();

    
    string relativeFilepath = "\\brain.glb";
    string path;
    GameObject brain;


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
    }
    // public void disableRenderer(){
    //     if(enabledRenderers.Count !=0){
    //         Debug.Log("disabled");
    //         enabledRenderers.Peek().enabled = false;
    //         disabledRenderers.Push(enabledRenderers.Pop());
    //     }
    // }
    // public void enableRenderer(){
    //     if(disabledRenderers.Count !=0){
    //         Debug.Log("enabled");
    //         disabledRenderers.Peek().enabled = true;
    //         enabledRenderers.Push(disabledRenderers.Pop());
    //     }
    // }
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

    public void selectSegment1(){
        clickSegmentButton(segments[0]);
    }
    public void selectSegment2(){
        clickSegmentButton(segments[1]);
    }
    public void selectSegment3(){
        clickSegmentButton(segments[2]);
    }
    public void selectSegment4(){
        clickSegmentButton(segments[3]);
    }
    public void selectSegment5(){
        clickSegmentButton(segments[4]);
    }
    


    public float SegOpacity = 1.0f;

    public void AdjustOpacity(float newOp) {
        SegOpacity = newOp;
        // segments[0].GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, SegOpacity);
        Color color = segments[0].seg.GetComponent<MeshRenderer>().material.color;
        color.a = SegOpacity;
        segments[0].seg.GetComponent<MeshRenderer>().material.color = color;
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