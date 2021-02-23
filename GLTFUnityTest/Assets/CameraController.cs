using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CameraController : MonoBehaviour
{
    private Vector3 curMouse;
    private Vector3 prevMouse;
    private Vector3 mouseDelta;
    public bool activeRotate;
    private Vector3 pivot = new Vector3(0f,0f,0f);
    private float scrollSpeed = 5f;
    private Vector3 displacement = new Vector3(0f,0f,-250f);
    private Vector3 farTarget;
    private Vector3 prevPos;
    private Vector3 translationCache = Vector3.zero;

    void Update(){
        if(Input.GetMouseButton(0))
        {
            activeRotate = true;
        }
        else activeRotate = false;

        curMouse = Input.mousePosition;
        mouseDelta = (prevMouse - curMouse).normalized;
        Debug.Log(mouseDelta);
        
    }
    void LateUpdate(){
        prevPos = Camera.main.transform.position;
        if(activeRotate){
            farTarget = getFarTarget2(Vector3.zero, Camera.main.transform.position - translationCache, 1000f);
            Camera.main.transform.position = pivot + translationCache;
            Camera.main.transform.Rotate(new Vector3(1f,0f,0f), mouseDelta.y);
            Camera.main.transform.Rotate(new Vector3(0f,1f,0f), -mouseDelta.x, Space.World);
            Camera.main.transform.Translate(displacement);
            //Camera.main.transform.position += translationCache;
        }else if(Input.GetAxis("Mouse ScrollWheel") != 0){
            //Camera.main.transform.position = pivot;
            Camera.main.transform.position = pivot;
            float scrollAmount = Input.GetAxis("Mouse ScrollWheel")*scrollSpeed;
            displacement -= new Vector3(0, 0, scrollAmount);
            Camera.main.transform.Translate(displacement);
        }else if(Input.GetKey(KeyCode.D)){
            StartCoroutine(cameraDist());
        }

        prevMouse = Input.mousePosition;
    }
    private IEnumerator cameraDist(){
        Camera.main.transform.RotateAround(farTarget, Camera.main.transform.up, -0.1f);
        Vector3 curPos = Camera.main.transform.position;
        yield return new WaitUntil(() => prevPos != curPos);
        Vector3 diff = curPos - prevPos;
        Debug.Log("The difference isssss: "+ diff);
        translationCache+= new Vector3(diff.x, 0f, 0f);
        Debug.Log(translationCache);
    }

    private Vector3 getFarTarget2(Vector3 oPiv, Vector3 cameraPos, float distance){
        Vector3 dirVec = (oPiv - cameraPos).normalized;
        Debug.DrawRay(Camera.main.transform.position, dirVec*distance, Color.red, 1f);
        return dirVec * distance;
    }
}
 