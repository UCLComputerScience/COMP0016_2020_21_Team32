using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using System;
using System.IO;
using UnityEditor;

///<summary>This class uses the factory design pattern to instantiate the appopriate concrete organ at runtime based on the filename
///received. The loaded model itself is passed as a parameter to the newly instantiated organ. If the factory does not recognise the 
///filename as one of the example model filenames, a LoadedOrgan object will be instantiated, which is the standard wrapper class for the loaded organ.</summary>
public static class OrganFactory
{
    public static Organ GetOrgan(){
        switch(FileHelper.currentAnnotationFolder){
            case "brain.glb":
                return new BrainExample(FileHelper.currentModelFileName); 
            case "abdomen.glb":
                return new AbdomenExample(FileHelper.currentModelFileName);
            case "bone.glb":
                return new BoneExample(FileHelper.currentModelFileName);
            case "lung.glb":
                return new LungExample(FileHelper.currentModelFileName);
            case "kidney.glb":
                return new KidneyExample(FileHelper.currentModelFileName);
            default:
                return new LoadedOrgan(FileHelper.currentModelFileName);
        }
    }
}
