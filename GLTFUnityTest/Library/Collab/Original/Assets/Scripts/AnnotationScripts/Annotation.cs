using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
///<summary>This class is attatched to the Annotation prefab. It initialises the text in the input fields, provides
/// interactivity to the confirm/cancel buttons, and uses the user input (along with the other relevant data available in the scene)
<<<<<<< HEAD
/// to instantiate a new AnnotationData object. On confirm, this object is converted to JSON format and  written to a file.</summary>
=======
/// to instantiate a new AnnotationData object and write this to a file in JSON format.</summary>
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
public class Annotation : MonoBehaviour
{
    public Canvas canvas;
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
        canvas = GetComponentInParent<Canvas>();
        plane = GameObject.Find("Plane");
        confirmButton = this.transform.Find("Confirm").GetComponent<Button>();
        cancelButton = this.transform.Find("Cancel").GetComponent<Button>();
        titleInputField = this.transform.Find("title input field").GetComponent<TMP_InputField>();
        inputField = this.transform.Find("input field").GetComponent<TMP_InputField>(); 
        titleInputField.text = "Annotation #" + numAnnotations; 
        inputField.text = "enter...";
<<<<<<< HEAD

    }
    void Start(){
        hide(); //initially set inactive
    }
    /*Sets the annotation prefab to active and adds callbacks to the onclick events of the confirm and cancel buttons. Pos is the position in which the annotation
    pin was dropped*/
=======
    }

>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    public void show(Vector3 pos){
        EventManager.current.onEnableUIBlocker();
        gameObject.SetActive(true);
        data = new AnnotationData();
        confirmButton.onClick.AddListener(() => {
<<<<<<< HEAD
            save(pos);
=======
            save(titleInputField.text, inputField.text, pos);
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
            annotationPin.SetActive(false);
            numAnnotations++;
            hide();
        });
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => {
            annotationPin.SetActive(false);
            hide();
        });
    }
<<<<<<< HEAD

    /*Called when the confirm button is pressed. Instantiates a new AnnotationData object, converts it to a JSON string and writes that string to a file.*/
    public void save(Vector3 pos){
=======
    public void save(String title, String input, Vector3 pos){
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
        initialiseAnnotation(pos);
        writeAnnotationToJsonFile();
    }    
    public void hide(){
        ToolTip.current.gameObject.SetActive(false);
        gameObject.SetActive(false);
        EventManager.current.onDisableUIBlocker();
<<<<<<< HEAD
=======
        Debug.Log(inputField.text == null);
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
        titleInputField.text = "Annotation #" + numAnnotations;
        inputField.text = "enter...";
        EventManager.current.onEnableCamera();
    }
<<<<<<< HEAD
    /*Populate the AnnotationData object created when show() was called*/
    private void initialiseAnnotation(Vector3 pos){
        data.title = titleInputField.text; //the value entered by the user in the title input field
        data.text = inputField.text; //the value entered by the user in the main input field
        data.cameraCoordinates = Camera.main.transform.position; //current position of the camera
        data.cameraRotation = Camera.main.transform.rotation; //current rotation of the camera
        data.annotationPosition = pos; //the position the text of the annotation should appear (same as the position the annotationPin was dropped)
        data.screenDimensions = new Vector2(Screen.width, Screen.height); //current screen dimensions (so the position in which the text appears on any screen looks the same)
        data.planeNormal = plane.transform.up; //current plane normal (for saving cross sectional views)
        data.planePosition = plane.transform.position; //current plane position (for saving cross sectional views)
        data.colours = new List<Color>();
        foreach(GameObject g in ModelHandler.segments)data.colours.Add(g.GetComponent<MeshRenderer>().material.color); //list of the colours (r,g,b,a) of the segments of the model being viewed
    }

    /*Convert the AnnotationData object to a string and write it to a file in a folder with the same name as the model currently being viewed*/
    private void writeAnnotationToJsonFile(){
        String jsonAnnotation = JsonUtility.ToJson(data);
        string dirPath = Path.Combine(Application.dataPath, FileHelper.currentAnnotationFolder);
=======
    private void initialiseAnnotation(Vector3 pos){
        Debug.Log("Annotation pin position" + pos);
        data.title = titleInputField.text;
        data.text = inputField.text;
        data.cameraCoordinates = Camera.main.transform.position;
        data.cameraRotation = Camera.main.transform.rotation;
        data.colours = new List<Color>();
        data.annotationPosition = pos;
        data.screenDimensions = new Vector2(Screen.width, Screen.height);
        data.planeNormal = plane.transform.up;
        data.planePosition = plane.transform.position;
        foreach(GameObject g in ModelHandler.segments)data.colours.Add(g.GetComponent<MeshRenderer>().material.color);
    }
    private void writeAnnotationToJsonFile(){
        Debug.Log("data.annotationPosition: "+data.annotationPosition);
        String jsonAnnotation = JsonUtility.ToJson(data);
        Debug.Log(FileHelper.currentAnnotationFolder);
        Debug.Log("This is the "+Application.dataPath);
        Debug.Log("Full thing: "+Path.Combine(Application.dataPath, FileHelper.currentAnnotationFolder));
        string dirPath = Path.Combine(Application.dataPath, FileHelper.currentAnnotationFolder);
        Debug.Log("Write annotations to dirpath "+dirPath);
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
        if(!Directory.Exists(dirPath)){
            DirectoryInfo dir = Directory.CreateDirectory(dirPath);
        }
        string filePath = Path.Combine(dirPath, titleInputField.text + ".json");
        File.WriteAllText(filePath, jsonAnnotation);
    }
    
}


