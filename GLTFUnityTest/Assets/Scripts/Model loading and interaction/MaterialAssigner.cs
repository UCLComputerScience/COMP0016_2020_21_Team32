using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>Helper class that handles the assignment and change of state of materials to a loaded model at runtime</summary>
public static class MaterialAssigner
{
    public static Stack<float> opacities = new Stack<float>();

    #region public methods

    /*Assigns a certain material to all segments in a list below index*/
    public static void assignMaterialToAllChildrenBelowIndex(GameObject plane, List<GameObject> segments, Shader shader, int index =0,float opacity = 1.0f){
        for(int i = segments.Count - 1 - index; i !=-1; i--){
            Debug.Log(i +" "+ segments[i].name);
            if(segments[i].GetComponent<Renderer>() != null)
            {
                Debug.Log(segments[i].name);
                assignNewMaterial(plane, segments[i], segments.Count - i, shader);

            }
        }
    }

    /*Adjusts the opacity of the desired segment from a list of segments. The renderer of the segment is disabled if it goes below minOpacity to optimise performance*/
    public static float adjustOpacity(float newOpacity, List<GameObject> segments, int segmentIndex, float minOpacity) {
        Color color = segments[segmentIndex].GetComponent<Renderer>().material.color;
        color.a = newOpacity;
        if(color.a < minOpacity)segments[segmentIndex].GetComponent<Renderer>().enabled = false;
        else{
            if(segments[segmentIndex].GetComponent<Renderer>().enabled == false)segments[segmentIndex].GetComponent<MeshRenderer>().enabled = true;
            segments[segmentIndex].GetComponent<Renderer>().material.SetColor("_Color", color);
        }
        return newOpacity;
    }

    /*Reduces the opacities of all segments to a minimum of targetOpacity.*/
    public static void reduceOpacityAll(float targetOpacity, List<GameObject> segments){
        foreach(GameObject g in segments){
            Renderer r = g.GetComponent<Renderer>();
            opacities.Push(r.material.color.a);
            float op = (r.material.color.a < targetOpacity) ? r.material.color.a : targetOpacity;
            r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, op);
        }
    }
    /*Resets the opacities of the segments - has no effect if not called after the above method*/
    public static void resetOpacities(List<GameObject> segments){
        if(opacities.Count > 0){
            foreach(GameObject g in ModelHandler.organ.segments){
                Renderer r = g.GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, opacities.Pop());
            }
        }
    }
    #endregion
    /*changes the colour of a single segment/gameobject*/
    public static void changeColour(GameObject segment, Color colour){
        segment.GetComponent<Renderer>().material.SetColor("_Color", colour);
    }

    /*Converts a hexadecimal string to a colour*/
    public static Color hexToColour(string hex){
        float r = hexToDec(hex.Substring(0, 2))/255f;
        float g = hexToDec(hex.Substring(2,2))/255f;
        float b = hexToDec(hex.Substring(4,2))/255f;
        return new Color(r,g,b);
    }



    #region private methods
    /*
    Models have a material assigned to them as soon as they are loaded in by the GLTFUtility.Importer. The new material we assign uses the same textures and colours, but
    a new shader is passed as a parameter. The renderqueue is also set based on the child's position in the list to deal with draw order issues that arise when overlaying
    multiple transparent objects.
    */
    private static void assignNewMaterial(GameObject plane, GameObject child, int i, Shader shader, float opacity=1.0f){
        Color col = child.GetComponent<Renderer>().material.GetColor("_Color");
        Texture t = child.GetComponent<Renderer>().material.GetTexture("_MainTex");
        col.a = opacity;
        Material mat = new Material(shader);
        mat.SetColor("_Color", col);
        mat.SetTexture("_MainTex", t);
        mat.SetVector("_PlanePosition", plane.transform.position); 
        mat.SetVector("_PlaneNormal", plane.transform.up);
        mat.renderQueue = 3000 + i*20;
        child.GetComponent<Renderer>().material = mat;
    }
    
    /*converts a hexadecimal string to a 32 bit integer*/
    private static int hexToDec(string hex){
        return System.Convert.ToInt32(hex, 16);
    }
    #endregion

}
    
