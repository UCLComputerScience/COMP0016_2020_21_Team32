using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Organ 
{
    /* 
    All subclasses will have a constructor that takes in the model loaded as the model parameter.
    The parent GameObject will be set in the Model Handler class.
    */
    public GameObject parent{get; set;}
    public GameObject model{get; set;}
    public List<GameObject> segments{get; set;}
    public Vector3 centrePos{get; protected set;}
    public Quaternion centreRot{get; protected set;}

    // public Organ(GameObject model){
    //     this.model = model;
    //     centrePos = Vector3.zero;
    //     centreRot = Quaternion.identity;
    // }
    public virtual void initialiseModel(){
        Debug.Log("Super");
        model.transform.SetParent(parent.transform);
        model.transform.localPosition = centrePos;
        model.transform.localRotation = centreRot;
        foreach(Transform child in model.transform){
             if(child.gameObject.GetComponent<Renderer>() != null)segments.Add(child.gameObject);
             else if(child.gameObject.GetComponent<Camera>() != null)child.gameObject.SetActive(false);
        }
    }
    
}
