using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using System;
using System.IO;
using UnityEditor;

///<summary>This class uses the factory design pattern to instantiate the appopriate concrete organ at runtime based on the filename
///received. 
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
            case "eye segment.glb":
                return new EyeSegmentExample(FileHelper.currentModelFileName);
            default:
                return new LoadedOrgan(FileHelper.currentModelFileName);
        }
    }
}
