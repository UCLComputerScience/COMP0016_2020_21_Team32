using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;
using System.IO;
using System.Linq;
using System.Globalization;

public class FileManager : MonoBehaviour
{
    private Button fileExplorer;
    [SerializeField ]private TMP_Text chosenPath;
    void Awake(){
        this.fileExplorer = GetComponent<Button>();
        fileExplorer.onClick.AddListener(openFileExplorer);
    }
    
    public void openFileExplorer()
    {
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a glb/gltf file", "", "", false);
        if(paths.Length == 0)return;
        chosenPath.gameObject.SetActive(true);
        chosenPath.text = "Loaded: "+paths[0];
    }
}