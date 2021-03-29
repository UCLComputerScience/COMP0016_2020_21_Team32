using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using TMPro;

namespace Tests
{
    /*Tests checking that all Events trigger the correct responses in the GUI*/
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
        private AnnotationData exampleAnnotation;


        private void initialiseTestScene(){
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
            eventListener.AddComponent<EventSystem>();
            eventManager = publisher.GetComponent<EventManager>();

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
            foreach(GameObject g in Object.FindObjectsOfType<GameObject>()){
                Object.Destroy(g);
            }
            yield return new ExitPlayMode();
        }


        [UnityTest]
        public IEnumerator EventManagerTest_onCameraEnabled()
        {
            var cameraController = eventListener.AddComponent<CameraController>();
            cameraController.pivot = pivot;
            cameraController.isEnabled = false;
            eventListener.SetActive(true);
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
        public IEnumerator EventManager_onEnableDicom(){
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
        public IEnumerator EventManager_onEnablePivot(){
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
        public IEnumerator EventManager_onEnableCrossSection(){
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
        public IEnumerator EventManager_onDisableLoadingScreen(){
            var uiManager = eventListener.AddComponent<UIManager>();
            var loadingScreen = new GameObject();
            uiManager.loadingScreen = loadingScreen;
            loadingScreen.SetActive(true);
            eventListener.SetActive(true);
            yield return null;
            Assert.True(loadingScreen.activeInHierarchy);
            eventManager.onModelLoaded();
            Assert.False(loadingScreen.activeInHierarchy);
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
            yield return null;
            eventManager.onChangeSettings();
            Assert.True(settingsController.activeInHierarchy);
        }
        [UnityTest]
        public IEnumerator EventManager_onZoomIn(){
            var cameraController = eventListener.AddComponent<CameraController>();
            cameraController.pivot = pivot;
            eventListener.SetActive(true);
            yield return new WaitUntil(() => Camera.main.transform.position != Vector3.zero);
            Vector3 initialCamPos = Camera.main.transform.position;
            eventManager.onZoomIn();
            yield return new WaitForEndOfFrame();
            Vector3 finalCamPos = Camera.main.transform.position;
            Assert.Less(initialCamPos.z, finalCamPos.z);
        }
        [UnityTest]
        public IEnumerator EventManager_onZoomOut(){
            var cameraController = eventListener.AddComponent<CameraController>();
            cameraController.pivot = pivot;
            eventListener.SetActive(true);
            yield return new WaitUntil(() => Camera.main.transform.position != Vector3.zero);
            Vector3 initialCamPos = Camera.main.transform.position;
            eventManager.onZoomOut();
            yield return new WaitForEndOfFrame();
            Vector3 finalCamPos = Camera.main.transform.position;
            Assert.Greater(initialCamPos.z, finalCamPos.z);
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
