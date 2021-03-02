using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SFB;
using System.IO;
using System.Globalization;

///<summary>
///This class is attatched to the MainMenu gameobject and provides the elements on it with interactivity by passing callback actions
///to their events.
///</summary>
public class MainMenu : MonoBehaviour
{
    private FileHelper fileHelper;
    private List<string> exampleFiles; 
    private List<string> options;
    private TMP_Dropdown exampleOrganDropDown;
    private Button viewButton;
    private Button quitButton;
    private Button fileExplorer;
    private TMP_Text chosenPath;

    void Awake(){
        /*Initialise all interactable elements*/
        exampleOrganDropDown = this.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
        viewButton = this.transform.Find("View").GetComponent<Button>();
        quitButton = this.transform.Find("Quit").GetComponent<Button>();
        fileExplorer = this.transform.Find("File Explorer").GetComponent<Button>();
        chosenPath = this.transform.Find("Chosen Path").GetComponent<TMP_Text>();

        /*Get all example model files in the streaming assets folder and add them as options to the drop down menu*/
        fileHelper = new FileHelper(Application.streamingAssetsPath);
        exampleFiles = fileHelper.getPathsInDir("*.glb", true);
        options = fileHelper.getRelativePathsNoExtensions("*.glb");
        exampleOrganDropDown.ClearOptions();
        exampleOrganDropDown.AddOptions(options);
        FileHelper.setCurrentModelFileName(exampleFiles[0]); 

        /*Add callback actions to the interactable elements*/
        exampleOrganDropDown.onValueChanged.AddListener(OnValueChanged);
        viewButton.onClick.AddListener(nextScene);
        quitButton.onClick.AddListener(quitApplication);
        fileExplorer.onClick.AddListener(openFileExplorer);
    }
    public void nextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void prevScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }
    public void OnValueChanged(int index){
       FileHelper.setCurrentModelFileName(exampleFiles[index]);
    }
    public void quitApplication(){
        Application.Quit();
    }
    public void openFileExplorer(){
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a glb/gltf file", "", "", false);
        if(paths.Length == 0) return;
        FileHelper.setCurrentModelFileName(paths[0]);
        chosenPath.gameObject.SetActive(true);
        chosenPath.text = "Loaded: "+paths[0];
    }
}
