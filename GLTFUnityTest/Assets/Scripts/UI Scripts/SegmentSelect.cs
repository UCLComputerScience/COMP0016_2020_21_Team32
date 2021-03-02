using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        if(numSegments == 0){
            numSegments = ModelHandler.organ.segments.Count;
            for(int i = 1; i <= numSegments; i++)segments.Add("Segment" + " " + i);
        }
        if(curSeg == segments.Count -1)curSeg = -1;
        text.text = segments[++curSeg];
    }
}
