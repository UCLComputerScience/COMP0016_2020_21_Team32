using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using TMPro;

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
        ModelHandler.fileName = paths[0];
        chosenPath.gameObject.SetActive(true);
        chosenPath.text = "Loaded: "+paths[0];
    }
}
