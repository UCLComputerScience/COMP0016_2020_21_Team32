using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using SFB;
using UnityEngine.EventSystems;
using System.IO;

///<summary>This script is attached to the  DICOMController prefab and provides it with interactivity.</summary>
public class DICOMController : MonoBehaviour, IBeginDragHandler, IDragHandler 
{
    private DicomToPNG converter;
    private Button hideButton;
    private Button loadButton;
    private Canvas canvas;
    private String[] paths;
    private RawImage viewArea;
    private Slider dicomSlider;
    private List<Texture2D> images;
    private RectTransform canvasRect;
    private RectTransform rectTransform; 
    private RectTransform viewAreaRect;
    private Vector3 dragOffset;

    /*Initialise variables*/
    void Awake(){
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        rectTransform = GetComponent<Image>().GetComponent<RectTransform>();
        images = new List<Texture2D>();
        viewArea = GetComponentInChildren<RawImage>();
        viewAreaRect = viewArea.GetComponent<RectTransform>();
        dicomSlider = GetComponentInChildren<Slider>();
        dicomSlider.minValue = 0;
        dicomSlider.wholeNumbers = true;
        dicomSlider.onValueChanged.AddListener(viewImage);
        hideButton = this.transform.Find("Hide panel").GetComponent<Button>();
        loadButton = this.transform.Find("Load different scans").GetComponent<Button>();
        hideButton.onClick.AddListener(hide);
        loadButton.onClick.AddListener(openFileExplorer);
        converter = new DicomToPNG((int)rectTransform.rect.width, (int)rectTransform.rect.height);
    }

    /*Drag and drop functionality (implements the IBeginDragHandler and IEndDragHandler interfaces)*/
    public void OnBeginDrag(PointerEventData data){
        dragOffset = (Vector3)data.position - rectTransform.position;
    }
    public void OnDrag(PointerEventData data){      
        rectTransform.position = (Vector3)data.position - dragOffset;
    }

/*Open the native file browser using the StandAloneFileBrowser library. Updates the controller with the .dcm files selected by the user. 
  Called when the load button is pressed*/
    public void openFileExplorer(){
        var extension = new [] {new ExtensionFilter("DICOM", "dcm")};
        paths = StandaloneFileBrowser.OpenFilePanel("Select one or multiple dcm files", "", extension, true);
        updateImages();
    }

/*Called when the hideButton is pressed - disables the controller, disables the tooltip and fires an onEnableCamera event so the user can start moving the camera.*/
    public void hide(){
        ToolTip.current.gameObject.SetActive(false);
        EventManager.current.onEnableCamera();
        this.gameObject.SetActive(false);
    }
    void OnEnable(){
        if(paths == null) openFileExplorer();
        else{
            EventManager.current.onEnableCamera();
            return;
        }
    }
    /*Convert the selected .dcm files into Texture2D using the converter object. Populate the images
    array with these Texture2D objects so they can be loaded onto the viewArea */
    private void updateImages(){
        if(paths.Length == 0){ //If the user presses cancel in the fileExplorer disable the controller
            ToolTip.current.gameObject.SetActive(false);
            paths = null;
            EventManager.current.onEnableCamera();
            this.gameObject.SetActive(false);
            return;
        }
        images.Clear(); //clear existing images if loadButton is pressed
        foreach(String path in paths){
            string outputfile = Path.Combine(Application.dataPath, path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar)) + ".png");
            images.Add(converter.ReadDICOM(path));
        }
        StartCoroutine(initialiseSliderMaxValue());
    }

    /*Coroutine that sets the maxValue of the slider equal to the number of .dcm files selected by the user.*/
    private IEnumerator initialiseSliderMaxValue(){
        yield return new WaitUntil(() => images.Count != 0);
        viewArea.texture = images[0]; //initialise the viewArea with the first selected dcm
        dicomSlider.maxValue = images.Count;
        EventManager.current.onEnableCamera();
    }

    /*Passed as a callback action to the OnValueChanged event of the slider. Changes the .dcm being viewed.*/
    public void viewImage(float index){
        if(index >= images.Count || index < 0)return;
        viewArea.texture = images[(int) index];
    }
}
