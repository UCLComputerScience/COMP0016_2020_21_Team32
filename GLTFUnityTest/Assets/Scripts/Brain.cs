using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public List<GameObject> segments; 
    public List<Renderer> renderers; 

    public Brain(List<GameObject> segments){
        this.segments = segments;
        foreach(GameObject child in segments){
            if(child.GetComponent<Renderer>() != null)renderers.Add(child.GetComponent<Renderer>());
        }
    }
    public void selectSegment(Renderer renderer){
        renderer.enabled = true;
        foreach(Renderer ren in renderers){
            if(!Object.Equals(ren, renderer)) ren.enabled = false;
        }
    }
}