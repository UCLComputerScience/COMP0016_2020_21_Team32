using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class is instantiated if a model is chosen from local storage</summary>
public class LoadedOrgan : Organ
{
    public LoadedOrgan(string filename):base(filename){
        base.centrePos = Vector3.zero;
        base.centreRot = Quaternion.identity; 
        base.segments = new List<GameObject>();
    }
}
