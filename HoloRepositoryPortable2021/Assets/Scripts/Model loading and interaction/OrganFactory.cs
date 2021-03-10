using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using System;
using System.IO;
using UnityEditor;

///<summary>This class uses the factory design pattern to instantiate the appopriate concrete organ at runtime based on the filename
///received. The loaded model itself is passed as a parameter to the newly instantiated organ. If the factory does not recognise the 
///filename as one of the example model filenames, a LoadedOrgan object will be instantiated, which is the standard wrapper class for the loaded organ.
///Here, we make a call to the Siccity.GLTFUtility.Importer API, which converts a glb/gltf file into a Unity game object (unity does not natively support glb/gltf files)</summary>
public static class OrganFactory
{
    public static Organ GetOrgan(string filepath, GameObject model){
        switch(filepath){
            case "brain.glb":
                return new BrainExample(model); 
            case "abdomen.glb":
                return new AbdomenExample(model);
            case "bone.glb":
                return new BoneExample(model);
            case "lung.glb":
                return new LungExample(model);
            case "kidney.glb":
                return new KidneyExample(model);
            default:
                return new LoadedOrgan(model);
        }
    }


    // private static IEnumerator waitForOrgan(string filepath){
    //     yield return new WaitUntil(() => loadedModel != null);
    //     String exampleModelPath = Path.Combine(Application.streamingAssetsPath, filepath);
    //     switch(filepath){
    //         case "brain.glb":
    //             ImportGLTFAsync(exampleModelPath);
    //             yield return new WaitUntil(()=>loadedModel != null);
    //             organ = new BrainExample(loadedModel); 
    //             break;
    //         case "abdomen.glb":
    //             ImportGLTFAsync(exampleModelPath);
    //             yield return new WaitUntil(()=>loadedModel != null);
    //             organ = new AbdomenExample(loadedModel);
    //             break;
    //         case "bone.glb":
    //             ImportGLTFAsync(exampleModelPath);
    //             yield return new WaitUntil(()=>loadedModel != null);
    //             organ = new BoneExample(loadedModel);
    //             break;
    //         case "lung.glb":
    //             ImportGLTFAsync(exampleModelPath);
    //             yield return new WaitUntil(()=>loadedModel != null);
    //             organ = new LungExample(loadedModel);
    //             break;
    //         case "kidney.glb":
    //             ImportGLTFAsync(exampleModelPath);
    //             yield return new WaitUntil(()=>loadedModel != null);
    //             organ = new KidneyExample(loadedModel);
    //             break;
    //         default:
    //             ImportGLTFAsync(filepath);
    //             yield return new WaitUntil(()=>loadedModel !=null);
    //             organ =  new LoadedOrgan(loadedModel);
    //             break;
    //     }
    // }  


}
