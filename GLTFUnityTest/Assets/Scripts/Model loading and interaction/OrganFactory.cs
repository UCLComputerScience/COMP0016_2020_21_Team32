using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using System;
using System.IO;

public static class OrganFactory
{
    public static Organ GetOrgan(string filepath){
        Debug.Log("HERE'S YA MATHAFACKIN FILEPAF "+ filepath);
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
