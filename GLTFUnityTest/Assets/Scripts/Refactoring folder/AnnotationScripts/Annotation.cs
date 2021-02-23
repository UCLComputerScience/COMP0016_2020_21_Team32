using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Annotation : MonoBehaviour
{
    public AnnotationData data;
    [SerializeField] GameObject plane;
    [SerializeField] GameObject UIBlocker;
    [SerializeField] GameObject organ;
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
        //organ = LoadBrain.current.gameObject;
        titleInputField.text = "Annotation #" + numAnnotations; 
    }

    public void show(Vector3 pos){
        UIBlocker.SetActive(true);
        gameObject.SetActive(true);
        data = new AnnotationData();
        annotationId = numAnnotations;
        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => {
            Debug.Log("confirm button clicked");
            save(titleInputField.text, inputField.text, pos);
            AnnotationPin.SetActive(false);
            numAnnotations++;
            hide();
        });
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => {
            Debug.Log("Cancelling");
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
        EventManager.current.onEnableCamera();
    }
    private void initialiseAnnotation(Vector3 pos){
        data.title = titleInputField.text;
        data.text = inputField.text;
        data.cameraCoordinates = Camera.main.transform.position;
        data.cameraRotation = Camera.main.transform.rotation;
        data.colours = new List<Color>();
        data.annotationPosition = pos;
        data.planeNormal = plane.transform.up;
        data.planePosition = plane.transform.position;
        foreach(GameObject g in ModelHandler.segments)data.colours.Add(g.GetComponent<MeshRenderer>().material.color);
    }
    private void writeAnnotationToJsonFile(){
        String jsonAnnotation = JsonUtility.ToJson(data);
        string dirPath = Path.Combine(Application.dataPath, ModelHandler.annotationFolder);
        if(!Directory.Exists(dirPath)){
            DirectoryInfo dir = Directory.CreateDirectory(dirPath);
        }
        string filePath = Path.Combine(dirPath, titleInputField.text + ".json");
        File.WriteAllText(filePath, jsonAnnotation);
    }
    
}

    // public void show(String title, String input, Action<string> onConfirm, Action onCancel){
    //     data = new AnnotationData();
    //     Debug.Log(numAnnotations);
    //     annotationId = numAnnotations;
    //     gameObject.SetActive(true);
    //     titleInputField.text =  title;
    //     inputField.text = input;
    //     confirmButton.onClick.AddListener(() =>{ 
    //         hide();
    //         onConfirm(inputField.text);
    //         AnnotationPin.SetActive(false);
    //         numAnnotations++;
    //     });
    //     cancelButton.onClick.AddListener(() => {
    //         hide();
    //         //AnnotationPin.transform.position = new Vector3(0f, 0f, 0f);
    //         AnnotationPin.SetActive(false);
    //         onCancel();
    //     });
    // }
