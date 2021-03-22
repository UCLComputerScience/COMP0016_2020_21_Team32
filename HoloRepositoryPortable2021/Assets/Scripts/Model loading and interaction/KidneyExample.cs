using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class is instantiated if the example model of the kidneys is loaded in</summary>
public class KidneyExample : Organ
{
    public KidneyExample(string filename):base(filename){
        base.centrePos = new Vector3(76f, -151f, 114f);
        base.centreRot = Quaternion.Euler(-0.36f, -22.049f, -0.889f); 
        base.segments = new List<GameObject>();
    }
}
