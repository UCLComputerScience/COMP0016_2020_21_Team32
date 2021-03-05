<<<<<<< HEAD
using System.Collections;
=======
﻿using System.Collections;
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
<<<<<<< HEAD
using SFB;
using UnityEngine.EventSystems;
using System.IO;

///<summary>This script is attached to the  DICOMController prefab and provides it with interactivity. The user can load in </summary>
public class DICOMController : MonoBehaviour, IBeginDragHandler, IDragHandler 
{
    #region variableDeclaration 
    private DicomToTexture2D converter;
=======
using Dicom;
using Dicom.Imaging;
using SFB;
using UnityEngine.EventSystems;

/**/
public class DICOMController : MonoBehaviour, IBeginDragHandler, IDragHandler 
{
    #region variableDeclaration 
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    private Button hideButton;
    private Button loadButton;
    private Canvas canvas;
    private String[] paths;
<<<<<<< HEAD
    private List<String> outputPaths;
=======
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    private RawImage viewArea;
    private Slider dicomSlider;
    private List<Texture2D> images;
    private RectTransform canvasRect;
    private RectTransform rectTransform; 
    private RectTransform viewAreaRect;
    private Vector3 dragOffset;
    #endregion variableDeclaration 

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
<<<<<<< HEAD
        converter = new DicomToTexture2D((int)rectTransform.rect.width, (int)rectTransform.rect.height);
        outputPaths = new List<String>();
    }

    /*Drag and drop functionality*/
    public void OnBeginDrag(PointerEventData data){
        dragOffset = (Vector3)data.position - rectTransform.position;
    }
    public void OnDrag(PointerEventData data){      
        rectTransform.position = (Vector3)data.position - dragOffset;
    }

/*Open native file browser and updates the controller with the dcm files selected by the user. Called when the load button is pressed*/
=======
    }

    
    public void OnBeginDrag(PointerEventData data){
        dragOffset = (Vector3)data.position - rectTransform.position;
    }
    	// 	Vector3 globalMousePos;
		// if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos))
		// {
		// 	rt.position = globalMousePos;
		// 	rt.rotation = m_DraggingPlanes[eventData.pointerId].rotation;
		// }
    public void OnDrag(PointerEventData data){      
        rectTransform.position = (Vector3)data.position - dragOffset;
        //rectTransform.anchoredPosition = data.position - dragOffset;
    }

    /*Open native file browser and update the controller with the dcm files selected by the user. Called when the load button is pressed*/
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    public void openFileExplorer(){
        var extension = new [] {new ExtensionFilter("DICOM", "dcm")};
        paths = StandaloneFileBrowser.OpenFilePanel("Select one or multiple dcm files", "", extension, true);
        updateImages();
    }

/*Called when the hideButton is pressed - disables the controller*/
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
<<<<<<< HEAD
    /*Convert the selected .dcm files into Texture2D objects by temporarily writing them to a png. Populate the images
    array with these textures loaded from the pngs so they can be loaded onto the viewArea */
=======
    /*Using the fo-Dicom library, convert the selected .dcm files into Texture2D objects. Populate the images
    array with these textures so they can be loaded onto the viewArea */
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    private void updateImages(){
        if(paths.Length == 0){ //If the user presses cancel in the fileExplorer disable the controller
            paths = null;
            EventManager.current.onEnableCamera();
            this.gameObject.SetActive(false);
            return;
        }
        images.Clear(); //clear existing images if loadButton is pressed
<<<<<<< HEAD
        outputPaths.Clear();
        foreach(String path in paths){
            string outputfile = Application.dataPath + Path.DirectorySeparatorChar + path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar)) + ".png";
            converter.ReadDICOM(path, outputfile, images);
            outputPaths.Add(outputfile);
        }
        foreach(string path in outputPaths){
            Texture2D tex = new Texture2D((int)rectTransform.rect.width, (int)rectTransform.rect.height);
            tex.LoadImage(File.ReadAllBytes(path));
            images.Add(tex);
        }
=======
        foreach(String path in paths){
            images.Add(new DicomImage(path).RenderImage().AsTexture2D()); 
        }
        viewArea.texture = images[0]; //initialise the viewArea with the first selected dcm
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
        StartCoroutine(initialiseSliderMaxValue());
    }

    /*Coroutine that sets the maxValue of the slider equal to the number of .dcm files selected by the user. Also 
    re-enables the camera.*/
    private IEnumerator initialiseSliderMaxValue(){
        yield return new WaitUntil(() => images.Count != 0);
<<<<<<< HEAD
        viewArea.texture = images[0]; //initialise the viewArea with the first selected dcm
        dicomSlider.maxValue = images.Count;
        EventManager.current.onEnableCamera();
        StartCoroutine(deleteFiles());
    }
    private IEnumerator deleteFiles(){
        yield return new WaitUntil(() => images.Count == outputPaths.Count);
        foreach(string path in outputPaths){
            File.Delete(path);
        }
        outputPaths.Clear();
        
=======
        dicomSlider.maxValue = images.Count;
        EventManager.current.onEnableCamera();
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    }

    /*Adjusting the slider will fire the onValueChanged UnityEvent, triggering this callback. It changes the dcm
    displayed on the panel based on the slider's position */
    public void viewImage(float index){
        if(index >= images.Count || index < 0)return;
        viewArea.texture = images[(int) index];
    }
<<<<<<< HEAD
    void onDestory(){
        foreach(string path in outputPaths){
            File.Delete(path);
        }
    }
=======
    // void LateUpdate(){
    //     Vector2 anchoredPos = Input.mousePosition / canvas.r.transform.localScale.x;
    //     if(anchoredPos.x + background.rect.width > canvasRect.rect.width){
    //         anchoredPos.x = canvasRect.rect.width - background.rect.width;
    //     }
    //     if(anchoredPos.y + background.rect.height > canvasRect.rect.height){
    //         anchoredPos.y = canvasRect.rect.height - background.rect.height;
    //     }
    //     rect.anchoredPosition = anchoredPos;
        
    // }
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
}
