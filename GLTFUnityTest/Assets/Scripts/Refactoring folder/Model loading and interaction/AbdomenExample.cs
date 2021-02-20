using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbdomenExample : Organ
{
    // Start is called before the first frame update
    public AbdomenExample(GameObject model){
        base.model = model;
        base.centrePos = new Vector3(-52f, -86f, -175f);
        base.centreRot = Quaternion.Euler(0.453f, -288.9f, 1.323f);
        base.segments = new List<GameObject>();
    }
}
