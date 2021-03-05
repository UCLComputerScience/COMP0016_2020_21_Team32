using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

<<<<<<< HEAD
///<summary>Class is attached to the segment select button.
///When the user presses the button, the currently selected button changes.</summary>
public class SegmentSelect : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
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
=======
public class SegmentSelect : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    List<string> segments = new List<string>();
    int curSeg = 0;

    private int numSegments;
    Organ organ;

    void Start(){

    }

    public void setText(){
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
        if(numSegments == 0){
            numSegments = ModelHandler.organ.segments.Count;
            for(int i = 1; i <= numSegments; i++)segments.Add("Segment" + " " + i);
        }
<<<<<<< HEAD
        if(currentSegment == segments.Count -1)currentSegment = -1;
        text.text = segments[++currentSegment];
        ModelHandler.current.selectSegment();
=======
        if(curSeg == segments.Count -1)curSeg = -1;
        text.text = segments[++curSeg];
>>>>>>> 82abfe8fcbdad9d7e902c5387123d5828c2ba7d3
    }
}
