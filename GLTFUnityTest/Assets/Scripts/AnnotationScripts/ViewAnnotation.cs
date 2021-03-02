using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using TMPro;

///<summary>
///This class allows the user to view the annotations made on a particular model by parsing all json files in the folder for 
///that model into annotation objects. The titles of these annotations are displayed as the options of the dropdown list. 
///<summary>
public class ViewAnnotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Shader shader;
    [SerializeField] GameObject plane;
    public Image pointOfInterest;
    [SerializeField] TMP_Dropdown dropdown;

    [SerializeField] Canvas canvas; 
    
    private CanvasScaler scaler;

    Vector2 screenScale;
    private FileHelper fileHelper;
    [SerializeField] GameObject annotationTextBox;
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
        EventManager.current.OnEnableCamera += otherEvent;
        EventManager.current.OnEnablePivot += otherEvent;
        EventManager.current.OnEnableCrossSection += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
        EventManager.current.OnReset += otherEvent;
        EventManager.current.OnViewAnnotations += EventManager_onAnnotationViewButtonPressed;
        EventManager.current.OnAddAnnotations += otherEvent;
    }

    public void onIndexChanged(int index){
        if(index != 0){
            Debug.Log(index + " " +annotations[index].screenDimensions);
            float scaledXPos = annotations[index].annotationPosition.x * Screen.width/annotations[index].screenDimensions.x;
            float scaledYPos = annotations[index].annotationPosition.y * Screen.height/annotations[index].screenDimensions.y;
            Vector2 scaledPos = new Vector2(scaledXPos, scaledYPos);  
            Debug.Log(scaledPos);

            Camera.main.gameObject.transform.position = annotations[index].cameraCoordinates;
            Camera.main.gameObject.transform.rotation = annotations[index].cameraRotation;
            pointOfInterest.transform.position = scaledPos;
            annotationText.text = annotations[index].text;
            annotationTextBox.gameObject.transform.position = scaledPos;
            plane.transform.position = annotations[index].planePosition;
            plane.transform.up = annotations[index].planeNormal; 

            annotationText.gameObject.SetActive(true);
            annotationTextBox.SetActive(true);
            MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments,shader);
            for(int i = 0; i < ModelHandler.segments.Count(); i++){
                ModelHandler.segments[i].GetComponent<MeshRenderer>().material.color = annotations[index].colours[i];
            }
        }

    }
    private void jsonToAnnotations(){
        DirectoryInfo dir;
        annotations = new List<AnnotationData>();
        annotationTitles = new List<String>();
        dropdown.ClearOptions();
        string dirPath = Path.Combine(Application.dataPath, FileHelper.currentAnnotationFolder);
        dir = Directory.CreateDirectory(dirPath);


        /*Problem with loading in other files here */
        FileInfo[] info = dir.GetFiles("*.json");
        
        foreach (FileInfo f in info){
            if(!f.Exists)f.Create();
            Debug.Log(f.FullName);
            String jsonToParse = File.ReadAllText(f.FullName);
            annotations.Add(JsonUtility.FromJson<AnnotationData>(jsonToParse) as AnnotationData);
        }
        //Debug.Log("Length of annotations: " +annotations.Count);
        Annotation.setNumAnnotations(annotations.Count);
        annotationTitles.Add("--Select Annotation--");
        foreach(AnnotationData annotation in annotations){
            Debug.Log("All the positions stored: "+ annotation.annotationPosition);
            annotationTitles.Add(annotation.title);
        }
        annotations.Insert(0, new AnnotationData());
        dropdown.AddOptions(annotationTitles);
        //Debug.Log("Length of annotationTitles: "+annotationTitles.Count);
    }


    public void EventManager_onAnnotationViewButtonPressed(object sender, EventArgs e){
        dropdown.gameObject.SetActive(true);
        jsonToAnnotations();
    }
    public void otherEvent(object sender, EventArgs e){
        annotationText.gameObject.SetActive(false);
        annotationTextBox.gameObject.SetActive(false);
        isEnabled = false;
    }
}
