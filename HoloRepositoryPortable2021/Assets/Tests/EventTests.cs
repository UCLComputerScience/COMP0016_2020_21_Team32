using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.IO;
using TMPro;

namespace Tests
{
    /*Testing all the events that the event manager can fire*/
    public class EventTests
    {
        private GameObject publisher;
        private GameObject model;
        private GameObject eventListener;
        private GameObject slider;
        private GameObject plane;
        private GameObject pivot;
        private GameObject camera;
        private EventManager eventManager;
        private ModelHandler modelHandler;
        // private GameObject annotationPin;
        // private GameObject settingsController;
        // private GameObject DICOMController;
        // private GameObject planeController;
        // private GameObject pivotController;
        // private GameObject settingsButton;
        // private GameObject fullScreenToggle;
        // private GameObject segmentSelectButton;
        // private GameObject colourPalette;
        // private GameObject navigationBar;
        // private GameObject exitButton;
        // private GameObject logos;
        // private GameObject dropdown;
        // private GameObject mainPage;
        // private GameObject UIBlocker;
        // private GameObject controllerHandler;
        // private GameObject canvas;
        // private GameObject annotationTextBox;
        // private GameObject annotationText;
        // private GameObject annotation;

        private void initialiseTestScene(){
            Debug.Log("Startin up");
            publisher = new GameObject();
            publisher.AddComponent<EventManager>();
            model = new GameObject();
            eventListener = new GameObject();
            eventListener.SetActive(false);
            camera = new GameObject("MainCamera");
            camera.AddComponent<Camera>();
            camera.tag = "MainCamera";
            slider = new GameObject();
            pivot = new GameObject();
            eventManager = publisher.GetComponent<EventManager>();
            Debug.Log(eventManager);
            // plane = new GameObject("Plane");
            // pivot = new GameObject("Pivot");
            // camera = new GameObject("MainCamera");
            // annotationPin = new GameObject("Annotation Pin"); 
            // navigationBar = new GameObject("Navigation bar");
            // colourPalette = new GameObject("Colour Palette"); 
            // mainPage = new GameObject("Main Page");
            // exitButton = new GameObject("Exit");
            // settingsButton = new GameObject("Settings");
            // fullScreenToggle = new GameObject("Toggle full screen");
            // segmentSelectButton = new GameObject("Toggle Selected Segment");
            // UIBlocker = new GameObject("UIBlocker");
            // controllerHandler = new GameObject("Controller Handler");
            // DICOMController = new GameObject("Dicom Controller");
            // pivotController = new GameObject("Pivot Controller");
            // planeController = new GameObject("Plane Controller");
            // settingsController = new GameObject("Settings Controller");
            // dropdown = new GameObject("Dropdown");
            // canvas = new GameObject("Canvas");
            // annotation = new GameObject("Annotation");
            // annotationTextBox = new GameObject("Annotation Textbox");
            // annotationText = new GameObject("AnnotationText");


            // var cam = camera.AddComponent<Camera>();
            // cam.tag = "MainCamera";
            // slider.AddComponent<Slider>();
            // settingsButton.AddComponent<Button>();
            // fullScreenToggle.AddComponent<Toggle>();
            // dropdown.AddComponent<TMP_Dropdown>();
            // exitButton.AddComponent<Button>();
            // segmentSelectButton.AddComponent<Button>();
            // canvas.AddComponent<Canvas>();
            // eventManager = publisher.AddComponent<EventManager>();
            // modelHandler = model.AddComponent<ModelHandler>();
            // annotationText.AddComponent<TMP_Text>();


            

            // pivotController.transform.SetParent(controllerHandler.transform);
            // planeController.transform.SetParent(controllerHandler.transform);
            // DICOMController.transform.SetParent(controllerHandler.transform);
            // settingsController.transform.SetParent(controllerHandler.transform);

            // annotationText.transform.SetParent(annotationTextBox.transform);

            // annotation.transform.SetParent(canvas.transform);
            // dropdown.transform.SetParent(canvas.transform);
            // controllerHandler.transform.SetParent(canvas.transform);
            // navigationBar.transform.SetParent(canvas.transform);
            // colourPalette.transform.SetParent(canvas.transform);
            // UIBlocker.transform.SetParent(canvas.transform);
            // exitButton.transform.SetParent(canvas.transform);
            // annotationPin.transform.SetParent(canvas.transform);
            // slider.transform.SetParent(canvas.transform);
            // annotationTextBox.transform.SetParent(canvas.transform);
        }
        private void loadModel(){
            modelHandler = eventListener.AddComponent<ModelHandler>();
            modelHandler.plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        }

        [UnitySetUp]
        public IEnumerator setUp(){
            yield return new EnterPlayMode();
            initialiseTestScene();
            loadModel();
        }

        [UnityTearDown]
        public IEnumerator tearDown(){
            Object.Destroy(modelHandler);
            Object.Destroy(eventManager);
            Object.Destroy(eventListener);
            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator EventManagerTest_onCameraEnabled()
        {
            var cameraController = eventListener.AddComponent<CameraController>();
            cameraController.pivot = pivot;
            cameraController.isEnabled = false;
            eventListener.SetActive(true);
            Debug.Log(eventManager);
            yield return null;
            eventManager.onEnableCamera();
            Assert.True(cameraController.isEnabled);
        }

        [UnityTest]
        public IEnumerator EventManagerTest_onReset(){
            var resetter = eventListener.AddComponent<CameraResetter>();
            resetter.isEnabled = false;
            eventListener.SetActive(true);
            yield return null;
            eventManager.onReset();
            Assert.True(resetter.isEnabled);
        }

        [UnityTest]
        public IEnumerator EventManagerTest_onAddAnnotation(){
            var annotationPin = new GameObject();
            annotationPin.SetActive(false);
            var uiManager = eventListener.AddComponent<UIManager>();
            uiManager.annotationPin = annotationPin;
            eventListener.SetActive(true);
            yield return null;
            eventManager.onAddAnnotations();
            Assert.True(annotationPin.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManagerTest_onViewAnnotation(){
            var dropdown = new GameObject();
            var dropDown = dropdown.AddComponent<TMP_Dropdown>();
            var annotationTextBox = new GameObject();
            var annotationText = annotationTextBox.AddComponent<TMP_Text>();
            var annotationSelector = eventListener.AddComponent<AnnotationSelector>();
            annotationSelector.dropdown = dropDown;
            annotationSelector.annotationTextBox = annotationTextBox;
            dropdown.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onViewAnnotations();
            Assert.True(dropdown.activeInHierarchy);
        }
        
        [UnityTest]
        public IEnumerator EventManger_onEnableDicom(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var dicomController = new GameObject();
            uiManager.dicomController = dicomController;
            dicomController.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onEnableDicom();
            Assert.True(dicomController.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManger_onEnablePivot(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var pivotController = new GameObject();
            uiManager.pivotController = pivotController;
            pivotController.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onEnablePivot();
            Assert.True(pivotController.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManger_onEnableCrossSection(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var planeController = new GameObject();
            uiManager.planeController = planeController;
            planeController.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onEnableCrossSection();
            Assert.True(planeController.activeInHierarchy);
        }

        [UnityTest]
        public IEnumerator EventManager_onEnableUIBlocker(){
            Debug.Log(eventManager);
            var uiManager = eventListener.AddComponent<UIManager>();
            var UIBlocker = new GameObject();
            uiManager.UIBlocker = UIBlocker;
            UIBlocker.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onEnableUIBlocker();
            Assert.True(UIBlocker.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onDisableUIBlocker(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var UIBlocker = new GameObject();
            uiManager.UIBlocker = UIBlocker;
            UIBlocker.SetActive(true);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onDisableUIBlocker();
            Assert.False(UIBlocker.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onToggleColourPalette(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var colourPalette = new GameObject();
            uiManager.colourPalette = colourPalette;
            colourPalette.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onToggleColourPalette();
            Assert.True(colourPalette.activeInHierarchy);
            eventManager.onToggleColourPalette();
            Assert.False(colourPalette.activeInHierarchy);
        }

        [UnityTest]
        public IEnumerator EventManager_onToggleLogos(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var logos = new GameObject();
            uiManager.logos = logos;
            logos.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onToggleLogos();
            Assert.True(logos.activeInHierarchy);
            eventManager.onToggleLogos();
            Assert.False(logos.activeInHierarchy);
        }
        
        [UnityTest]
        public IEnumerator EventManager_onToggleNavigationBar(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var navigationBar = new GameObject();
            uiManager.navigationBar = navigationBar;
            navigationBar.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onToggleNavigationBar();
            Assert.True(navigationBar.activeInHierarchy);
            eventManager.onToggleNavigationBar();
            Assert.False(navigationBar.activeInHierarchy);
        }

        [UnityTest]
        public IEnumerator EventManager_onToggleSegmentSelectButton(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var segmentSelectButton = new GameObject();
            uiManager.segmentSelect = segmentSelectButton;
            segmentSelectButton.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onToggleSegmentSelect();
            Assert.True(segmentSelectButton.activeInHierarchy);
            eventManager.onToggleSegmentSelect();
            Assert.False(segmentSelectButton.activeInHierarchy);
        }

        [UnityTest]
        public IEnumerator EventManager_onToggleOpacitySlider(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var opacitySlider = new GameObject();
            uiManager.opacitySlider = opacitySlider;
            opacitySlider.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onToggleOpacitySlider();
            Assert.True(opacitySlider.activeInHierarchy);
            eventManager.onToggleOpacitySlider();
            Assert.False(opacitySlider.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onToggleFullScreen(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var mainPage = new GameObject();
            uiManager.mainPage = mainPage;
            mainPage.SetActive(false);
            eventListener.SetActive(true);
            yield return null;
            eventManager.onToggleFullScreen();
            Assert.True(mainPage.activeInHierarchy);
            eventManager.onToggleFullScreen();
            Assert.False(mainPage.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onColourSelect(){
            eventListener.SetActive(true);
            Color newCol = new Color(1f,0f,0f,1f); //set new colour red
            yield return new  WaitUntil(() => ModelHandler.current.modelRadius != 0); //wait for model to be loaded
            Color initialColour = ModelHandler.current.segments[0].GetComponent<Renderer>().material.color = new Color(0f,1f,0f,1f); //set initial colour green
            Assert.AreNotEqual(initialColour, newCol);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      
            
            eventManager.onColourSelect(newCol);
            Assert.AreEqual(newCol, ModelHandler.current.segments[0].GetComponent<Renderer>().material.color);
        }
        [UnityTest]
        public IEnumerator EventManager_onChangeOpacity(){
            eventListener.SetActive(true);
            Color initialColour = new Color(0f, 1f, 0f, 1f);
            float newOp = 0.5f;
            Color colourAfterEvent = new Color(0f, 1f, 0f, newOp);
            yield return new  WaitUntil(() =>  ModelHandler.current.modelRadius != 0); //wait for model to be loaded
            ModelHandler.current.segments[0].GetComponent<Renderer>().material.color = initialColour; //set initial colour green
            Assert.AreNotEqual(initialColour, colourAfterEvent);
            eventManager.onChangeOpacity(newOp);
            Assert.AreEqual(colourAfterEvent, ModelHandler.current.segments[0].GetComponent<Renderer>().material.color);
        }

        [UnityTest]
        public IEnumerator EventManger_onChangeSettings(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var settingsController = new GameObject();
            uiManager.settingsController = settingsController;
            settingsController.SetActive(false);
            eventListener.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            eventManager.onChangeSettings();
            Assert.True(settingsController.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onSegmentSelect(){
            eventListener.SetActive(true);
            Color initialColour = Color.red; 
            Color finalColour = Color.green;
            yield return new WaitUntil(()=> ModelHandler.current.modelRadius != 0);
            ModelHandler.current.segments[0].GetComponent<Renderer>().material.color = initialColour;
            ModelHandler.current.segments[1].GetComponent<Renderer>().material.color = initialColour;
            eventManager.onColourSelect(finalColour);
            eventManager.onSegmentSelect();
            eventManager.onColourSelect(finalColour);
            Assert.AreEqual(ModelHandler.current.segments[0].GetComponent<Renderer>().material.color, finalColour);
            Assert.AreEqual(ModelHandler.current.segments[1].GetComponent<Renderer>().material.color, finalColour);
        }
    }
}
