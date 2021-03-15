using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class is instantiated if a model is chosen from local storage</summary>
public class LoadedOrgan : Organ
{
    public LoadedOrgan(string filename):base(filename){
        base.centrePos = Vector3.zero;//Vector3.zero;//new Vector3(-94.2f, -99.23f, -93.6f);
        base.centreRot = Quaternion.identity; //Quaternion.Euler(0.453f, -288.9f, 1.323f);
        base.segments = new List<GameObject>();
    }
}
