using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Siccity;
using System.IO;
using System;

public class LoadModel : MonoBehaviour
{
    public void LoadInModel(GameObject parent, GameObject model, List<GameObject> segments, string fileName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        #if UNITY_ANDROID && !UNITY_STANDALONE
            StartCoroutine(getModel(path, (UnityWebRequest request) =>
                {
                if(request.isNetworkError || request.isHttpError){
                    Debug.Log("error");
                }else{
                    model = Siccity.GLTFUtility.Importer.LoadFromBytes(request.downloadHandler.data);
                    initialiseModel(parent, model, segments);
                }
            }));
        #else
            model = Siccity.GLTFUtility.Importer.LoadFromFile(path);
            initialiseModel(parent, model, segments);
        #endif
        // initialiseModel(parent, model);
    }

    IEnumerator getModel(string filepath, Action<UnityWebRequest> callback){
        using(UnityWebRequest request = UnityWebRequest.Get(filepath)){
            yield return request.SendWebRequest();
            callback(request);
        }
    }
    private void initialiseModel(GameObject parent, GameObject model, List<GameObject> segments){
        model.transform.SetParent(parent.transform);
        model.transform.localPosition = new Vector3(-94.2f, -99.23f, -93.6f);
        model.transform.localRotation = Quaternion.Euler(0.453f, -288.9f, 1.323f);

        int count = 0;
        foreach(Transform child in model.transform){
            if(child.gameObject.GetComponent<Renderer>() != null && count != 1)
            { 
                segments.Add(child.gameObject);
            }
            if(count ==1)child.gameObject.GetComponent<Renderer>().enabled = false;
            count++;   
        }
    }


}


