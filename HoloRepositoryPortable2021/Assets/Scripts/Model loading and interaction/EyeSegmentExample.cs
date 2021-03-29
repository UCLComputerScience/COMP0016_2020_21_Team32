using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class is instantiated if the example model of an eye segment is loaded in</summary>
public class EyeSegmentExample : Organ{

    public EyeSegmentExample(string filename):base(filename){
        base.centrePos = new Vector3(30, -103, -9f);
        base.centreRot = Quaternion.identity;
    }
}

