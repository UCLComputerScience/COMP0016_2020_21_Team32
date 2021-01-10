using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    String title;
    [SerializeField] Canvas canvas;

    [SerializeField] Annotation annotation;
    public event EventHandler placeAnnotationPin;
    private RectTransform rectTransform; //stores position, size, anchor, pivot of a rectangle
    void awake(){
        annotation.hide();
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data){
        Debug.Log("Mouse down");
    }
    public void OnDrag(PointerEventData data){
        Debug.Log("Draggin");
        rectTransform.anchoredPosition += data.delta /canvas.scaleFactor; //movement delta - amount mouse moved since previous frame
        //must be divided by canvas scale factor because of the difference between mouse movement and canvas scale. This will vary 
        //due to the canvas adjusting itself to fit on every screen.
    }
    public void OnBeginDrag(PointerEventData data){
        Debug.Log("Beginnin draggin");
    }
    public void OnEndDrag(PointerEventData data){
        Debug.Log("StoppingDraggin");
        title = "Annotation #" + annotation.annotationId;
        annotation.show(title, "enter...", (string input) => Debug.Log(input),() => Debug.Log("Cancel")); 

    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
