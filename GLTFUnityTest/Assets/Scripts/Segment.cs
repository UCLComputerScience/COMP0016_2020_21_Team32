using Siccity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//New class representing segments. Makes it easier to check if a segment is or isn't selected. Might be a better way of doing this, maybe with
//Events. Something to look into. 
public class Segment : MonoBehaviour
{
        public GameObject seg;
        public bool selected;

        public Segment(GameObject seg){
            this.seg = seg;
            this.selected = false;
        }
}
