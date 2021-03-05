<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>This class is instantiated if a model is chosen from local storage</summary>
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
public class LoadedOrgan : Organ
{
    public LoadedOrgan(GameObject model){
        base.model = model;
        base.centrePos = Vector3.zero;//Vector3.zero;//new Vector3(-94.2f, -99.23f, -93.6f);
        base.centreRot = Quaternion.identity; //Quaternion.Euler(0.453f, -288.9f, 1.323f);
        base.segments = new List<GameObject>();
    }
}
