using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungExample : Organ
{
    // Start is called before the first frame update
    public LungExample(string filename):base(filename){
        base.centrePos = new Vector3(-147f, -88f, 64f);
        base.centreRot = Quaternion.Euler(2.632f, 76.56f, -90.901f);
        base.segments = new List<GameObject>();
    }
}
