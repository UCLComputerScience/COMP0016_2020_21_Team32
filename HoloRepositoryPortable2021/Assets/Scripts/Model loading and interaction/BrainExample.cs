using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 ///<summary>This class is instantiated if the example model of a brain is loaded in</summary>
public class BrainExample : Organ
{   
    public BrainExample(string filename): base(filename){
        base.centrePos = new Vector3(-161f, 14f, 27f);
        base.centreRot = Quaternion.Euler(0f, 72.838f, 0f);
    }
    public override void setParent(GameObject parent)
    {
        GameObject temp = null;
        int count = 0;
        model.transform.SetParent(parent.transform);
        foreach(Transform child in model.transform){
            if(child.gameObject.GetComponent<Renderer>() != null && count != 1)segments.Add(child.gameObject);
            if(count ==1) temp = child.gameObject;
            else if(child.gameObject.GetComponent<Camera>() != null) child.gameObject.SetActive(false);
            count++;   
        }
        segments.Add(temp);
    }

}
