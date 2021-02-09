using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ReturnToStartPage : MonoBehaviour
{
    Button quitButton;
    void Awake(){
        quitButton = GetComponent<Button>();
        quitButton.onClick.AddListener(returnToStartPage);
    }
    public void returnToStartPage(){
        SceneManager.LoadScene(0);
    }
}
