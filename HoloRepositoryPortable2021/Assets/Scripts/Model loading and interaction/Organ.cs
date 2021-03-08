using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

///<summary>This abstract class represents the loaded organ.</summary>

public abstract class Organ 
{    
    
    public GameObject model{get; set;} //The model itself
    private List<Transform> segmentTransforms; //The children of the model (ie, its segments)
    public List<GameObject> segments{get; set;} 
    public Vector3 centrePos{get; protected set;} //how the model should be positioned relative to its parent
    public Quaternion centreRot{get; protected set;} //how the model should be orientated relative to its parent
    

    /*Virtual method that may or may not be overridden by subclasses. The model is loaded in at runtime and 
    is made a child of a parent gameObject. It's local position and rotation relative to this parent are set, and all the 
    children of the model are stored in a list. If a camera exists in the loaded model, it is set to inactive.*/
    public virtual void initialiseModel(GameObject parent){
        model.transform.SetParent(parent.transform);
        model.transform.localPosition = centrePos;
        model.transform.localRotation = centreRot;
        segmentTransforms = model.GetComponentsInChildren<Transform>().ToList();
        segments = new List<GameObject>();
        foreach(Transform t in segmentTransforms){
            if(t.gameObject.GetComponent<Renderer>() != null)segments.Add(t.gameObject);
            else if(t.gameObject.GetComponent<Camera>() != null)t.gameObject.SetActive(false);
        }
    }
    
}
