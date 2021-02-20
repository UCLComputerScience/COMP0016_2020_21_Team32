using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidneyExample : Organ
{
    // Start is called before the first frame update
    public KidneyExample(GameObject model){
        base.model = model;
        base.centrePos = new Vector3(240f, -388f, -120f);//Vector3.zero;//new Vector3(-94.2f, -99.23f, -93.6f);
        base.centreRot = Quaternion.identity; //Quaternion.Euler(0.453f, -288.9f, 1.323f);
        base.segments = new List<GameObject>();
    }
}
