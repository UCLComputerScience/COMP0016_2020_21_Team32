
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

///<summary>
///This class defines all the events that trigger UI actions to occur, as well as the methods to fire them.
///As this class is a singleton, it can be referenced from any other class and its methods called from any other class.
///</summary>

public class EventManager : MonoBehaviour
{
    public static EventManager current; 
    public event EventHandler OnEnableCamera;
    public event EventHandler OnEnablePivot;
    public event EventHandler OnEnableCrossSection;
    public event EventHandler OnEnableDicom;
    public event EventHandler OnReset;
    public event EventHandler OnViewAnnotations;
    public event EventHandler OnAddAnnotations;
    public event EventHandler OnSelectAnnotation;
    public event EventHandler OnSegmentSelect;
    public event EventHandler<EventArgsColourData> OnColourSelect;
    public event EventHandler OnToggleOpacitySlider;
    public event EventHandler OnToggleSegmentSelect;
    public event EventHandler OnToggleNavigationBar;
    public event EventHandler OnToggleLogos;
    public event EventHandler OnToggleColourPalette;
    public event EventHandler OnChangeSettings;
    public event EventHandler OnEnableUIBlocker;
    public event EventHandler OnDisableUIBlocker;
    public event EventHandler OnToggleFullScreen;
    public event EventHandler<EventArgsFloat> OnChangeOpacity;

    private void Awake(){
        //Making use of the Singleton design pattern
        if(current != null && current != this){
            Destroy(this.gameObject);
        }else{
            current = this;
        }
    }
    public void onEnableUIBlocker(){
        OnEnableUIBlocker?.Invoke(this, EventArgs.Empty);
    }
    public void onDisableUIBlocker(){
        OnDisableUIBlocker?.Invoke(this, EventArgs.Empty);
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
    public void onSegmentSelect(){
        OnSegmentSelect?.Invoke(this, EventArgs.Empty);
    }
    public void onAddAnnotations(){
        OnAddAnnotations?.Invoke(this, EventArgs.Empty);
    }
    public void onSelectAnnotation(){
        OnSelectAnnotation?.Invoke(this, EventArgs.Empty);
    }
    public void onToggleOpacitySlider(){
        OnToggleOpacitySlider?.Invoke(this, EventArgs.Empty);
    }
    public void onToggleSegmentSelect(){
        OnToggleSegmentSelect?.Invoke(this, EventArgs.Empty);
    }
    public void onToggleNavigationBar(){
        OnToggleNavigationBar?.Invoke(this, EventArgs.Empty);
    }
    public void onToggleLogos(){
        OnToggleLogos?.Invoke(this, EventArgs.Empty);
    }
    public void onToggleColourPalette(){
        OnToggleColourPalette?.Invoke(this, EventArgs.Empty);
    }
    public void onChangeSettings(){
        OnChangeSettings?.Invoke(this, EventArgs.Empty);
    }
    public void onToggleFullScreen(){
        OnToggleFullScreen?.Invoke(this, EventArgs.Empty);
    }
    public void onColourSelect(Color col){
        EventArgsColourData e = new EventArgsColourData(col);
        OnColourSelect?.Invoke(this, e);
    }
    public void onChangeOpacity(float op){
        EventArgsFloat e = new EventArgsFloat(op);
        OnChangeOpacity?.Invoke(this, e);
    }
}
public class EventArgsColourData : EventArgs{
    public Color col;
    public EventArgsColourData(Color col){
        this.col = col;
    }
}
public class EventArgsFloat : EventArgs{
    public float value;
    public EventArgsFloat(float value){
        this.value = value;
    }
}