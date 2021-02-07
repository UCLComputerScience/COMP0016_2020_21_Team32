using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Globalization;

public class MainMenu : MonoBehaviour
{
    List<string> exampleFiles; 

    TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
    List<string> options;
    [SerializeField] TMP_Dropdown exampleButtons;

    [SerializeField] Button viewButton;

    void Awake(){
        exampleButtons.ClearOptions();
        exampleButtons.onValueChanged.AddListener(OnValueChanged);
        viewButton.onClick.AddListener(playgame);

        exampleFiles = new List<string>();
        options = new List<string>();

        string path = Application.streamingAssetsPath;
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.glb");

        foreach (FileInfo f in info){
            Debug.Log("These are the file names");
            exampleFiles.Add(f.Name);
            options.Add(ti.ToTitleCase(f.Name.Substring(0,f.Name.IndexOf("."))));
        }
        exampleButtons.AddOptions(options);
        ModelHandler.fileName = exampleFiles[0];
    }
    public void playgame(){
        SceneManager.LoadScene("MainPage");
    }

    public void OnValueChanged(int index){
        ModelHandler.fileName = exampleFiles[index];
    }
}
