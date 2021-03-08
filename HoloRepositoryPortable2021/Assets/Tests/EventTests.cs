using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.IO;

namespace Tests
{
    public class EventTests
    {



        [UnityTest]
        public IEnumerator EventManagerTest_onCameraEnabled()
        {
            GameObject publisher = new GameObject();
            GameObject model = new GameObject();
            GameObject eventListener = new GameObject();
            GameObject slider = new GameObject("Opacity Slider");
            GameObject plane = new GameObject("Plane");
            GameObject pivot = new GameObject("Pivot");
            GameObject camera = new GameObject("MainCamera");
            var cam = camera.AddComponent<Camera>();
            cam.tag = "MainCamera";
        

            slider.AddComponent<Slider>();
            
            var modelHandler = model.AddComponent<ModelHandler>();
            Organ organ = OrganFactory.GetOrgan(Application.streamingAssetsPath + Path.DirectorySeparatorChar+ "brain.glb");
            organ.initialiseModel(model);
            ModelHandler.organ = organ;
            
            var cameraController = eventListener.AddComponent<CameraController>();
            cameraController.pivot = pivot;
            cameraController.isEnabled = false;

            var eventManager = publisher.AddComponent<EventManager>();
            eventManager.onEnableCamera();
            

            yield return new WaitForSeconds(1f);

            Assert.True(cameraController.isEnabled);
            
        }
    }
}
