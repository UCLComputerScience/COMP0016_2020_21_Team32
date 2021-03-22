using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeSegmentExample : Organ{

    public EyeSegmentExample(string filename):base(filename){
        base.centrePos = new Vector3(30, -103, -9f);
        base.centreRot = Quaternion.identity;
        base.segments = new List<GameObject>();
    }
}

