using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

///<summary>This class uses the OrganFactory to load in the model and helps to otherwise initialise the model and scene based on the model that is loaded in. 
///The radius of the model is calculated here, and the segments of the model are loaded into a list. The list and radius are kept public and static as they are
///needed by many other classes.</summary>
public class ModelHandler : MonoBehaviour // This class should be a singleton
{
    public static ModelHandler current;
    public static float modelRadius; 
    public static Vector3 modelCentre;
    public static Organ organ; 
    public GameObject plane; 
    public static List<GameObject> segments;
    public Shader crossSectionalShader;
    private float segOpacity;
    private float minOpacity;
    private int currentlySelected;
    void Awake(){
        //singleton pattern
        if(current != null && current != this){
            Destroy(this.gameObject);
        }else{
            current = this;
        }
        segOpacity = 1.0f;
        minOpacity = 0.3f;
        currentlySelected = 0;
        segments = new List<GameObject>();
        crossSectionalShader = Shader.Find("Custom/Clipping");

        //this.opacitySlider.onValueChanged.AddListener(AdjustOpacity);
        StartCoroutine(loadModel());
        
    }
    void Start(){
        subscribeToEvents();
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
        MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, segments, crossSectionalShader);
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
    private void EventManager_onAdjustOpacity(object sender, EventArgsFloat e){
        if(segments[currentlySelected] != null){
            segOpacity = MaterialAssigner.adjustOpacity(e.data, segments, currentlySelected, minOpacity);
        }
    }
    private void EventManager_onSelectSegment(object sender, EventArgs e){
        if(currentlySelected == segments.Count-1)currentlySelected = 0;
        else currentlySelected++;
    }
    /*When the user clicks the pallete, an event is fired that holds the data of the selected colour. 
     Here the currently selected mesh is set to that colour.
    */
   private void EventManager_onColourSelect(object sender, EventArgsColourData e){
        Color col = e.col;
        col.a = segOpacity;
        MaterialAssigner.changeColour(segments[currentlySelected], col);
    }
    private void subscribeToEvents(){
        EventManager.current.OnColourSelect += EventManager_onColourSelect;
        EventManager.current.OnChangeOpacity+=EventManager_onAdjustOpacity;
        EventManager.current.OnSegmentSelect+=EventManager_onSelectSegment;
    }  
}
