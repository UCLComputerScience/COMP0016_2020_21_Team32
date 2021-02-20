
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public static EventManager current; //singleton pattern.
    public event EventHandler OnEnableCamera;
    public event EventHandler OnEnablePivot;
    public event EventHandler OnEnableCrossSection;
    public event EventHandler OnEnableDicom;
    public event EventHandler OnReset;
    public event EventHandler OnViewAnnotations;
    public event EventHandler OnAddAnnotations;
    public event EventHandler<EventArgsColourData> OnColourSelect;

    private void Awake(){
        current = this;
    }

    public void onEnableCamera(){
        OnEnableCamera?.Invoke(this, EventArgs.Empty);
    }
    public void onEnablePivot(){
        OnEnablePivot?.Invoke(this, EventArgs.Empty);
    }
    public void onEnableCrossSection(){
        OnEnableCrossSection?.Invoke(this, EventArgs.Empty);
    }
    public void onEnableDicom(){
        OnEnableDicom?.Invoke(this, EventArgs.Empty);
    }
    public void onReset(){
        OnReset?.Invoke(this, EventArgs.Empty);
    }
    public void onViewAnnotations(){
        OnViewAnnotations?.Invoke(this, EventArgs.Empty);
    }
    public void onAddAnnotations(){
        OnAddAnnotations?.Invoke(this, EventArgs.Empty);
    }
    public void onColourSelect(Color col){
        EventArgsColourData e = new EventArgsColourData(col);
        OnColourSelect?.Invoke(this, e);
    }
}
public class EventArgsColourData : EventArgs{
    public Color col;
    public EventArgsColourData(Color col){
        this.col = col;
    }
}