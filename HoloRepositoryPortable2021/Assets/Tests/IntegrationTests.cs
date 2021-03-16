using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Tests{
    public class IntegrationTests{
        [UnitySetUp]
        public IEnumerator setUp(){
            SceneManager.LoadScene(1);
            yield return new EnterPlayMode();
        }

        // [UnityTearDown]
        // public IEnumerator tearDown(){
        //     Object.Destroy(modelHandler);
        //     Object.Destroy(eventManager);
        //     Object.Destroy(eventListener);
        //     yield return new ExitPlayMode();
        // }

        [UnityTest]
        public IEnumerator sceneLoaded(){
            yield return new WaitForEndOfFrame();
            Debug.Log(SceneManager.GetActiveScene().name);
            Assert.True(SceneManager.GetActiveScene().name == "Main Page");
        }
    }
}