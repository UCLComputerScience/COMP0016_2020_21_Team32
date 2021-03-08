using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

///<summary>This class is attached to, and defines the behaviour of, the tooltip, which hovers above the pointer whenever it is active.
///There is only one tooltip in the scene at any one time, but its position and text varies depending on the position of the pointer</summary>
public class ToolTip : MonoBehaviour
{   
    
    private RectTransform canvasRect;
    private RectTransform rect;
    private RectTransform background;
    private TMP_Text text;
    private int textPadding;
    public static ToolTip current;
    void Awake(){
        if(current != null && current != this){
            Destroy(this.gameObject);
        }else{
            current = this;
        }
        rect = this.GetComponent<RectTransform>();
        canvasRect = this.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        background = this.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        text = this.GetComponentInChildren<TMP_Text>();
        textPadding = 10;
        this.gameObject.SetActive(false);
    }
    private void setText(string textToSet){
        text.SetText(textToSet);
        text.ForceMeshUpdate(); //forces the text to update in the same frame as this function is called
        Vector2 textSize = text.GetRenderedValues();
        Vector2 padding = textSize/textPadding; 
        background.sizeDelta = textSize + padding; //makes sure the text does not leave the bounds of the box
    }
    public static void SetToolTipText(string textToSet){
        current.setText(textToSet);
    }
    
    //called every frame. Whenever the tooltip is active, it will hover just above the pointer. 
    void LateUpdate(){
        Vector2 anchoredPos = Input.mousePosition / canvasRect.transform.localScale.x;
        if(anchoredPos.x + background.rect.width > canvasRect.rect.width){ //ensures the tooltip cannot go beyond the bounds of the UI to the right
            anchoredPos.x = canvasRect.rect.width - background.rect.width;
        }
        if(anchoredPos.y + background.rect.height > canvasRect.rect.height){ //ensures the tooltip cannot go beyond the bounds of the UI to the top.
            anchoredPos.y = canvasRect.rect.height - background.rect.height;
        }
        rect.anchoredPosition = anchoredPos;
        
    }
}
