using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class is instantiated if the example model of a bone is loaded in</summary>
public class BoneExample : Organ
{
    public BoneExample(string filename): base(filename){
        base.centrePos = Vector3.zero;
        base.centreRot = Quaternion.identity;
        base.segments = new List<GameObject>();
    }
}
