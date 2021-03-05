using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
///<summary>Used to draw the lines of the 3D coordinate axis whenever the pivot controller is enabled</summary>
public class GenerateLines : MonoBehaviour
{
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
=======
public class GenerateLines : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] LineRenderer xLine;
    [SerializeField] LineRenderer yLine;
    [SerializeField] LineRenderer zLine;

    void drawAxis(LineRenderer line, Vector3 axisDir, Color color){
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
        line.material.SetColor("_Color", color); 
        line.useWorldSpace = true;
        line.sortingOrder = 5;
        line.startWidth = line.endWidth = 0.2f;
        line.startColor = line.endColor = color;
<<<<<<< HEAD
        line.SetPositions(new Vector3[]{-axisDir*5000, axisDir*5000}); //arbitrarily large length of line in the direction specified 
    }
    /*Draws 3 lines, 1 for each axis in 3D space*/
    public void draw(){
        drawAxis(xLine, Vector3.right, new Color(1f, 0f, 0f, 0.5f)); //draw line in x axis
        drawAxis(yLine, Vector3.up, new Color(0f, 1f, 0f, 0.5f)); //draw line in y axis
        drawAxis(zLine, Vector3.forward, new Color(0f, 0f, 1f, 0.5f));//draw line in z axis
=======
        line.SetPositions(new Vector3[]{-axisDir*5000, axisDir*5000});
    }

    public void draw(){
        drawAxis(xLine, Vector3.right, new Color(1f, 0f, 0f, 0.5f));
        drawAxis(yLine, Vector3.up, new Color(0f, 1f, 0f, 0.5f));
        drawAxis(zLine, Vector3.forward, new Color(0f, 0f, 1f, 0.5f));
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    }

    void OnEnable(){
        draw();
    }
<<<<<<< HEAD

=======
    void OnDisable(){
        xLine.startColor = xLine.endColor = new Color(0f,0f,0f,0f);
        yLine.startColor = yLine.endColor = new Color(0f,0f,0f,0f);
        zLine.startColor = zLine.endColor = new Color(0f,0f,0f,0f);
    }
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
}
