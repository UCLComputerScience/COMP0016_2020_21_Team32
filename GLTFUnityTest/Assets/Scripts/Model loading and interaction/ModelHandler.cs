using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

///<summary>This class is uses the OrganFactory to load in the model and helps to otherwise initialise the model and scene based on the model that is loaded in. 
///The radius of the model is calculated here, and the segments of the model are loaded into a list. The list and radius are kept public and static as they are
///used by many other classes.</summary>
public class ModelHandler : MonoBehaviour // This class should be a singleton
{
    public static ModelHandler current;
    public static float modelRadius; 
    public static Vector3 modelCentre;
    public static Organ organ; 
    public GameObject plane; // could make plane singleton 
    public Shader crossSectionalShader;
    public static List<GameObject> segments;
    private float segOpacity;
    private float minOpacity;
    [SerializeField] Slider opacitySlider;
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
        plane = GameObject.Find("Plane");
        opacitySlider = GameObject.Find("Opacity Slider").GetComponent<Slider>();

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
