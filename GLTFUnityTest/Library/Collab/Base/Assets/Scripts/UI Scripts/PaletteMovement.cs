using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PaletteMovement : MonoBehaviour, IDragHandler
{
    // Start is called before the first frame update
    private Vector2 centre;

    /*
    Calculates the change in angle from the centre of the wheel to the pointer between frames and rotates the
    wheel by that many degrees if the pointer is held down.
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
