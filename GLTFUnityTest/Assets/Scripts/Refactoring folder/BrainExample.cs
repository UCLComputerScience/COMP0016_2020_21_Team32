using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainExample : Organ
{   
    public BrainExample(GameObject model){
        base.model = model;
        base.centrePos = new Vector3(-94.2f, -99.23f, -93.6f);
        base.centreRot = Quaternion.Euler(0.453f, -288.9f, 1.323f);
        base.segments = new List<GameObject>();
    }
    public override void initialiseModel()
    {
        int count = 0;
        model.transform.SetParent(parent.transform);
        model.transform.localPosition = centrePos;
        model.transform.localRotation = centreRot;
        foreach(Transform child in model.transform){
            if(child.gameObject.GetComponent<Renderer>() != null && count != 1)segments.Add(child.gameObject);
            if(count ==1)child.gameObject.GetComponent<Renderer>().enabled = false;
            else if(child.gameObject.GetComponent<Camera>() != null) child.gameObject.SetActive(false);
            count++;   
        }
    }

}
