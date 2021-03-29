using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


///<summary>This class uses the OrganFactory to load in the model and helps to otherwise initialise the model. 
///The radius of the model is calculated here, and the segments of the model are loaded into a list.</summary>
public class ModelHandler : MonoBehaviour, IEventManagerListener 
{
    public static ModelHandler current;
    public float modelRadius; 
    public Vector3 modelCentre;
    private Organ organ; 
    public GameObject plane; 
    public List<GameObject> segments;
    public Shader crossSectionalShader;
    private float segOpacity;
    private float minOpacity;
    private int currentlySelected;

    public void subscribeToEvents(){
        EventManager.current.OnColourSelect+=EventManager_onColourSelect;
        EventManager.current.OnChangeOpacity+=EventManager_onAdjustOpacity;
        EventManager.current.OnSegmentSelect+=EventManager_onSelectSegment;
        EventManager.current.OnSelectAnnotation+=EventManager_onSelectAnnotation;
    }
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
        crossSectionalShader = Shader.Find("Custom/Clipping");
        StartCoroutine(loadModel());
    }  
    void Start(){
        subscribeToEvents();
    }
    /*
    Loads the appropriate model and sets it as a child to the gameObject this script is attatched to.
    Done using a coroutine so that the model is fully initialised before the script attempts to access it. 
    (ie, so that the organ.setParent() is not called on a model that has not yet loaded in) ->
    control is returned to the caller until the model is loaded in.
    */
    private IEnumerator loadModel(){
        organ = OrganFactory.GetOrgan(); //Factory pattern - returns the appropriate subclass of Organ based on the model selected by the user
        yield return new WaitUntil(() => organ.model != null);
        EventManager.current.onModelLoaded();
        organ.setParent(this.gameObject);
        segments = organ.segments; //make segments public to other classes
        MaterialAssigner.assignToAllChildren(plane, segments, crossSectionalShader);
        Bounds modelBounds = getModelBounds();
        modelRadius = modelBounds.extents.magnitude;
        modelCentre = modelBounds.center; 
        organ.adjustPosition();
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
            segOpacity = MaterialAssigner.adjustOpacity(e.value, segments[currentlySelected], minOpacity);
        }
    }
    /*When the user clicks the palette, an event is fired that holds the data of the selected colour. 
     Here the currently selected mesh is set to that colour.*/
   private void EventManager_onColourSelect(object sender, EventArgsColourData e){
        Color col = e.col;
        col.a = segOpacity;
        MaterialAssigner.changeColour(segments[currentlySelected], col);
    }


    /*Called whenever the segment select button is clicked.*/
    private void EventManager_onSelectSegment(object sender, EventArgs e){
        if(currentlySelected == segments.Count-1)currentlySelected = 0;
        else currentlySelected++;
    }

    private void EventManager_onSelectAnnotation(object sender, EventArgsAnnotation e){
        //set the position of the plane and its normal (so that cross sections of the model can be saved)
        plane.transform.position = e.data.planePosition;
        plane.transform.up = e.data.planeNormal; 
        //reassign the material (with the updated plane's position and colours stored in the annotation) to the model
        MaterialAssigner.assignToAllChildren(plane, segments, crossSectionalShader);
        for(int i = 0; i < segments.Count; i++){
            MaterialAssigner.changeColour(segments[i], e.data.colours[i]);
        }
    }
}
