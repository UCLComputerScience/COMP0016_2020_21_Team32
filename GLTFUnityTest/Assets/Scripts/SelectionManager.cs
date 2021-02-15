// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;

// public class SelectionManager : MonoBehaviour
// {
//     [SerializeField] Toggle tButton;
//     [SerializeField] Toggle rButton;
//     [SerializeField] Toggle reButton;
//     [SerializeField] Toggle annotationButton;
//     [SerializeField] Toggle vAnnotationButton;
    
//     private CanvasRenderer selectedButton;
//     List<CanvasRenderer> selectables;
//     List<RectTransform> selectableTransforms;
//     Color defaultCol;
//     Color selectedCol;
//     GraphicRaycaster ray;
//     PointerEventData pointerEventData;
//     EventSystem eventSystem;

//     void start(){
//         selectables = new List<CanvasRenderer>();
//         selectableTransforms = new List<RectTransform>();
//         selectables.Add(tButton.GetComponent<CanvasRenderer>());
//         selectables.Add(rButton.GetComponent<CanvasRenderer>());
//         selectables.Add(reButton.GetComponent<CanvasRenderer>());
//         selectables.Add(annotationButton.GetComponent<CanvasRenderer>());
//         selectables.Add(vAnnotationButton.GetComponent<CanvasRenderer>());
//         selectableTransforms.Add(tButton.GetComponent<RectTransform>());
//         selectableTransforms.Add(rButton.GetComponent<RectTransform>());
//         selectableTransforms.Add(reButton.GetComponent<RectTransform>());
//         selectableTransforms.Add(annotationButton.GetComponent<RectTransform>());
//         selectableTransforms.Add(vAnnotationButton.GetComponent<RectTransform>());
//         defaultCol = selectables[0].GetColor();
//         selectedCol = new Color(255, 0, 0);
//         ray = GetComponent<GraphicRaycaster>();
//         eventSystem = GetComponent<EventSystem>();
//     }
//     void Update()
//     {

//         //if(Input.GetKey(KeyCode.Mouse0)){
//             pointerEventData = new PointerEventData(eventSystem);
//             pointerEventData.position = Input.mousePosition;

//             List<RaycastResult> results = new List<RaycastResult>();
//             //CanvasRenderer selectionRenderer;

//             ray.Raycast(pointerEventData, results);

//             foreach (RaycastResult result in results)
//             {
//                     Debug.Log("Hit " + result.gameObject.name);
//             }
//         //}

//         // ray.Raycast(pointerEventData, results);
//         // if(selectedButton !=null){
//         //     selectionRenderer = selectedButton;
//         //     selectedButton.SetColor(selectedCol);
//         // }

//         // RaycastHit hit;
//         // if(ray.Raycast(ray, out hit)){
//         //     RectTransform selection = (RectTransform) hit.transform;
//         //     foreach(RectTransform t in selectableTransforms){
//         //         if(selection.Equals(t))Debug.Log("Hey there");
//         //     }

//         // }
        
//     }
// }
//Attach this script to your Canvas GameObject.
//Also attach a GraphicsRaycaster component to your canvas by clicking the Add Component button in the Inspector window.
//Also make sure you have an EventSystem in your hierarchy.
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager current; //singleton pattern.
    void Awake(){
        current = this;
    }
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;

    public event EventHandler onCameraButtonPressed;
    public event EventHandler onTButtonPressed;
    public event EventHandler onRButtonPressed;
    public event EventHandler onReButtonPressed;
    public event EventHandler onVAnnotationButtonPressed;
    public event EventHandler onAnnotationButton;
    public event EventHandler onColourSelect;

    public event EventHandler onChangePivot;

    [SerializeField] GameObject UIBlocker;
    
    [SerializeField] Toggle cameraButton;
    [SerializeField] Toggle tButton;
    [SerializeField] Toggle rButton;
    [SerializeField] Toggle reButton;
    [SerializeField] Toggle vAnnotationButton;
    [SerializeField] Toggle annotationButton;

    [SerializeField] Button segmentSelector;
    
    [SerializeField] GameObject pallete; 
    [SerializeField] GameObject pivotChangeButton;
    private Toggle selectedToggle;
    private List<Toggle> toggles;
    private List<EventHandler> events;


    void Start()
    {
        cameraButton.Select();

        toggles = new List<Toggle>();
        events = new List<EventHandler>();

        toggles.Add(cameraButton);
        toggles.Add(tButton);
        toggles.Add(rButton);
        toggles.Add(reButton);
        toggles.Add(vAnnotationButton);
        toggles.Add(annotationButton);

        events.Add(onCameraButtonPressed);
        events.Add(onTButtonPressed);
        events.Add(onRButtonPressed);
        events.Add(onReButtonPressed);
        events.Add(onVAnnotationButtonPressed);
        events.Add(onAnnotationButton);
        

        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();

        events[0]?.Invoke(this, EventArgs.Empty); //have first button selected immediately
        selectedToggle = toggles[0];

    }

    void Update()
    {
        if(UIBlocker.activeInHierarchy){
            Debug.Log(UIBlocker.activeInHierarchy);
            return;
        }
        foreach(Toggle toggle in toggles){
            if(toggle.Equals(selectedToggle))toggle.isOn = true;
            else{
                toggle.isOn = false;
            }
        }
        // if(selectedToggle != null){
        //     selectedToggle.Select();
        //     //selectedToggle.isOn = true;
        // }
        //Check if the left Mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            pointerEventData = new PointerEventData(eventSystem);
            //Set the Pointer Event Position to that of the mouse position
            pointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            raycaster.Raycast(pointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                for(int i = 0; i < toggles.Count; i++){
                    if(result.isValid){
                        if(result.gameObject.Equals(toggles[i].gameObject)){
                            //Debug.Log("I clicked it");
                            selectedToggle = toggles[i];
                            selectedToggle.isOn = true;
                            events[i]?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
                if(result.gameObject.Equals(pivotChangeButton.gameObject) && result.isValid){
                    Debug.Log("On change pivot is fired");
                    onChangePivot?.Invoke(this, EventArgs.Empty);
                }
                // if(result.gameObject.Equals(pallete)){
                //     onColourSelect?.Invoke(this, EventArgs.Empty);
                // }
            }   
        }
    }
}