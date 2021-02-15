using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnotationPIn3D : MonoBehaviour
{
    private bool dragging = false;
    // Start is called before the first frame update
    Plane plane;

    [SerializeField] Organ organ;
    Ray ray;

    RaycastHit hit;
    void OnEnable()
    {
        organ = ModelHandler.organ;
        StartCoroutine(getOrganPosition());
        
    }

    IEnumerator getOrganPosition(){
        Debug.Log(organ);
        yield return new WaitUntil(() => organ.segments[0] != null);
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, organ.segments[0].transform.position.z);
    }
    void Update()
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, organ.segments[0].transform.position.z);

    //     if(Input.GetMouseButtonDown(0)){
    //         if(!dragging)return;
    //         ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         if(Physics.Raycast(ray, out hit)){
    //             transform.position = hit.transform.position;
    //         }
    // }
    }
    void OnMouseButtonDown(){
        dragging = true;
    }
    void OnMouseUp(){
        dragging = false;
    }
}
