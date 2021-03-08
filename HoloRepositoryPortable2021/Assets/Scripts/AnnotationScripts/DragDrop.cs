using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
///<summary>Class is attached as a component to the AnnotationPin GameObject, and allows the it
/// to be dragged and dropped.</summary>
public class DragDrop : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] Annotation annotation;
    public Canvas canvas;
    private RectTransform rectTransform; //stores position, size, anchor, pivot of a rectangle
    void awake(){
        canvas = GetComponentInParent<Canvas>();
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    /*Implemented from the IDragHandler interface. Is only invoked if the pointer is held down and is within the 
    rectTransform of the annotationPin. Changes its position each frame the same amount the pointer's position changes each frame, hence
    providing the ability to drag the pin.*/
    public void OnDrag(PointerEventData data){
        rectTransform.anchoredPosition += data.delta /canvas.scaleFactor; 

    }

    /*Implemented from the IEndDragHandler interface. Called when the pointer is released after dragging. This passes the current position of 
    the annotation pin to the Annotation*/
    public void OnEndDrag(PointerEventData data){
        annotation.show(this.transform.position); 
    }
}
