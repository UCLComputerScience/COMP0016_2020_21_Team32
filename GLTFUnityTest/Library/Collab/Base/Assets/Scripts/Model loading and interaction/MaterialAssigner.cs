using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///<summary>Helper class that handles the assignment and change of state of materials to a loaded model at runtime</summary>
public static class MaterialAssigner
{
    public static Stack<float> opacities = new Stack<float>();
    /*
    Materials are created and assigned to the models at runtime.
    Setting the rendering queue is important to ensure that the organ segments are drawn in the right order (higher render queue values are drawn later)
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
    public static void assignMaterialToAllChildrenBelowIndex(GameObject plane, List<GameObject> segments, Shader shader, int index =0,float opacity = 1.0f){
        for(int i = segments.Count - 1 -index; i !=-1; i--){
            Debug.Log(i +" "+ segments[i].name);
            if(segments[i].GetComponent<Renderer>() != null)
            {
                Debug.Log(segments[i].name);
                assignNewMaterial(plane, segments[i], segments.Count - i, shader);

            }
        }
    }
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
    public static void reduceOpacityAll(float targetOpacity, List<GameObject> segments){
        foreach(GameObject g in segments){
            Renderer r = g.GetComponent<Renderer>();
            opacities.Push(r.material.color.a);
            float op = (r.material.color.a < targetOpacity) ? r.material.color.a : targetOpacity;
            r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, op);
        }
    }
    public static void resetOpacities(List<GameObject> segments){
        if(opacities.Count > 0){
            foreach(GameObject g in ModelHandler.organ.segments){
                Renderer r = g.GetComponent<Renderer>();
                r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, opacities.Pop());
            }
        }
    }


    public static void changeColour(GameObject segment, Color colour){
        segment.GetComponent<Renderer>().material.SetColor("_Color", colour);
    }
}
    
