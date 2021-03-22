using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class is instantiated if the example model of an abdomen is loaded in</summary>
public class AbdomenExample : Organ
{
    public AbdomenExample(string filename) : base(filename){
        base.centrePos = new Vector3(-127f, -19f, -117f);
        base.centreRot = Quaternion.Euler(0f, 63.548f, 0f);
        base.segments = new List<GameObject>();
    }
}
