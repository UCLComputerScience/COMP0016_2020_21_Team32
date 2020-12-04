using Siccity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//loads the model at runtime


public class LoadBrain : MonoBehaviour
{
    List<GameObject> segments = new List<GameObject>();
    public Stack<Renderer> enabledRenderers = new Stack<Renderer>();
    public List<Renderer> disabledMeshes = new List<Renderer>();
    public Stack<Renderer> disabledRenderers = new Stack<Renderer>();

    
    string filepath = "Assets\\SampleGLTF\\brain.glb";
    GameObject brain;


    void Start()
    {

        brain = Siccity.GLTFUtility.Importer.LoadFromFile(filepath);
        brain.transform.SetParent(this.transform);
        brain.transform.localPosition = new Vector3(-94.2f, -99.23f, -93.6f);
        brain.transform.localRotation = Quaternion.Euler(0.453f, -288.9f, 1.323f);
        foreach(Transform child in brain.transform){
            segments.Add(child.gameObject);
        }
        for(int i = segments.Count - 1; i !=-1; i--){
            if(segments[i].GetComponent<Renderer>() != null)enabledRenderers.Push((segments[i].GetComponent<Renderer>()));
            Debug.Log(i);
            //child.AddComponent<Rigidbody>();
        }
    }
    public void disableRenderer(){
        if(enabledRenderers.Count !=0){
            Debug.Log("disabled");
            enabledRenderers.Peek().enabled = false;
            disabledRenderers.Push(enabledRenderers.Pop());
        }
    }
    public void enableRenderer(){
        if(disabledRenderers.Count !=0){
            Debug.Log("enabled");
            disabledRenderers.Peek().enabled = true;
            enabledRenderers.Push(disabledRenderers.Pop());
        }
    }

    public float SegOpacity = 0.8f;

    public void AdjustOpacity(float newOp) {
        SegOpacity = newOp;
        // segments[0].GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, SegOpacity);
        Color color = segments[0].GetComponent<MeshRenderer>().material.color;
        color.a = SegOpacity;
        segments[0].GetComponent<MeshRenderer>().material.color = color;
    }
}