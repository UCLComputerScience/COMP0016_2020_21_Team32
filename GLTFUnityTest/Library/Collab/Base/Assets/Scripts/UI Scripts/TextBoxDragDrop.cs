﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TextBoxDragDrop : MonoBehaviour, IPointerDownHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    [SerializeField] Canvas canvas;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData data){
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
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}