using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLines : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] LineRenderer xLine;
    [SerializeField] LineRenderer yLine;
    [SerializeField] LineRenderer zLine;

    void drawAxis(LineRenderer line, Vector3 axisDir, Color color){
        line.material.SetColor("_Color", color); 
        line.useWorldSpace = true;
        line.sortingOrder = 5;
        line.startWidth = line.endWidth = 0.2f;
        line.startColor = line.endColor = color;
        line.SetPositions(new Vector3[]{-axisDir*5000, axisDir*5000});
    }

    public void draw(){
        drawAxis(xLine, Vector3.right, new Color(1f, 0f, 0f, 0.5f));
        drawAxis(yLine, Vector3.up, new Color(0f, 1f, 0f, 0.5f));
        drawAxis(zLine, Vector3.forward, new Color(0f, 0f, 1f, 0.5f));
    }

    void OnEnable(){
        draw();
    }
    void OnDisable(){
        xLine.startColor = xLine.endColor = new Color(0f,0f,0f,0f);
        yLine.startColor = yLine.endColor = new Color(0f,0f,0f,0f);
        zLine.startColor = zLine.endColor = new Color(0f,0f,0f,0f);
    }
}
