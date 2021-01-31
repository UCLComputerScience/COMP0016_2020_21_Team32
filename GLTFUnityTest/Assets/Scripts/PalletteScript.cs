using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class PalletteScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    List<Collider2D> colourSegments;
    public float speed = 1.0f;
    Vector2 mousePos;
    Quaternion rot;
    // Update is called once per frame
    public float baseAngle = 0.0f;

    RawImage image;

    Texture2D colours;
    RectTransform rect;
    int width;
    int height;
    public void Start(){
        mousePos = Input.mousePosition;
        image = GetComponent<RawImage>();
        colours = image.texture as Texture2D;
        rect = image.GetComponent<RectTransform>();
        int width = (int) rect.rect.width;
        int height = (int) rect.rect.height;
    }
    public void OnPointerEnter(PointerEventData data){

    }
    public void OnPointerExit(PointerEventData data){

    }

    public void OnPointerDown(PointerEventData data){
        //print(colours.GetPixel((int) mousePos.x, (int) mousePos.y));
        Vector3 dir = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - dir;
        baseAngle  = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        baseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

    }
    public void OnPointerUp(PointerEventData data){

    }
    public void OnBeginDrag(PointerEventData data){
    }
    public void OnDrag(PointerEventData data){
        Vector3 dir = Camera.main.WorldToScreenPoint(transform.position);
        dir = Input.mousePosition - dir;
        float angle  = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - baseAngle;
        //print(angle);
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rot;

    }
    public void OnEndDrag(PointerEventData data){
        print("No more draggin");
    }
}

