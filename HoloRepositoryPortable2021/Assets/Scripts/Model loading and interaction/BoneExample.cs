using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class is instantiated if the example model of a bone is loaded in</summary>
public class BoneExample : Organ
{
    // Start is called before the first frame update
    public BoneExample(GameObject model){
        base.model = model;
        base.centrePos = new Vector3(-94.2f, -99.23f, -93.6f);
        base.centreRot = Quaternion.Euler(0.453f, -288.9f, 1.323f);
        base.segments = new List<GameObject>();
    }
}
