using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelHandler : MonoBehaviour
{
    /*
    Class handles interaction between the UI and the model.
    */
    public static string fileName = "brain.glb";
    Organ organ; 
    [SerializeField] GameObject plane; // could make plane singleton
    
    [SerializeField] Shader clippingPlaneShader;
    List<GameObject> segments = new List<GameObject>();
    private float segOpacity = 1.0f;
    private float minOpacity;
    [SerializeField] Slider opacitySlider;
    private int currentlySelected = 0;


/*
Coroutine ensures that the model has been read from the file and converted into a GameObject 
before initialising its child segments
*/

    
    void Start()
    {
        this.opacitySlider.onValueChanged.AddListener(AdjustOpacity);
        StartCoroutine(loadModel());

    }
    IEnumerator loadModel(){
        organ = OrganFactory.GetOrgan(fileName);
        organ.parent = this.gameObject;
        yield return new WaitUntil(() => (organ.model != null)); 
        organ.initialiseModel();
        segments = organ.segments;
        assignMaterialToAllChildrenBelowIndex(0, clippingPlaneShader);
    }
    private void assignMaterialToAllChildrenBelowIndex(int index, Shader shader){
        for(int i = segments.Count - 1 -index; i !=-1; i--){
            Debug.Log(i +" "+ segments[i].name);
            if(segments[i].GetComponent<Renderer>() != null)
            {
                Debug.Log(segments[i].name);
                assignNewMaterial(segments[i], segments.Count - i, shader);

            }
        }
    }

    void assignNewMaterial(GameObject child, int i, Shader shader){
        Color col = child.GetComponent<MeshRenderer>().material.GetColor("_Color");
        col.a = 1.0f;
        Material mat = new Material(shader);
        mat.SetColor("_Color", col);
        mat.SetVector("_PlanePosition", plane.transform.position); //Just sets the plane WAY away from the model (for the time being)
        mat.SetVector("_PlaneNormal", plane.transform.up);
        mat.renderQueue = 3000 + i*20;
        child.GetComponent<MeshRenderer>().material = mat;
    }

    public void AdjustOpacity(float newOp) {
        if(segments[currentlySelected] != null){
            Debug.Log(newOp);
            segOpacity = newOp;
            Color color = segments[currentlySelected].GetComponent<MeshRenderer>().material.color;
            color.a = segOpacity;
            if(color.a < minOpacity)segments[currentlySelected].GetComponent<MeshRenderer>().enabled = false;
            else{
                if(segments[currentlySelected].GetComponent<MeshRenderer>().enabled == false)segments[currentlySelected].GetComponent<MeshRenderer>().enabled = true;
                segments[currentlySelected].GetComponent<MeshRenderer>().material.SetColor("_Color", color);
            }
        }
    }

    public void selectSegment(){
        if(currentlySelected == segments.Count-1)currentlySelected = 0;
        else currentlySelected++;
        // onSegmentSelectEventArgs e = new onSegmentSelectEventArgs(segments[currentlySelected]);
        // onSegmentSelect?.Invoke(this, e);
    }
}
