using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Class representing an annotation. All fields are serializable to allow it to be parsed to and from JSON. </summary>
public class AnnotationData{ 
    public Vector3 cameraCoordinates;
    public Quaternion cameraRotation;
    public string title;
    public string text;
    public List<Color> colours;
    public Vector2 screenDimensions;
    public Vector3 annotationPosition;
    public Vector3 planeNormal;
    public Vector3 planePosition;
    public Vector3 cameraDisplacement;

}
