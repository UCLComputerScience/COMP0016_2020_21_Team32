using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using SFB;
using System.IO;
using System.Globalization;
using UnityEditor;

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
    private Button controlButton;
    private GameObject controls;
    private Button minimiseControls;

    void Awake(){
        /*Initialise all interactable elements*/
        exampleOrganDropDown = this.transform.Find("Dropdown").GetComponent<TMP_Dropdown>();
        viewButton = this.transform.Find("View").GetComponent<Button>();
        quitButton = this.transform.Find("Quit").GetComponent<Button>();
        fileExplorer = this.transform.Find("File Explorer").GetComponent<Button>();
        chosenPath = this.transform.Find("Chosen Path").GetComponent<TMP_Text>();
        controlButton = this.transform.Find("Control Button").GetComponent<Button>();
        controls = this.transform.Find("Controls").gameObject;
        minimiseControls = controls.transform.Find("Minimise").GetComponent<Button>();

        /*Get all example model files in the streaming assets folder and add them as options to the drop down menu*/
        fileHelper = new FileHelper(Application.streamingAssetsPath);
        exampleFiles = fileHelper.getPathsInDir("*.glb", false);
        options = fileHelper.getRelativePathsNoExtensions("*.glb");
        exampleOrganDropDown.ClearOptions();
        exampleOrganDropDown.AddOptions(options);
        FileHelper.setCurrentModelFileName(exampleFiles[0]); 

        /*Add callback actions to the interactable elements*/
        exampleOrganDropDown.onValueChanged.AddListener(OnValueChanged);
        viewButton.onClick.AddListener(nextScene);
        quitButton.onClick.AddListener(quitApplication);
        fileExplorer.onClick.AddListener(openFileExplorer);
        controlButton.onClick.AddListener(openControls);
        minimiseControls.onClick.AddListener(closeControls);
    }
    private void nextScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    private void OnValueChanged(int index){
       FileHelper.setCurrentModelFileName(exampleFiles[index]);
    }
    private void quitApplication(){
        Application.Quit();
    }
    private void openFileExplorer(){
        var extension = new [] {new ExtensionFilter("3D model files", "gltf", "glb")};
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select a glb/gltf file", "", extension, false);
        if(paths.Length == 0) return;
        FileHelper.setCurrentModelFileName(paths[0]);
        chosenPath.gameObject.SetActive(true);
        chosenPath.text = "Loaded: "+paths[0];
    }
    private void openControls(){
        controls.SetActive(true);
    }
    private void closeControls(){
        controls.SetActive(false);
    }
}
