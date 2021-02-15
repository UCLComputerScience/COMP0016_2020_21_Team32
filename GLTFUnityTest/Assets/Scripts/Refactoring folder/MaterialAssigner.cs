using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialAssigner
{
    private static void assignNewMaterial(GameObject plane, GameObject child, int i, Shader shader, float opacity=1.0f){
        Color col = child.GetComponent<Renderer>().material.GetColor("_Color");
        col.a = opacity;
        Material mat = new Material(shader);
        mat.SetColor("_Color", col);
        mat.SetVector("_PlanePosition", plane.transform.position); 
        mat.SetVector("_PlaneNormal", plane.transform.up);
        mat.renderQueue = 3000 + i*20;
        child.GetComponent<Renderer>().material = mat;
    }

    /*
    Materials are created and assigned to the models at runtime.
    Setting the rendering queue is important to ensure that the organ segments are drawn in the right order (higher render queue values are drawn later)
    */
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
    
}
