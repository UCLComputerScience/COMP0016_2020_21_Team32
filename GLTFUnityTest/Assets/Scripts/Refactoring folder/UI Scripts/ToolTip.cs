using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
Class that defines a tooltip. 
*/
public class ToolTip : MonoBehaviour
{   
    
    private RectTransform canvasRect;
    private RectTransform rect;
    private RectTransform background;
    private TMP_Text text;
    public static ToolTip current;
    void Awake(){
        current = this;
        rect = this.GetComponent<RectTransform>();
        canvasRect = this.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        background = this.GetComponentInChildren<Image>().GetComponent<RectTransform>();
        text = this.GetComponentInChildren<TMP_Text>();
        this.gameObject.SetActive(false);
    }
    private void setText(string textToSet){
        text.SetText(textToSet);
        text.ForceMeshUpdate();
        Vector2 textSize = text.GetRenderedValues();
        Vector2 padding = textSize/10;
        background.sizeDelta = textSize + padding;
    }
    public static void SetToolTipText(string textToSet){
        current.setText(textToSet);
    }
    void LateUpdate(){
        Vector2 anchoredPos = Input.mousePosition / canvasRect.transform.localScale.x;
        if(anchoredPos.x + background.rect.width > canvasRect.rect.width){
            anchoredPos.x = canvasRect.rect.width - background.rect.width;
        }
        if(anchoredPos.y + background.rect.height > canvasRect.rect.height){
            anchoredPos.y = canvasRect.rect.height - background.rect.height;
        }
        rect.anchoredPosition = anchoredPos;
        
    }
}
