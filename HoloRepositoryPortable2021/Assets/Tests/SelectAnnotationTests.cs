using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using TMPro;

namespace Tests{
    public class SelectAnnotationTests
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
            eventListener.AddComponent<EventSystem>();
            eventManager = publisher.GetComponent<EventManager>();
            Debug.Log(eventManager);

        }
        private void initialiseRandomAnnotation(){
            exampleAnnotation = new AnnotationData();
            exampleAnnotation.cameraCoordinates = new Vector3(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
            exampleAnnotation.cameraRotation = Quaternion.Euler(Random.Range(0, 180),Random.Range(0, 180),Random.Range(0, 180));
            exampleAnnotation.cameraDisplacement = new Vector3(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
            exampleAnnotation.text = "random";
            exampleAnnotation.title = "randomTitle";
            exampleAnnotation.planeNormal = new Vector3(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
            exampleAnnotation.planePosition = new Vector3(Random.Range(0, 500), Random.Range(0, 500), Random.Range(0, 500));
            exampleAnnotation.colours = new List<Color>(){Color.red, Color.green, Color.blue, Color.black, Color.white};
            exampleAnnotation.screenDimensions = new Vector2(Screen.width, Screen.height);
        }
        
        private void loadModel(){
            modelHandler = eventListener.AddComponent<ModelHandler>();
            modelHandler.plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        }

        [UnityTest]
        public IEnumerator EventManagerTest_onSelectAnnotation_CameraPositionUpdate(){
            initialiseRandomAnnotation();
            var cameraController = eventListener.AddComponent<CameraController>();
            cameraController.pivot = pivot;
            eventListener.SetActive(true);
            yield return new WaitUntil(() => ModelHandler.current.modelRadius != 0); //wait for model to be loaded
            eventManager.onSelectAnnotation(exampleAnnotation);
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(exampleAnnotation.cameraCoordinates, exampleAnnotation.cameraCoordinates);
            Assert.AreEqual(exampleAnnotation.cameraRotation, exampleAnnotation.cameraRotation);
            Assert.AreEqual(exampleAnnotation.cameraDisplacement, exampleAnnotation.cameraDisplacement);
        }
    }
}
