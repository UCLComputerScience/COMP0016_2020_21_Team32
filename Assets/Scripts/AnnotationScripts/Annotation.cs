using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Annotation : MonoBehaviour
{
    public static int numAnnotations = 0;
    public int annotationId; 
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TMP_InputField inputField;
    public void show(String title, String input, Action<string> onConfirm, Action onCancel){
        annotationId = numAnnotations;
        gameObject.SetActive(true);
        titleText.text =  title;
        inputField.text = input;
        confirmButton.onClick.AddListener(() =>{ 
            hide();
            onConfirm(inputField.text);
            numAnnotations++;
        });
        cancelButton.onClick.AddListener(() => {
            hide();
            onCancel();
        });
    }
    public void hide(){
        gameObject.SetActive(false);
    }
    
}
