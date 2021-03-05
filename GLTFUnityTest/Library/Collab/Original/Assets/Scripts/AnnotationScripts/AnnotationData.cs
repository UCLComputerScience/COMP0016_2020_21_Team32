using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
///<summary>Class representing an annotation. All fields are serializable to allow it to be parsed to and from JSON. </summary>
public class AnnotationData{ 
=======
public class AnnotationData{ 
    
    //Class used to store the data associated with a particular view of the model. Contains no methods as it must be parsed to and from JSON
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    public Vector3 cameraCoordinates;
    public Quaternion cameraRotation;
    public string title;
    public string text;
    public List<Color> colours;
    public Vector2 screenDimensions;
    public Vector3 annotationPosition;
    public Vector3 planeNormal;
    public Vector3 planePosition;

}
