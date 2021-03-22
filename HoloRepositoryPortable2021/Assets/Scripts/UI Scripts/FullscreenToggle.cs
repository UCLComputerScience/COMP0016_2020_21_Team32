using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///<summary>Attached to the fullscreen toggle. Changes the image displayed when the toggle is clicked.</summary>
public class FullscreenToggle : MonoBehaviour
{
    private GameObject onImage;
    private GameObject offImage;
    Toggle toggle;
    void Awake(){
        onImage = transform.Find("isOn").gameObject;
        onImage.gameObject.SetActive(true);
        offImage = transform.Find("isOff").gameObject;
        offImage.gameObject.SetActive(false);
        toggle = this.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(onClicked);
    }
    /*Fires an event to toggle the visibility of all UI elements that are children of the mainpage gameobject*/
    private void onClicked(bool isOn){
        EventManager.current.onToggleFullScreen();
        onImage.SetActive(!onImage.activeInHierarchy);
        offImage.gameObject.SetActive(!offImage.activeInHierarchy);
    }

}
