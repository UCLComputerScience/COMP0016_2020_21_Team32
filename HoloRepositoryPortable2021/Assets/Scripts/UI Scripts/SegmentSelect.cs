using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

///<summary>Class is attached to the segment select button.
///When the user presses the button, the currently selected button changes.</summary>
public class SegmentSelect : MonoBehaviour
{
    private TMP_Text text;
    List<string> segments;
    int currentSegment;
    Button segmentSelect;
    private int numSegments;
    void Awake(){
        /*initialise variables*/
        segments = new List<string>();
        currentSegment = 0;
        segmentSelect = this.GetComponent<Button>();
        text = this.GetComponentInChildren<TMP_Text>();

        /*pass callback to the onClick event of the button*/
        segmentSelect.onClick.AddListener(selectSegment);
    }    

    /*Changes the text displayed on the button, and call the selectSegment method defined by ModelHandler so that any interactions the user
    has with the model (ie: changing colour, changing opacity) is applied to the intended segment */
     public void selectSegment(){
        if(numSegments == 0){
            numSegments = ModelHandler.current.segments.Count;
            for(int i = 1; i <= numSegments; i++)segments.Add("Segment" + " " + i);
        }
        if(currentSegment == segments.Count -1)currentSegment = -1;
        text.text = segments[++currentSegment];
        EventManager.current.onSegmentSelect();
    }
}
