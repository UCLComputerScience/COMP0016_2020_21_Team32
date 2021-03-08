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
        private GameObject annotationPin;
        private GameObject settingsController;
        private GameObject DICOMController;
        private GameObject planeController;
        private GameObject pivotController;
        private GameObject settingsButton;
        private GameObject fullScreenToggle;
        private GameObject segmentSelectButton;
        private GameObject colourPalette;
        private GameObject navigationBar;
        private GameObject exitButton;
        private GameObject logos;
        private GameObject dropdown;
        private GameObject mainPage;
        private GameObject UIBlocker;
        private GameObject controllerHandler;
        private GameObject canvas;
        private GameObject annotationTextBox;
        private GameObject annotationText;
        private GameObject annotation;

        private void initialiseTestScene(){
            publisher = new GameObject();
            model = new GameObject();
            eventListener = new GameObject();
            slider = new GameObject("Opacity Slider");
            plane = new GameObject("Plane");
            pivot = new GameObject("Pivot");
            camera = new GameObject("MainCamera");
            annotationPin = new GameObject("Annotation Pin"); 
            navigationBar = new GameObject("Navigation bar");
            colourPalette = new GameObject("Colour Palette"); 
            mainPage = new GameObject("Main Page");
            exitButton = new GameObject("Exit");
            settingsButton = new GameObject("Settings");
            fullScreenToggle = new GameObject("Toggle full screen");
            segmentSelectButton = new GameObject("Toggle Selected Segment");
            UIBlocker = new GameObject("UIBlocker");
            controllerHandler = new GameObject("Controller Handler");
            DICOMController = new GameObject("Dicom Controller");
            pivotController = new GameObject("Pivot Controller");
            planeController = new GameObject("Plane Controller");
            settingsController = new GameObject("Settings Controller");
            dropdown = new GameObject("Dropdown");
            canvas = new GameObject("Canvas");
            annotation = new GameObject("Annotation");
            annotationTextBox = new GameObject("Annotation Textbox");
            annotationText = new GameObject("AnnotationText");


            var cam = camera.AddComponent<Camera>();
            cam.tag = "MainCamera";
            slider.AddComponent<Slider>();
            settingsButton.AddComponent<Button>();
            fullScreenToggle.AddComponent<Toggle>();
            dropdown.AddComponent<TMP_Dropdown>();
            exitButton.AddComponent<Button>();
            segmentSelectButton.AddComponent<Button>();
            canvas.AddComponent<Canvas>();
            eventManager = publisher.AddComponent<EventManager>();
            modelHandler = model.AddComponent<ModelHandler>();
            annotationText.AddComponent<TMP_Text>();


            

            pivotController.transform.SetParent(controllerHandler.transform);
            planeController.transform.SetParent(controllerHandler.transform);
            DICOMController.transform.SetParent(controllerHandler.transform);
            settingsController.transform.SetParent(controllerHandler.transform);

            annotationText.transform.SetParent(annotationTextBox.transform);

            annotation.transform.SetParent(canvas.transform);
            dropdown.transform.SetParent(canvas.transform);
            controllerHandler.transform.SetParent(canvas.transform);
            navigationBar.transform.SetParent(canvas.transform);
            colourPalette.transform.SetParent(canvas.transform);
            UIBlocker.transform.SetParent(canvas.transform);
            exitButton.transform.SetParent(canvas.transform);
            annotationPin.transform.SetParent(canvas.transform);
            slider.transform.SetParent(canvas.transform);
            annotationTextBox.transform.SetParent(canvas.transform);
        }
        private void loadModel(){
            var eventManager = publisher.AddComponent<EventManager>();
            var modelHandler = model.AddComponent<ModelHandler>();
            Organ organ = OrganFactory.GetOrgan(Application.streamingAssetsPath + Path.DirectorySeparatorChar+ "brain.glb");
            organ.initialiseModel(model);
            ModelHandler.organ = organ;
        }

        [UnitySetUp]
        public IEnumerator setUp(){
            initialiseTestScene();
            loadModel();
            yield return new EnterPlayMode();
        }

        [UnityTearDown]
        public IEnumerator tearDown(){
            yield return new ExitPlayMode();
        }

        [UnityTest]
        public IEnumerator EventManagerTest_onCameraEnabled()
        {
            var cameraController = eventListener.AddComponent<CameraController>();
            cameraController.pivot = pivot;
            cameraController.isEnabled = false;

            yield return new WaitUntil(()=> cameraController != null);
            eventManager.onEnableCamera();
            Assert.True(cameraController.isEnabled);
        }

        [UnityTest]
        public IEnumerator EventManagerTest_onReset(){
            var resetter = eventListener.AddComponent<CameraResetter>();
            resetter.isEnabled = false;

            yield return new WaitUntil(()=> resetter != null);
            eventManager.onReset();
            Assert.True(resetter.isEnabled);
        }

        [UnityTest]
        public IEnumerator EventManagerTest_onAddAnnotation(){
            annotationPin.SetActive(false);
            var uiManager = eventListener.AddComponent<UIManager>();
            yield return new WaitUntil(()=> uiManager !=null);
            uiManager.annotationPin = annotationPin;
            eventManager.onAddAnnotations();
            Assert.True(annotationPin.activeInHierarchy);
        }
        
        [UnityTest]
        public IEnumerator EventManger_onEnableDicom(){
            var uiManager = eventListener.AddComponent<UIManager>();
            DICOMController.SetActive(false);
            yield return new WaitUntil(() => uiManager != null);
            eventManager.onEnableDicom();
            Assert.True(DICOMController.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onEnableUIBlocker(){
            var uiManager = eventListener.AddComponent<UIManager>();
            UIBlocker.SetActive(false);
            uiManager.UIBlocker = UIBlocker;
            yield return new WaitUntil(() => uiManager != null);
            eventManager.onEnableUIBlocker();
            Assert.True(UIBlocker.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onDisableUIBlocker(){
            var uiManager = eventListener.AddComponent<UIManager>();
            yield return new WaitUntil(() => uiManager != null);
            uiManager.UIBlocker = UIBlocker;
            UIBlocker.SetActive(true);
            eventManager.onDisableUIBlocker();
            Assert.False(UIBlocker.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onToggleColourPalette(){
            var uiManager = eventListener.AddComponent<UIManager>();
            yield return new WaitUntil(()=> uiManager != null);
            eventManager.onToggleColourPalette();
            Assert.False(colourPalette.activeInHierarchy);
            eventManager.onToggleColourPalette();
            Assert.True(colourPalette.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onToggleSegmentSelect(){
            var uiManager = eventListener.AddComponent<UIManager>();
            bool startState = segmentSelectButton.activeInHierarchy;
            yield return new WaitUntil(()=> uiManager != null);
            eventManager.onToggleSegmentSelect();
            Assert.False(segmentSelectButton.activeInHierarchy == startState);
        }
        
    
        // [UnityTest]
        // public IEnumerator EventManagerTest_onViewAnnotation(){
        //     initialiseTestScene();
        //     loadModel();
        //     dropdown.SetActive(false);
        //     var selector = eventListener.AddComponent<AnnotationSelector>();
        //     yield return new WaitUntil(()=> selector != null);
        //     eventManager.onViewAnnotations();
        //     Assert.True(dropdown.activeInHierarchy);
        // }
    }
}
