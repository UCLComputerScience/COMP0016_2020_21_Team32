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
        if(current != null && current != this){
            Destroy(this.gameObject);
        }else{
            current = this;
        }
        this.opacitySlider.onValueChanged.AddListener(AdjustOpacity);
        StartCoroutine(loadModel());
        EventManager.current.OnColourSelect += EventManager_onColourSelect;
    }
    /*
    Loads the appropriate model and sets it as a child to the gameObject this script is attatched to.
    Done using a coroutine to prevent nullReferenceException errors being thrown 
    (ie, so that the initialiseModel() is not called on a model that has not yet loaded in) ->
    control is returned to the caller until the model is loaded in.
    */
    private IEnumerator loadModel(){
        organ = OrganFactory.GetOrgan(FileHelper.currentModelFileName);
        yield return new WaitUntil(() => (organ.model != null));
        organ.initialiseModel(this.gameObject);
        segments = organ.segments;
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, segments, clippingPlaneShader);
        Bounds modelBounds = getModelBounds();
        modelRadius = modelBounds.extents.magnitude;
        modelCentre = modelBounds.center; 
    }
    private Bounds getModelBounds(){
        Bounds combinedBounds = new Bounds();
        Renderer[] renderers = organ.model.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers)combinedBounds.Encapsulate(r.bounds);
        return combinedBounds;
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
   public void EventManager_onColourSelect(object sender, EventArgsColourData e){
        Color col = e.col;
        col.a = segOpacity;
        segments[currentlySelected].GetComponent<Renderer>().material.SetColor("_Color", col);
    }  
}
