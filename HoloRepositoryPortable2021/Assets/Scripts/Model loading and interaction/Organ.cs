using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Siccity.GLTFUtility;

///<summary>This abstract class represents the loaded organ.</summary>

public abstract class Organ 
{    
    
    public GameObject model = null; //The model itself
    private List<Transform> segmentTransforms; 
    public List<GameObject> segments{get; set;} //The children of the model (ie, its segments)
    public Vector3 centrePos{get; protected set;} //how the model should be positioned relative to its parent
    public Quaternion centreRot{get; protected set;} //how the model should be orientated relative to its parent
    

    public Organ(string filename){
        Importer.LoadFromFileAsync(filename, new ImportSettings(), onLoaded); //GLTF Utility call
    }
    private void onLoaded(GameObject loadedModel, AnimationClip[] clips){ //passed as callback action to LoadFromFileAsync.
        model = loadedModel;
    }

    /*Virtual method that may or may not be overridden by subclasses. The model is loaded in at runtime and 
    is made a child of a parent gameObject. It's local position and rotation relative to this parent are set, and all the 
    children of the model are stored in a list. If a camera exists in the loaded model, it is set to inactive.*/
    public virtual void setParent(GameObject parent){
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
