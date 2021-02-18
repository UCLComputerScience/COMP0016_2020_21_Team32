using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationData{ 
    
    //Class used to store the data associated with a particular view of the model. Contains no methods as it must be parsed to and from JSON
    public Vector3 cameraCoordinates;
    public Quaternion cameraRotation;
    public string title;
    public string text;
    public List<Color> colours;
    
    public Vector3 annotationPosition;
    public Vector3 planeNormal;
    public Vector3 planePosition;

}
