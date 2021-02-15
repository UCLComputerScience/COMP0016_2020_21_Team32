using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class ViewAnnotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Shader shader;
    [SerializeField] GameObject plane;
    public Image pointOfInterest;

    public GameObject POI;
    [SerializeField] TMP_Dropdown dropdown;

    [SerializeField] Canvas canvas; 
    
    private CanvasScaler scaler;

    Vector2 screenScale;

    //[SerializeField] GameObject poi;
    
    [SerializeField] Image annotationTextBox;
    [SerializeField] TMP_Text annotationText;
    private bool isEnabled = false;
    public List<AnnotationData> annotations = new List<AnnotationData>();
    public List<string> annotationTitles = new List<string>();
    void Awake(){
        scaler = canvas.GetComponent<CanvasScaler>();
        screenScale = new Vector2(scaler.referenceResolution.x/Screen.width, scaler.referenceResolution.y/Screen.height);
    }
    void Start()
    {
        subscribeToEvents();
        jsonToAnnotations();
    }
    private void subscribeToEvents(){
        SelectionManager.current.onCameraButtonPressed += otherEvent;
        SelectionManager.current.onTButtonPressed += otherEvent;
        SelectionManager.current.onRButtonPressed += otherEvent;
        SelectionManager.current.onReButtonPressed += otherEvent;
        SelectionManager.current.onVAnnotationButtonPressed += SelectionManager_onAnnotationViewButtonPressed;
        SelectionManager.current.onAnnotationButton += otherEvent;
    }

    public void onIndexChanged(int index){
        if(index != 0){ 
            // annotationTextBox.gameObject.SetActive(false);
            // pointOfInterest.SetActive(false);
            // pointOfInterest.SetActive(false);
            // annotationTextBox.gameObject.SetActive(false);
            Camera.main.gameObject.transform.position = annotations[index].cameraCoordinates;
            Camera.main.gameObject.transform.rotation = annotations[index].cameraRotation;
            //var scaledPos = new Vector2(annotations[index].annotationPosition.x * screenScale.x, annotations[index].annotationPosition.y * screenScale.y);
            annotationTextBox.rectTransform.position= annotations[index].annotationPosition + new Vector3(100f,0f,0f);
            pointOfInterest.transform.position = annotations[index].annotationPosition;
            //poi.transform.position = annotations[index].annotationPosition;
            //new Vector2(annotations[index].annotationPosition.x * screenScale.x, annotations[index].annotationPosition.y * screenScale.y);//annotations[index].annotationPosition;
            Debug.Log(pointOfInterest.rectTransform.position);
            pointOfInterest.gameObject.SetActive(true);
            annotationText.text = annotations[index].text;
            annotationText.gameObject.SetActive(true);
            annotationTextBox.gameObject.SetActive(true);
            plane.transform.position = annotations[index].planePosition;
            plane.transform.up = annotations[index].planeNormal;
            MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments,shader);
            for(int i = 0; i < ModelHandler.segments.Count(); i++){
                ModelHandler.segments[i].GetComponent<MeshRenderer>().material.color = annotations[index].colours[i];
            }


            //POI.transform.position = Camera.main.ScreenToWorldPoint(annotations[index].annotationPosition);
            //POI.transform.SetParent(ModelHandler.segments[0].transform);
        }

    }
    private void jsonToAnnotations(){
        annotations = new List<AnnotationData>();
        annotationTitles = new List<String>();
        dropdown.ClearOptions();
        String path = Application.persistentDataPath;
        DirectoryInfo dir = new DirectoryInfo(path);

        /*Problem with loading in other files here */
        FileInfo[] info = dir.GetFiles("*" + "-" + ModelHandler.fileName + "*.json");
        
        foreach (FileInfo f in info){
            //if(f.Exists)f.Delete();
            if(!f.Exists)f.Create();
            // Debug.Log(f.FullName);
            String jsonToParse = File.ReadAllText(f.FullName);
            // // Debug.Log(f.FullName);
            // // Debug.Log(jsonToParse);
            annotations.Add(JsonUtility.FromJson<AnnotationData>(jsonToParse) as AnnotationData);
        }
        //Debug.Log("Length of annotations: " +annotations.Count);
        Annotation.setNumAnnotations(annotations.Count);
        annotationTitles.Add("--Select Annotation--");
        foreach(AnnotationData annotation in annotations){
            annotationTitles.Add(annotation.title);
        }
        annotations.Insert(0, new AnnotationData());
        dropdown.AddOptions(annotationTitles);
        //Debug.Log("Length of annotationTitles: "+annotationTitles.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectionManager_onAnnotationViewButtonPressed(object sender, EventArgs e){
        dropdown.gameObject.SetActive(true);
        jsonToAnnotations();
    }
    public void otherEvent(object sender, EventArgs e){
        pointOfInterest.gameObject.SetActive(false);
        annotationText.gameObject.SetActive(false);
        annotationTextBox.gameObject.SetActive(false);
        isEnabled = false;
    }
}
