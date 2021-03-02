using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    String title;

    // private Ray ray;
    // private RaycastHit hit;
    // private RaycastHit[] hits;
    Organ organ;

    [SerializeField] GameObject sphere;
    [SerializeField] Canvas canvas;

    [SerializeField] Annotation annotation;

    List<Collider> colliders;
    private RectTransform rectTransform; //stores position, size, anchor, pivot of a rectangle
    void awake(){
        annotation.hide();
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    void OnEnable(){

    }

    public void OnPointerDown(PointerEventData data){
        // organ = ModelHandler.organ;
        // foreach(GameObject g in organ.segments){
        //     colliders.Add(g.AddComponent<SphereCollider>());
        // }
        Debug.Log("Mouse down");
    }
    public void OnDrag(PointerEventData data){
        Debug.Log("Draggin");
        rectTransform.anchoredPosition += data.delta /canvas.scaleFactor; 
        //movement delta - amount mouse moved since previous frame
        //must be divided by canvas scale factor because of the difference between mouse movement and canvas scale. This will vary 
        //due to the canvas adjusting itself to fit on every screen.
    }
    public void OnBeginDrag(PointerEventData data){
        Debug.Log("Beginnin draggin");
    }
    // public void OnEndDrag(PointerEventData data){
    //     title = "Annotation #" + annotation.annotationId; 
    //     annotation.show(title, "enter...", (string input) => annotation.save(title, input, this.transform.position),() => Debug.Log("Cancel")); 

    // }
    public void OnEndDrag(PointerEventData data){
        title = "Annotation #" + annotation.annotationId;
        // ray = Camera.main.ScreenPointToRay(data.position);
        // hits = Physics.RaycastAll(ray);
        // foreach(RaycastHit hit in hits){
        //     if(hit.transform.gameObject.name == "Plane"){
        //         annotation.show(hit.transform.GetComponent<MeshCollider>().ClosestPoint(hit.point));
        //         return;
        //     }
        // }
        // if(Physics.Raycast(ray, out hit)){
        //     Debug.Log(hit.transform.GetComponent<MeshCollider>().ClosestPoint(hit.point));
        //     annotation.show(hit.transform.GetComponent<MeshCollider>().ClosestPoint(hit.point));
        //     //sphere.transform.SetParent(hit.transform);
        //     Debug.Log(hit.transform);
        // }else{
        //     this.gameObject.SetActive(false);
        // } 
        Debug.Log(this.transform.position);
        
        annotation.show(this.transform.position); 
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
