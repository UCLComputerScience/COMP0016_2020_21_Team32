using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


///<summary>Class that handles the assignment and change of state of materials to a loaded model at runtime</summary>
public static class MaterialAssigner
{
    public static Stack<float> opacities = new Stack<float>();

    #region public methods

    /*Assigns a certain material to all segments*/
    public static void assignToAllChildren(GameObject plane, List<GameObject> segments, Shader shader){
        for(int i = 0; i < segments.Count; i++){
            if(segments[i].GetComponent<Renderer>() != null){
                assignNewMaterial(plane, segments[i], i, shader);
            }
        }
    }

    /*Adjusts the opacity of a gameobject. The renderer of the gameobject is disabled if it goes below minOpacity to optimise performance*/
    public static float adjustOpacity(float newOpacity, GameObject segment, float minOpacity) {
        Color color = segment.GetComponent<Renderer>().material.color;
        color.a = newOpacity;
        if(color.a < minOpacity){
            segment.GetComponent<Renderer>().enabled = false;
        }else{
            if(segment.GetComponent<Renderer>().enabled == false){
                segment.GetComponent<MeshRenderer>().enabled = true;
            }
            segment.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
        return newOpacity;
    }
    
    public static void updatePlanePos(GameObject g, GameObject plane){
        g.GetComponent<Renderer>().material.SetVector("_PlanePosition", plane.transform.position);
        g.GetComponent<Renderer>().material.SetVector("_PlaneNormal", plane.transform.up);
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
            foreach(GameObject g in segments){
                Renderer r = g.GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, opacities.Pop());
            }
        }
    }
    #endregion
    /*changes the colour of a single segment/gameobject */
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
    private static void assignNewMaterial(GameObject plane, GameObject child, int index, Shader shader, float opacity=1.0f){
        Color col = child.GetComponent<Renderer>().material.GetColor("_Color");
        Texture t = child.GetComponent<Renderer>().material.GetTexture("_MainTex");
        col.a = opacity;
        Material mat = new Material(shader);
        mat.SetColor("_Color", col);
        mat.SetTexture("_MainTex", t);
        mat.SetVector("_PlanePosition", plane.transform.position); 
        mat.SetVector("_PlaneNormal", plane.transform.up);
        child.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        child.GetComponent<MeshFilter>().mesh.RecalculateBounds();
        child.GetComponent<MeshFilter>().mesh.RecalculateTangents();
        mat.renderQueue = (int)RenderQueue.Transparent - index; //set the renderqueue of the material applied based on the index of the segment
        child.GetComponent<Renderer>().material = mat;
    }
    
    /*converts a hexadecimal string to a 32 bit integer*/
    private static int hexToDec(string hex){
        return System.Convert.ToInt32(hex, 16);
    }
    #endregion

}
    
