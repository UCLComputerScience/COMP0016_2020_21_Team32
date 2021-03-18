using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>Used to draw the lines of the 3D coordinate axis whenever the pivot controller is enabled</summary>
public class GenerateLines : MonoBehaviour
{
    private const float MODEL_RADIUS_TO_LINE_WIDTH = 750f;
    private const float LINE_LENGTH = 5000f;
    private LineRenderer xLine;
    private LineRenderer yLine;
    private LineRenderer zLine;
    


    void Awake(){
        /*initialise variables*/
        xLine = transform.Find("xAxis").GetComponent<LineRenderer>();
        yLine = transform.Find("yAxis").GetComponent<LineRenderer>();
        zLine = transform.Find("zAxis").GetComponent<LineRenderer>();
    }

    /*Draws a line in a specified direction with a specific colour */
    private void drawAxis(LineRenderer line, Vector3 axisDir, Color color){
        line.material.SetColor("_Color", color); 
        line.useWorldSpace = true;
        line.sortingOrder = 5;
        line.startWidth = line.endWidth = ModelHandler.current.modelRadius / MODEL_RADIUS_TO_LINE_WIDTH;
        line.startColor = line.endColor = color;
        line.SetPositions(new Vector3[]{-axisDir*LINE_LENGTH, axisDir*LINE_LENGTH}); //arbitrarily large length of line in the direction specified 
    }
    /*Draws 3 lines, 1 for each axis in 3D space*/
    public void draw(){
        drawAxis(xLine, Vector3.right, new Color(1f, 0f, 0f, 0.5f)); //draw line in x axis
        drawAxis(yLine, Vector3.up, new Color(0f, 1f, 0f, 0.5f)); //draw line in y axis
        drawAxis(zLine, Vector3.forward, new Color(0f, 0f, 1f, 0.5f));//draw line in z axis
    }

    void OnEnable(){
        draw();
    }

}
