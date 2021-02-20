using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ModelHandler : MonoBehaviour // This class should be a singleton
{
    /*
    Class handles interaction between the UI and the model.
    */
    public static string fileName = "brain.glb"; 
    public static string annotationFolder;
    public static ModelHandler current;
    public static float modelRadius; 
    public static Vector3 modelCentre;
    public static Organ organ; 
    [SerializeField] GameObject plane; // could make plane singleton
    
    [SerializeField] Shader clippingPlaneShader;
    public static List<GameObject> segments = new List<GameObject>();
    private float segOpacity = 1.0f;
    private float minOpacity = 0.3f;
    [SerializeField] Slider opacitySlider;
    private int currentlySelected = 0;
    void Awake(){
        current = this;
        this.opacitySlider.onValueChanged.AddListener(AdjustOpacity);
        StartCoroutine(loadModel());
        EventManager.current.OnColourSelect += eventsManager_onColourSelect;
        annotationFolder = getAnnotationFolderName(fileName);
    }
    void start(){
    }
    /*Called whenever the opacity slider is moved. Changes the opacity of the currently selected segment*/
    public void AdjustOpacity(float newOp) {
        if(segments[currentlySelected] != null){
            segOpacity = MaterialAssigner.adjustOpacity(newOp, segments, currentlySelected, minOpacity);
        }
    }
    public void selectSegment(){
        if(currentlySelected == segments.Count-1)currentlySelected = 0;
        else currentlySelected++;
    }

    /*When the user clicks the pallete, an event is fired that holds the data of the selected colour. 
     Here the currently selected mesh is set to that colour.
    */
   public void eventsManager_onColourSelect(object sender, EventArgsColourData e){
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
        yield return new WaitUntil(() => (organ.model != null));
        organ.parent = this.gameObject; 
        organ.initialiseModel();
        segments = organ.segments;
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, segments, clippingPlaneShader);
        Bounds modelBounds = getModelBounds();
        modelRadius = modelBounds.extents.magnitude;
        modelCentre = modelBounds.center;
    }

    /*Generates a readable foldername (not containing any path separators) to store the annotations of the model being viewed*/    
    private string getAnnotationFolderName(string filename){
        int index = filename.LastIndexOf(Path.DirectorySeparatorChar);
        return (index != -1) ? fileName.Substring(index) : filename;
    }
    private Bounds getModelBounds(){
        Bounds combinedBounds = new Bounds();
        Renderer[] renderers = organ.model.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers)combinedBounds.Encapsulate(r.bounds);
        return combinedBounds;
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
