using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using TMPro;

/*Dynamically caches all buttons and toggles that are children of the gameObject this script is attatched into an array, adding a toolTipShower component to each.
The text displayed by the tooltip is the same as the name of the gameObject that the button or toggle is attatched to, which means that any new buttons or
toggles that are added to the UI will automatically display a tooltip when the pointer hovers over it.
*/
public class ToolTipCreator : MonoBehaviour
{
    Canvas canvas;
    List<Selectable> selectables;
    void Awake(){
        canvas = GetComponent<Canvas>();
        selectables = canvas.GetComponentsInChildren<Selectable>(true).ToList();
        foreach(Selectable s in selectables){
            if(!(s is Button || s is Toggle)) continue;
            Debug.Log(s.name);
            s.gameObject.AddComponent<toolTipShower>();
        }
    }
    private class toolTipShower : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
        String prevText;
        /*Called when the pointer enters the rect transform of the button/toggle it is attatched to.
        Sets the text displayed in the tooltip to that of the name of the gameObject the button/toggle
        is a component of.
        */
        public void OnPointerEnter(PointerEventData data){
            //if(prevText == this.gameObject.name)return;
            ToolTip.current.gameObject.SetActive(true);
            ToolTip.SetToolTipText(this.gameObject.name);
            prevText = this.gameObject.name;
        }
        /*Called when the pointer is no longer within the rect transform of the button/toggle. Disables the tooltip*/
        public void OnPointerExit(PointerEventData data){
            ToolTip.current.gameObject.SetActive(false);
        }

    }
}

