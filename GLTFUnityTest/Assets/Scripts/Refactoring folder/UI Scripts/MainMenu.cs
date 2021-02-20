using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Globalization;

/*
Class provides interactivity to the buttons in the main menu.
*/
public class MainMenu : MonoBehaviour
{
    private List<string> exampleFiles; 

    private TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
    List<string> options;
    [SerializeField] TMP_Dropdown exampleButtons;

    [SerializeField] Button viewButton;

    void Awake(){
        exampleButtons.ClearOptions();
        exampleButtons.onValueChanged.AddListener(OnValueChanged);
        viewButton.onClick.AddListener(nextScene);
        getAllFiles();
    }
    public void nextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void prevScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    public void OnValueChanged(int index){
        ModelHandler.fileName = exampleFiles[index];
    }

    public void quitApplication(){
        Application.Quit();
    }


/*Gets all example models from the streaming Assets folder and converts the filenames to a dropdown list.*/
    private void getAllFiles(){
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
}
