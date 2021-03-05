using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dicom;
using Dicom.Imaging;
using SFB;
using System;
using UnityEngine.UI;

public class DICOMViewer : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] Slider dicomSlider;
    private bool isEnabled;
    private String[] paths;
    private List<Texture2D> images;
    // Start is called before the first frame update

    void Start(){
        dicomSlider.minValue = 0;
        dicomSlider.wholeNumbers = true;
        dicomSlider.onValueChanged.AddListener(viewImage);
        subscribeToEvents();
        images = new List<Texture2D>();
    }
    private void subscribeToEvents(){
        EventManager.current.OnEnableCamera += EventManager_otherEvent;
        EventManager.current.OnEnablePivot += EventManager_otherEvent;
        EventManager.current.OnEnableCrossSection += EventManager_otherEvent;
        EventManager.current.OnEnableDicom += EventManager_DicomView;
        EventManager.current.OnReset += EventManager_otherEvent;
        EventManager.current.OnViewAnnotations += EventManager_otherEvent;
        EventManager.current.OnAddAnnotations += EventManager_otherEvent;
    }

    private void EventManager_DicomView(object sender, EventArgs e){
        Debug.Log("Hey hey hey");
        var extension = new [] {new ExtensionFilter("DICOM", "dcm")};
        paths = StandaloneFileBrowser.OpenFilePanel("Select one or multiple dcm files", "", extension, true);
        foreach(String path in paths){
            images.Add(new DicomImage(path).RenderImage().AsTexture2D());
        }
        image.texture = images[0];
        Debug.Log("This is the width of the first image: "+ images[0].width);
        StartCoroutine(initialiseSliderMaxValue());
    }
    private IEnumerator initialiseSliderMaxValue(){
        yield return new WaitUntil(() => images.Count != 0);
        dicomSlider.maxValue = images.Count;
    }
    public void viewImage(float index){
        image.texture = images[(int) index];
    }
    private void EventManager_otherEvent(object sender, EventArgs e){
        isEnabled = false;
    }
}
