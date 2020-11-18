using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrainButton : MonoBehaviour
{
    public void loadBrain(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
