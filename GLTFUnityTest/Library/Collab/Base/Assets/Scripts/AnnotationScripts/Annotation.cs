using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
///<summary>This class is attatched to the Annotation prefab. It initialises the text in the input fields, provides
/// interactivity to the confirm/cancel buttons, and uses the user input (along with the other relevant data available in the scene)
/// to instantiate a new AnnotationData object and write this to a file in JSON format.</summary>
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
    }

    public void show(Vector3 pos){
        EventManager.current.onEnableUIBlocker();
        gameObject.SetActive(true);
        data = new AnnotationData();
        confirmButton.onClick.AddListener(() => {
            save(titleInputField.text, inputField.text, pos);
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
    public void save(String title, String input, Vector3 pos){
        initialiseAnnotation(pos);
        writeAnnotationToJsonFile();
    }    
    public void hide(){
        ToolTip.current.gameObject.SetActive(false);
        gameObject.SetActive(false);
        EventManager.current.onDisableUIBlocker();
        Debug.Log(inputField.text == null);
        titleInputField.text = "Annotation #" + numAnnotations;
        inputField.text = "enter...";
        EventManager.current.onEnableCamera();
    }
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
        if(!Directory.Exists(dirPath)){
            DirectoryInfo dir = Directory.CreateDirectory(dirPath);
        }
        string filePath = Path.Combine(dirPath, titleInputField.text + ".json");
        File.WriteAllText(filePath, jsonAnnotation);
    }
    
}


