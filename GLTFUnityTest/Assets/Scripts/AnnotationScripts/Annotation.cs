using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Annotation : MonoBehaviour
{
    public AnnotationData data;

    [SerializeField] GameObject organ;
    public static int numAnnotations = 0;
    public int annotationId; 
    [SerializeField] GameObject AnnotationPin;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TMP_InputField inputField;
    public void show(String title, String input, Action<string> onConfirm, Action onCancel){
        data = new AnnotationData();
        numAnnotations++;
        annotationId = numAnnotations;
        gameObject.SetActive(true);
        titleText.text =  title;
        inputField.text = input;
        confirmButton.onClick.AddListener(() =>{ 
            hide();
            onConfirm(inputField.text);
            AnnotationPin.transform.position = new Vector3(0f, 0f, 0f);
            AnnotationPin.SetActive(false);
            numAnnotations++;
        });
        cancelButton.onClick.AddListener(() => {
            hide();
            AnnotationPin.transform.position = new Vector3(0f, 0f, 0f);
            AnnotationPin.SetActive(false);
            onCancel();
        });
    }
    public void save(String title, String input){
        data.title = title;
        data.text = input;
        data.cameraCoordinates = Camera.main.transform.position;
        data.cameraRotation = Camera.main.transform.rotation;
        data.colours = new List<Color>();
        foreach(Transform transform in organ.transform){
            data.colours.Add(transform.gameObject.GetComponent<MeshRenderer>().material.color);
        }
    }
    public void hide(){
        gameObject.SetActive(false);
    }
    
}
