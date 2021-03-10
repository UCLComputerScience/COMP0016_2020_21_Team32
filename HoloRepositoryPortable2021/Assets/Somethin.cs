using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using SFB;
using System;

public class Somethin : MonoBehaviour
{
    public delegate void action(GameObject game, AnimationClip[] clips);
    // Start is called before the first frame update
    // void Start()
    // {
    //     //public delegate void Action<in T1,in T2>(T1 arg1, T2 arg2);
    //     public delegate void action(GameObject game, AnimationClip[] clips);
    //     action model;
    //     string[] paths = StandaloneFileBrowser.OpenFilePanel("Yes","","",false);
    // }
    void Start(){
        System.Action<UnityEngine.GameObject, UnityEngine.AnimationClip[]> model = null;
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Yes", "", "", false);
        Siccity.GLTFUtility.Importer.LoadFromFileAsync(paths[0], new ImportSettings(), model);
    }
}
