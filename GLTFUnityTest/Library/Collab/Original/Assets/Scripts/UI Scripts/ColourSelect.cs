using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

///<summary>
///This class is attatched to the colour palette gameobject. Monitors the position of the pointer each frame and determines 
///whether or not the user has double clicked over the colour palette. If a double click is detected, the pixel data beneath the pointer on that
//frame is passed to an event which is invoked so any listener classes can make use of the data. 
//Tutorial used to develop this class: https://www.youtube.com/watch?v=n5WOD5zoggg
///</summary>

public class ColourSelect : MonoBehaviour
{
<<<<<<< HEAD
    private Texture2D colours;
    private Vector2 mousePos; 
    private RectTransform rect;
    [SerializeField] GameObject UIBlocker;
=======
     

    private Texture2D colours;

    private Vector2 mousePos = new Vector2();

    private RectTransform rect;

    [SerializeField] GameObject UIBlocker;

>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    CircleCollider2D col; 
    private int width;
    private int height;
    [SerializeField] const float DOUBLE_CLICK_TIME = 0.2f;
    private float lastTimeClicked;
    void Start()
    {
<<<<<<< HEAD
        /*initialise variables*/
        mousePos = new Vector2();
=======
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
        RawImage image = GetComponent<RawImage>();
        colours = image.texture as Texture2D; 
        rect = image.GetComponent<RectTransform>(); //RectTransform of the colour palette
        width = (int) rect.rect.width;
        height = (int) rect.rect.height;
        print(width);
        print(height);
        col = GetComponent<CircleCollider2D>();
    }

    //The update function is called every frame
    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out mousePos);
        //Converts the screen space coordinates of the pointer to local space coordinates of the RectTransform component of 
        //the colour palette. Stores the updated coordinates in the mousePos variable.

        //The origin of our image's RectTransform is at its centre. The pixel data of the texture is stored in a 2D array with
        //the first element in the bottom left, so the pointer's position must be recalculated to ensure that it samples the 
        //correct pixel.
        mousePos.x = width - (width/2 -mousePos.x); 
        mousePos.y = Mathf.Abs((height/2 - mousePos.y) - height);

        //If the user double clicks within the circle collider attatched to the colour palette, fire the onColourSelect event, 
        //letting any subscribers of that event know what colour has been selected.
        if(Input.GetMouseButtonDown(0)){
            if(isInside(col, Input.mousePosition) && doubleClick(DOUBLE_CLICK_TIME) && !UIBlocker.activeInHierarchy){
                var col = colours.GetPixel((int)mousePos.x, (int)mousePos.y);
                EventManager.current.onColourSelect(col);
            }
        }
    }

    //Determines the time between consecutive clicks by the user and returns true if this is less than the time allowed for a double click.
    private bool doubleClick(float maxDoubleClickTime){
        bool returnVal = false;
        float timeSinceLastClick = Time.time - lastTimeClicked;
        print(timeSinceLastClick);
        if(timeSinceLastClick < maxDoubleClickTime){ 
            returnVal = true;
        }
        lastTimeClicked = Time.time;
        return returnVal;
    }

    //Returns true if a position is within a collider, false otherwise.
    private bool isInside(CircleCollider2D collider, Vector3 pos){
        Vector3 closestPoint = collider.ClosestPoint(pos);
        return closestPoint == pos;
    }

}

