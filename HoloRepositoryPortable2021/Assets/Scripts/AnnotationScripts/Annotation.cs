using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
///<summary>This class is attatched to the Annotation prefab. It initialises the text in the input fields, provides
/// interactivity to the confirm/cancel buttons, and uses the user input (along with the other relevant data available in the scene)
/// to instantiate a new AnnotationData object. On confirm, this object is converted to JSON format and  written to a file.</summary>
public class Annotation : MonoBehaviour
{
    public AnnotationData data;
    public GameObject plane;
    public static int numAnnotations = 0;
    [SerializeField] GameObject annotationPin;
    private Button confirmButton;
    private Button cancelButton;
    private TMP_InputField titleInputField;
    private TMP_InputField inputField;

    public static void setNumAnnotations(int value){
        numAnnotations = value;
    }
    void Awake(){
        /*initialise variables*/
        confirmButton = this.transform.Find("Confirm").GetComponent<Button>();
        cancelButton = this.transform.Find("Cancel").GetComponent<Button>();
        titleInputField = this.transform.Find("title input field").GetComponent<TMP_InputField>();
        inputField = this.transform.Find("input field").GetComponent<TMP_InputField>(); 
        titleInputField.text = "Annotation #" + numAnnotations; 
        inputField.text = "enter...";

    }
    void Start(){
        hide(); //initially set inactive
    }
    /*Sets the annotation prefab to active and adds callbacks to the onclick events of the confirm and cancel buttons. Pos is the position in which the annotation
    pin was dropped*/
    public void show(Vector3 pos){
        EventManager.current.onEnableUIBlocker();
        gameObject.SetActive(true);
        data = new AnnotationData();
        confirmButton.onClick.AddListener(() => {
            save(pos);
            annotationPin.SetActive(false);
            numAnnotations++;
            hide();
            confirmButton.onClick.RemoveAllListeners();
        });
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => {
            annotationPin.SetActive(false);
            hide();
        });
    }

    /*Called when the confirm button is pressed. Instantiates a new AnnotationData object, converts it to a JSON string and writes that string to a file.*/
    private void save(Vector3 pos){
        initialiseAnnotation(pos);
        writeAnnotationToJsonFile();
    }    
    private void hide(){
        ToolTip.current.gameObject.SetActive(false);
        gameObject.SetActive(false);
        EventManager.current.onDisableUIBlocker();
        titleInputField.text = "Annotation #" + numAnnotations;
        inputField.text = "enter...";
        EventManager.current.onEnableCamera();
    }
    /*Populate the AnnotationData object created when show() was called*/
    private void initialiseAnnotation(Vector3 pos){
        data.title = titleInputField.text; //the value entered by the user in the title input field
        data.text = inputField.text; //the value entered by the user in the main input field
        data.cameraCoordinates = Camera.main.transform.position; //current position of the camera
        data.cameraRotation = Camera.main.transform.rotation; //current rotation of the camera
        data.cameraDisplacement = CameraController.displacement; //the amount the user has displaced the camera
        data.annotationPosition = pos; //the position the text of the annotation should appear (same as the position the annotationPin was dropped)
        data.screenDimensions = new Vector2(Screen.width, Screen.height); //current screen dimensions (so the position in which the text appears on any screen looks the same)
        data.planeNormal = plane.transform.up; //current plane normal (for saving cross sectional views)
        data.planePosition = plane.transform.position; //current plane position (for saving cross sectional views)
        data.colours = new List<Color>();
        foreach(GameObject g in ModelHandler.current.segments)data.colours.Add(g.GetComponent<MeshRenderer>().material.color); //list of the colours (r,g,b,a) of the segments) 
    }

    /*Convert the AnnotationData object to a string and write it to a file in a folder with the same name as the model currently being viewed*/
    private void writeAnnotationToJsonFile(){
        String jsonAnnotation = JsonUtility.ToJson(data);
        string dirPath = Path.Combine(Application.dataPath, FileHelper.currentAnnotationFolder);
        if(!Directory.Exists(dirPath)){
            DirectoryInfo dir = Directory.CreateDirectory(dirPath);
        }
        string filePath = Path.Combine(dirPath, titleInputField.text + ".json");
        File.WriteAllText(filePath, jsonAnnotation);
    }
    
}


