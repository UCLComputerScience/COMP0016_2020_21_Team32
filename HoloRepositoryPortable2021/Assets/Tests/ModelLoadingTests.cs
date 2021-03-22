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
    public class ModelLoadingTests 
    {
        private GameObject model;
        private GameObject eventManager;
        ModelHandler modelHandler;

        [SetUp]
        public void setUp(){
            model = new GameObject();
            eventManager = new GameObject();
            eventManager.AddComponent<EventManager>();
            model.SetActive(false);
            var modelHandler = model.AddComponent<ModelHandler>();
            modelHandler.plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        }
        [TearDown]
        public void tearDown(){
            Object.Destroy(model);
            Object.Destroy(eventManager);
        }


        [UnityTest]
        public IEnumerator loadBrain_checkCorrectNumberOfSegments(){
            FileHelper.setCurrentModelFileName(Path.Combine(Application.streamingAssetsPath, "brain.glb"));
            model.SetActive(true); 
            yield return new WaitUntil(() => ModelHandler.current.segments != null);
            Debug.Log(ModelHandler.current.modelRadius);
            Assert.AreEqual(5, ModelHandler.current.segments.Count);
        }
        [UnityTest]
        public IEnumerator loadBone_checkCorrectNumberOfSegments(){
            FileHelper.setCurrentModelFileName(Path.Combine(Application.streamingAssetsPath, "bone.glb"));
            model.SetActive(true);
            yield return new WaitUntil(() => ModelHandler.current.segments != null);
            Assert.AreEqual(1, ModelHandler.current.segments.Count);
        }

        [UnityTest]
        public IEnumerator loadKidney_checkCorrectNumberOfSegments(){
            FileHelper.setCurrentModelFileName(Path.Combine(Application.streamingAssetsPath, "kidney.glb"));
            model.SetActive(true);
            yield return new WaitUntil(() => ModelHandler.current.segments != null);
            Assert.AreEqual(2, ModelHandler.current.segments.Count);
        }

        [UnityTest]
        public IEnumerator loadLung_checkCorrectNumberOfSegments(){
            FileHelper.setCurrentModelFileName(Path.Combine(Application.streamingAssetsPath, "lung.glb"));
            model.SetActive(true);
            yield return new WaitUntil(() => ModelHandler.current.segments != null);
            Assert.AreEqual(2, ModelHandler.current.segments.Count);
        }

        [UnityTest]
        public IEnumerator loadAbdomen_checkCorrectNumberOfSegments(){
            FileHelper.setCurrentModelFileName(Path.Combine(Application.streamingAssetsPath, "abdomen.glb"));
            model.SetActive(true);
            yield return new WaitUntil(() => ModelHandler.current.segments != null);
            Assert.AreEqual(8, ModelHandler.current.segments.Count);
        }

    }
}

