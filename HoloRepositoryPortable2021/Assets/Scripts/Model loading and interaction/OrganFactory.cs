using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using System;
using System.IO;

///<summary>This class uses the factory design pattern to instantiate the appopriate concrete organ at runtime based on the filename
///received. The loaded model itself is passed as a parameter to the newly instantiated organ. If the factory does not recognise the 
///filename as one of the example model filenames, a LoadedOrgan object will be instantiated, which is the standard wrapper class for the loaded organ.
///Here, we make a call to the GLTFUtility.Importer API, which converts a glb/gltf file into a Unity game object (unity does not natively support glb/gltf files)</summary>
public static class OrganFactory
{
    public static Organ GetOrgan(string filepath){
        String exampleModelPath = Path.Combine(Application.streamingAssetsPath, filepath);
        switch(filepath){
            case "brain.glb":
                return new BrainExample(Siccity.GLTFUtility.Importer.LoadFromFile(exampleModelPath)); 
            case "abdomen.glb":
                return new AbdomenExample(Siccity.GLTFUtility.Importer.LoadFromFile(exampleModelPath));
            case "bone.glb":
                return new BoneExample(Siccity.GLTFUtility.Importer.LoadFromFile(exampleModelPath));
            case "lung.glb":
                return new LungExample(Siccity.GLTFUtility.Importer.LoadFromFile(exampleModelPath));
            case "kidney.glb":
                return new KidneyExample(Siccity.GLTFUtility.Importer.LoadFromFile(exampleModelPath));
            default:
                return new LoadedOrgan(Siccity.GLTFUtility.Importer.LoadFromFile(filepath));
        }
    }
}
