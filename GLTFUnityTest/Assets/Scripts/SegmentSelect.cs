using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SegmentSelect : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    List<string> segments = new List<string>(){"Segment1", "Segment2","Segment3","Segment4"}; 
    int curSeg = 0;

    public void setText(){
        if(curSeg == segments.Count -1)curSeg = -1;
        text.text = segments[++curSeg];
    }
}
