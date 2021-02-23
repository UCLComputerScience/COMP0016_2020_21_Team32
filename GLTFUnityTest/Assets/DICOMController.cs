using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Dicom;
using Dicom.Imaging;
using SFB;
using UnityEngine.EventSystems;

public class DICOMController : MonoBehaviour, IDragHandler 
{
    Canvas canvas;
    String[] paths;
    RawImage viewArea;
    Slider dicomSlider;
    List<Texture2D> images;
    RectTransform rectTransform; 
    void Awake(){
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<Image>().GetComponent<RectTransform>();
        images = new List<Texture2D>();
        viewArea = GetComponentInChildren<RawImage>();
        dicomSlider = GetComponentInChildren<Slider>();
        dicomSlider.minValue = 0;
        dicomSlider.wholeNumbers = true;
        dicomSlider.onValueChanged.AddListener(viewImage);
    }
    public void OnDrag(PointerEventData data){
        Debug.Log("Draggin");
        rectTransform.anchoredPosition += data.delta /canvas.scaleFactor; 
        //movement delta - amount mouse moved since previous frame
        //must be divided by canvas scale factor because of the difference between mouse movement and canvas scale. This will vary 
        //due to the canvas adjusting itself to fit on every screen.
    }

    void OnEnable(){
        Debug.Log("Hey hey hey");
        EventManager.current.onEnableCamera();
        var extension = new [] {new ExtensionFilter("DICOM", "dcm")};
        paths = StandaloneFileBrowser.OpenFilePanel("Select one or multiple dcm files", "", extension, true);
        foreach(String path in paths){
            images.Add(new DicomImage(path).RenderImage().AsTexture2D());
        }
        viewArea.texture = images[0];
        Debug.Log("This is the width of the first image: "+ images[0].width);
        StartCoroutine(initialiseSliderMaxValue());
        Debug.Log(CameraMovement.isEnabled);
    }
    private IEnumerator initialiseSliderMaxValue(){
        yield return new WaitUntil(() => images.Count != 0);
        dicomSlider.maxValue = images.Count;
    }
    public void viewImage(float index){
        if(index >= images.Count || index < 0)return;
        viewArea.texture = images[(int) index];
    }

    void OnDisable(){

    }

}
