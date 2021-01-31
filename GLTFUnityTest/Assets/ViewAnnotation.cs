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
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMP_Text annotationText;
    private bool isEnabled = false;
    public List<AnnotationData> annotations = new List<AnnotationData>();
    public List<string> annotationTitles = new List<string>();
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
        Camera.main.gameObject.transform.position = annotations[index].cameraCoordinates;
        Camera.main.gameObject.transform.rotation = annotations[index].cameraRotation;
        annotationText.rectTransform.position = annotations[index].annotationPosition;
        annotationText.text = annotations[index].text;
        annotationText.gameObject.SetActive(true);

    }
    private void jsonToAnnotations(){
        annotations = new List<AnnotationData>();
        annotationTitles = new List<String>();
        dropdown.ClearOptions();
        String path = Application.persistentDataPath;
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.json");
        
        foreach (FileInfo f in info){
            //if(!f.Exists)f.Create();
            String jsonToParse = File.ReadAllText(f.FullName);
            // Debug.Log(f.FullName);
            // Debug.Log(jsonToParse);
            annotations.Add(JsonUtility.FromJson<AnnotationData>(jsonToParse) as AnnotationData);
        }
        //Debug.Log("Length of annotations: " +annotations.Count);
        Annotation.setNumAnnotations(annotations.Count);
        foreach(AnnotationData annotation in annotations){
            annotationTitles.Add(annotation.title);
        }
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
        dropdown.gameObject.SetActive(false);
        annotationText.gameObject.SetActive(false);
        isEnabled = false;
    }
}
