using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>
///This class is attatched to the colour palette gameobject. Monitors the position of the pointer each frame and determines 
///whether or not the user has double clicked over the colour palette. If a double click is detected, the pixel data beneath the pointer on that
//frame is passed to an event which is invoked so any listener classes can make use of the colour data. 
//Tutorial used to help develop this class: https://www.youtube.com/watch?v=n5WOD5zoggg
///</summary>

public class ColourSelector : MonoBehaviour
{

    private const float DOUBLE_CLICK_TIME = 0.2f;
    private Texture2D colours;
    private Vector2 mousePos; 
    private RectTransform rectTransform;
    private Rect boundingRectangle;
    [SerializeField] GameObject UIBlocker;
    private CircleCollider2D col; 
    private int width;
    private int height;
    private float lastTimeClicked;
    void Start()
    {
        /*initialise variables*/
        mousePos = new Vector2();
        RawImage image = GetComponent<RawImage>();
        rectTransform = image.GetComponent<RectTransform>(); //RectTransform of the colour palette
        boundingRectangle = rectTransform.rect; 
        colours = image.texture as Texture2D; 
        width = (int) boundingRectangle.width;
        height = (int) boundingRectangle.height;
        col = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, null, out mousePos);
        /*Converts the screen space coordinates of the pointer to local space coordinates of the RectTransform component of 
        the colour palette. Stores the updated coordinates in the mousePos variable.*/

        /*The dimensions of the bounding rectangle of the colour palette in the GUI are not equal to the dimensions of the PNG of the
        colour palette image, which is imported as a Texture2D. So that the user picks the correct colour, the pointer's position must
        be recalculated*/
        mousePos.x = Mathf.Clamp (0,(int)(((mousePos.x-boundingRectangle.x)*colours.width)/boundingRectangle.width),colours.width);
        mousePos.y = Mathf.Clamp (0,(int)(((mousePos.y-boundingRectangle.y)*colours.height)/boundingRectangle.height),colours.height);

        /*If the user double clicks within the circle collider attatched to the colour palette, given that the UIBlocker is not
        enabled, fire the onColourSelect event letting any subscribers of that event know what colour has been selected.*/
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

