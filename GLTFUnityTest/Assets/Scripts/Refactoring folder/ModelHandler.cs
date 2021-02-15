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
    public static Organ organ; 
    [SerializeField] GameObject plane; // could make plane singleton
    
    [SerializeField] Shader clippingPlaneShader;
    public static List<GameObject> segments = new List<GameObject>();
    private float segOpacity = 1.0f;
    private float minOpacity = 0.3f;
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
        ColourSelect.current.onColourSelect += Pallete_onColourSelect;
        Debug.Log(organ);
    }

    /*Called whenever the opacity slider is moved. Changes the opacity of the currently selected segment*/
    public void AdjustOpacity(float newOp) {
        if(segments[currentlySelected] != null){
            Debug.Log(newOp);
            segOpacity = newOp;
            Color color = segments[currentlySelected].GetComponent<Renderer>().material.color;
            color.a = segOpacity;
            if(color.a < minOpacity)segments[currentlySelected].GetComponent<Renderer>().enabled = false;
            else{
                if(segments[currentlySelected].GetComponent<Renderer>().enabled == false)segments[currentlySelected].GetComponent<MeshRenderer>().enabled = true;
                segments[currentlySelected].GetComponent<Renderer>().material.SetColor("_Color", color);
            }
        }
    }
    public void selectSegment(){
        if(currentlySelected == segments.Count-1)currentlySelected = 0;
        else currentlySelected++;
        // onSegmentSelectEventArgs e = new onSegmentSelectEventArgs(segments[currentlySelected]);
        // onSegmentSelect?.Invoke(this, e);
    }

    /*When the user clicks the pallete, an event is fired that holds the data of the selected colour. 
     Here the currently selected mesh is set to that colour.
    */
   public void Pallete_onColourSelect(object sender, EventArgsColourData e){
        Color col = e.col;
        col.a = segOpacity;
        segments[currentlySelected].GetComponent<Renderer>().material.SetColor("_Color", col);
    }

    /*
    Loads the appropriate model and sets it as a child to this gameObject.
    Done using a coroutine to prevent nullReferenceException errors being thrown 
    (ie, so that the initialiseModel() is not called on a model that has not yet loaded in) ->
    control is returned to the caller until the model is loaded in.
    */
    private IEnumerator loadModel(){
        organ = OrganFactory.GetOrgan(fileName);
        organ.parent = this.gameObject;
        yield return new WaitUntil(() => (organ.model != null)); 
        organ.initialiseModel();
        segments = organ.segments;
        //CameraMovement.target.position = segments[0].GetComponent<Renderer>().bounds.center;
        // foreach(GameObject g in segments
        // {
        //     g.AddComponent<MeshCollider>();
        //     MeshCollider m = g.GetComponent<MeshCollider>();
        //     m.cookingOptions =
        // }
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, segments, clippingPlaneShader);
    }

    // private void assignMaterialToAllChildrenBelowIndex(int index, Shader shader){
    //     for(int i = segments.Count - 1 -index; i !=-1; i--){
    //         Debug.Log(i +" "+ segments[i].name);
    //         if(segments[i].GetComponent<Renderer>() != null)
    //         {
    //             Debug.Log(segments[i].name);
    //             assignNewMaterial(plane,segments[i], segments.Count - i, shader);

    //         }
    //     }
    // }

    /*
    Materials are created and assigned to the models at runtime.
    Setting the rendering queue is important to ensure that the organ segments are drawn in the right order (higher render queue values are drawn later)
    */
    // private void assignNewMaterial(GameObject child, int i, Shader shader){
    //     Color col = child.GetComponent<MeshRenderer>().material.GetColor("_Color");
    //     col.a = 1.0f;
    //     Material mat = new Material(shader);
    //     mat.SetColor("_Color", col);
    //     mat.SetVector("_PlanePosition", plane.transform.position); 
    //     mat.SetVector("_PlaneNormal", plane.transform.up);
    //     mat.renderQueue = 3000 + i*20;
    //     child.GetComponent<MeshRenderer>().material = mat;
    // }

}
