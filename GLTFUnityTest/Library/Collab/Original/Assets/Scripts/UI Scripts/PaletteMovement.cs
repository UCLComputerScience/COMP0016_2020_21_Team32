<<<<<<< HEAD
using System.Collections;
=======
﻿using System.Collections;
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

<<<<<<< HEAD
///<summary> This class implements the IDragHandler interface. Allows the gameobject this script is attatched to to be 
///rotated about the z axis on mouse drag events.
///</summary>
public class PaletteMovement : MonoBehaviour, IDragHandler
{
=======
public class PaletteMovement : MonoBehaviour, IDragHandler
{
    // Start is called before the first frame update
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    private Vector2 centre;

    /*
    Calculates the change in angle from the centre of the wheel to the pointer between frames and rotates the
<<<<<<< HEAD
    wheel by that many degrees (about the z axis) if the pointer is held down.
=======
    wheel by that many degrees if the pointer is held down.
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    */
    public void OnDrag(PointerEventData eventData){
        centre = this.transform.position;
        Vector2 curPos = eventData.position;
        Vector2 dragVec = eventData.delta;
        Vector2 prevPos = curPos - dragVec;
        float dTheta = Vector2.SignedAngle(centre - curPos, centre - prevPos);
        transform.RotateAround(centre, -Vector3.forward, dTheta);
    }

}
