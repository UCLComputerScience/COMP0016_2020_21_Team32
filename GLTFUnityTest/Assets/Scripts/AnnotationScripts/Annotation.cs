using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Annotation : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    public AnnotationData data;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject UIBlocker;
    public static int numAnnotations = 0;
    public int annotationId; 
    [SerializeField] GameObject AnnotationPin;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField inputField;

    public static void setNumAnnotations(int value){
        numAnnotations = value;
    }
    void Awake(){
        canvas = GetComponentInParent<Canvas>();
        titleInputField.text = "Annotation #" + numAnnotations; 
        inputField.text = "enter...";
    }

    public void show(Vector3 pos){
        UIBlocker.SetActive(true);
        gameObject.SetActive(true);
        data = new AnnotationData();
        annotationId = numAnnotations;
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => {
            save(titleInputField.text, inputField.text, pos);
            AnnotationPin.SetActive(false);
            numAnnotations++;
            hide();
        });
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => {
            AnnotationPin.SetActive(false);
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
        UIBlocker.SetActive(false);
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


