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
public class AnnotationSelector : MonoBehaviour
{
    public GameObject plane; 
    public TMP_Dropdown dropdown;
    public GameObject annotationTextBox;
    private Shader shader;
    
    private FileHelper fileHelper;
    private TMP_Text annotationText;
    private List<AnnotationData> annotations;
    private List<string> annotationTitles;
    void Awake(){
        /*initialise variables*/
        shader = Shader.Find("Custom/Clipping");
        annotationText = annotationTextBox.GetComponentInChildren<TMP_Text>(true);
        annotations = new List<AnnotationData>();
        annotationTitles = new List<string>();
        /*Pass callback to the onValueChanged event of the dropdown menu*/
        dropdown.onValueChanged.AddListener(onIndexChanged);
    }
    void Start() 
    {
        subscribeToEvents();
        jsonToAnnotations();
    }

    /*This method is called whenever an option is selected in the dropdown menu. The options all correspond to a specific AnnotationData object. Selecting
    an option will load the state of the scene that was stored in the corresponding AnnotationData object.*/
    public void onIndexChanged(int index){
        if(index != 0){
            //ensures the position of the textbox is consistent regardless of the screen size
            float scaledXPos = annotations[index].annotationPosition.x * Screen.width/annotations[index].screenDimensions.x;
            float scaledYPos = annotations[index].annotationPosition.y * Screen.height/annotations[index].screenDimensions.y;
            Vector2 scaledPos = new Vector2(scaledXPos, scaledYPos);   
            
            //load the camera coordinates in which the annotation was made
            Camera.main.gameObject.transform.position = annotations[index].cameraCoordinates;
            Camera.main.gameObject.transform.rotation = annotations[index].cameraRotation;
            CameraController.displacement = annotations[index].cameraDisplacement;

            //set the text contained within the annotation and display in a transparent textbox
            annotationText.text = annotations[index].text;
            annotationTextBox.gameObject.transform.position = scaledPos;
            annotationText.gameObject.SetActive(true);
            annotationTextBox.SetActive(true);

            //set the position of the plane and its normal (so that cross sections of the model can be saved)
            plane.transform.position = annotations[index].planePosition;
            plane.transform.up = annotations[index].planeNormal; 

            //reassign the material (with the updated plane's position and colours stored in the annotation) to the model
            MaterialAssigner.assignMaterialToAllChildrenBelowIndex(plane, ModelHandler.segments,shader);
            for(int i = 0; i < ModelHandler.segments.Count(); i++){
                MaterialAssigner.changeColour(ModelHandler.segments[i], annotations[index].colours[i]);
            }
        }

    }

    /*Searches the Application.datapath (in builds, this is the HoloRepositoryPortable_Data folder, in the editor this is the assets folder)
    for a folder with the same name as the currently loaded model (which is created as soon as a new model is loaded into the application).
    Any json files found are parsed (using the JsonUtility class) back into AnnotationData objects and stored in a list.
     */
    private void jsonToAnnotations(){
        DirectoryInfo dir;
        annotations = new List<AnnotationData>();
        annotationTitles = new List<String>();
        dropdown.ClearOptions(); //The dropdown's options are cleared first with every call to this function to prevent duplicate entries of the same file appearing.
        string dirPath = Path.Combine(Application.dataPath, FileHelper.currentAnnotationFolder);
        dir = Directory.CreateDirectory(dirPath);


        FileInfo[] info = dir.GetFiles("*.json");
        
        foreach (FileInfo f in info){
            if(!f.Exists)f.Create();
            Debug.Log(f.FullName);
            String jsonToParse = File.ReadAllText(f.FullName); //read all the text in the json file into a string
            annotations.Add(JsonUtility.FromJson<AnnotationData>(jsonToParse) as AnnotationData); //parse the string into an AnnotationData object and store it in a list
        }
        Annotation.setNumAnnotations(annotations.Count); //sets the static variable that keeps track of the number of annotations made to the number of annotation in the array
        annotationTitles.Add("--Select Annotation--");
        foreach(AnnotationData annotation in annotations){
            annotationTitles.Add(annotation.title);
        }
        annotations.Insert(0, new AnnotationData());
        dropdown.AddOptions(annotationTitles);
    }

    /*Subscribe to the relevant events. Ensures that the DropDown menu does not remain visible if another button on the navigation bar is pressed - helps
    to keep the UI less cluttered when using the other features*/
    private void subscribeToEvents(){
        EventManager.current.OnEnableCamera += otherEvent;
        EventManager.current.OnEnablePivot += otherEvent;
        EventManager.current.OnEnableCrossSection += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
        EventManager.current.OnReset += otherEvent;
        EventManager.current.OnViewAnnotations += EventManager_onViewAnnotations;
        EventManager.current.OnAddAnnotations += otherEvent;
    }

    /*When an onViewAnnotations event is received the dropdown menu is set to active and its 
    options populated*/
    public void EventManager_onViewAnnotations(object sender, EventArgs e){
        dropdown.gameObject.SetActive(true);
        jsonToAnnotations();
    }
    /*Whenever any of the other events triggered by the buttons on the navigation bar are received, the annotation textbox is made inactive*/
    public void otherEvent(object sender, EventArgs e){
        annotationTextBox.gameObject.SetActive(false);
        dropdown.gameObject.SetActive(false);
    }
}
