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
public class AnnotationSelector : MonoBehaviour, IEventManagerListener 
{
    
    public TMP_Dropdown dropdown;
    public GameObject annotationTextBox;
    private TMP_Text annotationText;
    private List<AnnotationData> annotations;
    private List<string> annotationTitles;
    
    /*Subscribe to the relevant events. Ensures that the DropDown menu does not remain visible if another button on the navigation bar is pressed - helps
    to keep the UI less cluttered when using the other features*/
    public void subscribeToEvents(){
        EventManager.current.OnEnableCamera += otherEvent;
        EventManager.current.OnEnablePivot += otherEvent;
        EventManager.current.OnEnableCrossSection += otherEvent;
        EventManager.current.OnEnableDicom += otherEvent;
        EventManager.current.OnReset += otherEvent;
        EventManager.current.OnViewAnnotations += EventManager_onViewAnnotations;
        EventManager.current.OnAddAnnotations += otherEvent;
        EventManager.current.OnChangeSettings+=otherEvent;
    }
    void Awake(){
        /*initialise variables*/
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
    an option will load the view of the model that was stored in the corresponding AnnotationData object.*/
    public void onIndexChanged(int index){
        if(index != 0){

            EventManager.current.onSelectAnnotation(annotations[index]);
            setAnnotationTextBoxPosition(annotations[index]);   
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
            String jsonToParse = File.ReadAllText(f.FullName); //read all the text in the json file into a string
            annotations.Add(JsonUtility.FromJson<AnnotationData>(jsonToParse) as AnnotationData); //parse the string into an AnnotationData object and store it in a list
        }
        Annotation.setNumAnnotations(annotations.Count); //sets the static variable that keeps track of the number of annotations to the number of annotation in the list.
        annotationTitles.Add("--Select Annotation--");
        foreach(AnnotationData annotation in annotations){
            annotationTitles.Add(annotation.title);
        }
        annotations.Insert(0, new AnnotationData());
        dropdown.AddOptions(annotationTitles);
    }
    //Enables the annotation textbox and ensures the position of the textbox is consistent regardless of the screen size
    private void setAnnotationTextBoxPosition(AnnotationData data){
        float scaledXPos = data.annotationPosition.x * Screen.width/data.screenDimensions.x;
        float scaledYPos = data.annotationPosition.y * Screen.height/data.screenDimensions.y;
        Vector2 scaledPos = new Vector2(scaledXPos, scaledYPos);
        annotationText.text = data.text;
        annotationTextBox.gameObject.transform.position = scaledPos;
        annotationText.gameObject.SetActive(true);
        annotationTextBox.SetActive(true);
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
