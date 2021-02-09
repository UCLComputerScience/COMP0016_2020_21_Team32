using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Siccity.GLTFUtility;
using System;
using System.IO;

public static class OrganFactory
{
    public static Organ GetOrgan(string filepath){
        GameObject loadedModel = Siccity.GLTFUtility.Importer.LoadFromFile(Path.Combine(Application.streamingAssetsPath, filepath));
        switch(filepath){
            case "brain.glb":
                return new BrainExample(loadedModel);
            case "abdomen.glb":
                return new AbdomenExample(loadedModel);
            case "bone.glb":
                return new BoneExample(loadedModel);
            case "lung.glb":
                return new LungExample(loadedModel);
            case "kidney.glb":
                return new KidneyExample(loadedModel);
            default:
                return null;//StandardOrgan(Siccity.GLTFUtility.Importer.LoadFromFile(filepath));
        }
    }
}
