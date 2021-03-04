using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

///<summary>This class allows a UI element to be dragged by the cursor by implementing the IDragHandler interface</summary>
public class TextBoxDragDrop : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    public Canvas canvas;
    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnDrag(PointerEventData data){
        rectTransform.anchoredPosition += data.delta /canvas.scaleFactor; 
    }
}
