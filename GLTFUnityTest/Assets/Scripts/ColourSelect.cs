using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ColourSelect : MonoBehaviour
{
    // Start is called before the first frame update
    public static ColourSelect current;
    public event EventHandler<EventArgsColourData> onColourSelect; 

    public Texture2D colours;

    public Vector2 mousePos = new Vector2();

    public RectTransform rect;
    private int width;
    private int height;
    [SerializeField] const float DOUBLE_CLICK_TIME = 0.2f;
    [SerializeField] const float CURSOR_DRAG_TIME = 0.05f;
    private float lastTimeClicked;

    private Color[] pixelData;
    void Awake(){
        current = this;
    }
    void Start()
    {
        RawImage image = GetComponent<RawImage>();
        colours = image.texture as Texture2D;
        pixelData = colours.GetPixels();
        rect = image.GetComponent<RectTransform>();
        width = (int) rect.rect.width;
        height = (int) rect.rect.height;
        print(width);
        print(height);
    }

    // Update is called once per frame
    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, Input.mousePosition, null, out mousePos);

        /***ISSUE: when the palette is rotated the rect changes so the modified x and y values change. 
        Means you can select colours not on the palette and the selected segment will change colour.
        ***/


        //Centre of texture is currently (0,0). Pixel data isn't stored in this way - we need to make it so
        //bottom left = (0,0) and top right = (width, height);
        mousePos.x = width - (width/2 -mousePos.x);
        mousePos.y = Mathf.Abs((height/2 - mousePos.y) - height);
        if(Input.GetMouseButton(0)){
            if(mousePos.x > -1 && mousePos.y > -1 && doubleClick()){ //if mouse is within the rect of the palette
                Debug.Log("here");
                var col = colours.GetPixel((int)mousePos.x, (int)mousePos.y);
                EventArgsColourData e = new EventArgsColourData(col);
                onColourSelect?.Invoke(this, e);
            }
        }
    }
    private bool doubleClick(){
        bool returnVal = false;
        float timeSinceLastClick = Time.time - lastTimeClicked;
        print(timeSinceLastClick);
        if(timeSinceLastClick < DOUBLE_CLICK_TIME && timeSinceLastClick > CURSOR_DRAG_TIME){
            returnVal = true;
        }
        lastTimeClicked = Time.time;
        return returnVal;
        
    }

}
public class EventArgsColourData : EventArgs{
    public Color col;
    public EventArgsColourData(Color col){
        this.col = col;
    }
}
