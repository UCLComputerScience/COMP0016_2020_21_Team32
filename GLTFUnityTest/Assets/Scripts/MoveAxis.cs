using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAxis : MonoBehaviour
{
    Vector3 prevMousePos;
    private bool dropped;
    private bool dragging = false;
    // Start is called before the first frame update
    void OnMouseDown(){
        dragging = true;
        prevMousePos = Input.mousePosition;

    }
    void Update(){
        if(Input.GetMouseButton(0) && dragging){
            Vector3 deltaMouse = Input.mousePosition - prevMousePos;
            this.gameObject.transform.position += new Vector3(deltaMouse.x, deltaMouse.y, 0f);
            prevMousePos = Input.mousePosition;
        }
    }
    void OnMouseUp(){
        dragging = false;
        CameraMovement.target = this.transform;
        this.gameObject.SetActive(false);
    }
}
