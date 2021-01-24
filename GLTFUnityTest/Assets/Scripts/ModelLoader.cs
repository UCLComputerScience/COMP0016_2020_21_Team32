using Siccity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
//loads the model at runtime

public class ModelLoader : MonoBehaviour
{
    string curDir = Directory.GetCurrentDirectory();
    string man = "Assets\\SampleGLTF\\Man\\source\\dAb20200729.gltf";

    string brian = "Assets\\SampleGLTF\\brain.glb";
    GameObject male;
    GameObject brain;


    void Start()
    {
        //male = Siccity.GLTFUtility.Importer.LoadFromFile(man);
        brain = Siccity.GLTFUtility.Importer.LoadFromFile(brian);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}