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
    GameObject male;

    void Start()
    {
        male = Siccity.GLTFUtility.Importer.LoadFromFile(man);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
