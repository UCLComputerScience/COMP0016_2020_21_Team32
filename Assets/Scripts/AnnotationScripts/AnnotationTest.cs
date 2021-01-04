using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnotationTest : MonoBehaviour
{
    [SerializeField] private Annotation annotation;
    [SerializeField] private Button button;

    private void Start()
    {
        annotation.hide();
        String title = "Annotation #" + annotation.annotationId;
        button.GetComponent<Button>().onClick.AddListener(() => annotation.show(title, "enter...", 
        (string input) => Debug.Log(input),() => Debug.Log("Cancel")));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
