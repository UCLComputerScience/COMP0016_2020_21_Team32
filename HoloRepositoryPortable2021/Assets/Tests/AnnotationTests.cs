using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using TMPro;
using System;
namespace Tests{
    public class AnnotationTests : MonoBehaviour
    {
        AnnotationData data;
        [SetUp]
        public void initialiseAnnotation(){
            data = new AnnotationData();
            data.cameraCoordinates = Vector3.zero;
            data.cameraDisplacement = Vector3.zero;
            data.cameraRotation = Quaternion.identity;
            data.text = "";
            data.title = "";
            data.colours = new List<Color>();
            data.screenDimensions = new Vector2(Screen.width, Screen.height);
            data.planeNormal = Vector3.zero;
            data.planePosition = Vector3.zero;
        }

        [Test]
        public void parseAnnotationToAndFromJsonNoErrors(){
            string path = Path.GetTempFileName();
            FileInfo file = new FileInfo(path);
            string json = JsonUtility.ToJson(data);
            Debug.Log(json);
            File.WriteAllText(path, json);
            string jsonAgain = File.ReadAllText(path);
            AnnotationData dataAgain = JsonUtility.FromJson<AnnotationData>(jsonAgain);
            Assert.AreEqual(data.cameraDisplacement, dataAgain.cameraDisplacement);
            Assert.AreEqual(data.cameraCoordinates, dataAgain.cameraCoordinates);
            Assert.AreEqual(data.cameraRotation, dataAgain.cameraRotation);
            Assert.AreEqual(data.text, dataAgain.text);
            Assert.AreEqual(data.title, dataAgain.title);
            Assert.AreEqual(data.screenDimensions, dataAgain.screenDimensions);
            Assert.AreEqual(data.planeNormal, dataAgain.planeNormal);
            Assert.AreEqual(data.planePosition, dataAgain.planePosition);
        }
        [Test]
        public void parseInvalidJsonToAnnotation(){
            string path = Path.GetTempFileName();
            FileInfo file = new FileInfo(path);
            string json = "balls";
            try{
                AnnotationData data = JsonUtility.FromJson<AnnotationData>(json);
            }catch(Exception e){
                Assert.AreEqual(e.Message, "JSON parse error: Invalid value.");
            }    
        }
        [Test]
        public void parseValidButNotAnnotationJsonToAnnotation(){
            string json = "{\"firstName\": \"marvin\"}";
            try{
                AnnotationData data = JsonUtility.FromJson<AnnotationData>(json);
            }catch(Exception e){
                Assert.AreEqual(e.Message, "JSON parse error: Invalid value.");
            }
        }

    }
    
}
