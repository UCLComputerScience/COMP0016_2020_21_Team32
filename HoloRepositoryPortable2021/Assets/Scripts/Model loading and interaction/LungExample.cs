using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungExample : Organ
{
    // Start is called before the first frame update
    public LungExample(string filename):base(filename){
        base.centrePos = new Vector3(-229f, 23f, 191f);
        base.centreRot = Quaternion.Euler(4.348f, 78.478f, -103.107f);
        base.segments = new List<GameObject>();
    }
}
