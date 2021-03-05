using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

<<<<<<< HEAD
///<summary>Helper script that enables one of two images for a toggle. If the isOn variable is true then onImage is displayed,
///otherwise offImage is displayed</summary>
=======
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
public class ChangeToggleImage : MonoBehaviour
{
    private GameObject onImage;
    private GameObject offImage;
    Toggle toggle;
<<<<<<< HEAD
=======

    // Update is called once per frame
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    void Awake(){
        onImage = transform.Find("isOn").gameObject;
        onImage.gameObject.SetActive(true);
        offImage = transform.Find("isOff").gameObject;
        offImage.gameObject.SetActive(false);
        toggle = this.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(swapImages);
    }
    private void swapImages(bool isOn){
        onImage.SetActive(!onImage.activeInHierarchy);
        offImage.gameObject.SetActive(!offImage.activeInHierarchy);
    }

}
