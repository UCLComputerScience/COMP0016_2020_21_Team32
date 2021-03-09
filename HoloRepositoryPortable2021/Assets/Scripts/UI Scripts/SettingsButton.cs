using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{
    private Button button;
    void Awake(){
        button = this.GetComponent<Button>();
        button.onClick.AddListener(onClicked);
    }
    public void onClicked(){
        EventManager.current.onChangeSettings();
    }
}
