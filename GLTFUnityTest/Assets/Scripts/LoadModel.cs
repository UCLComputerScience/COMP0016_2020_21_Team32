using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Siccity;
using System.IO;
using System;

public class LoadModel : MonoBehaviour
{
    RaycastHit[] hits;
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
        // model.transform.localPosition = Vector3.zero;
        // model.transform.localRotation = Quaternion.Euler(0f,0f,0f);
        // foreach(Transform t in model.transform){
        //     Debug.Log("Trying to set the transform: "+ t.name);
        //     t.SetParent(model.transform);
        //     t.localPosition = Vector3.zero;
        //     t.localRotation = Quaternion.Euler(0f,0f,0f);
        // }
        model.transform.localPosition = new Vector3(-94.2f, -99.23f, -93.6f);
        model.transform.localRotation = Quaternion.Euler(0.453f, -288.9f, 1.323f);

        // foreach(Transform child in model.transform){
        //     child.gameObject.AddComponent<MeshCollider>();
        //     Debug.Log("mesh checking in");
        // }
        // Debug.Log("Hits checking in");
        // hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward);
        // Array.Sort(hits, (RaycastHit a, RaycastHit b) => (int) (a.distance - b.distance));
        // foreach(RaycastHit hit in hits){
        //     Debug.Log("This is what we're interested in: "+ hit.transform.gameObject.name);
        // }

        int count = 0;
        foreach(Transform child in model.transform){
            if(child.gameObject.GetComponent<Renderer>() != null && count != 1)
            {
//                Debug.Log("This is what you're after: "+ child.gameObject.name); 
                segments.Add(child.gameObject);
            }
            if(count ==1)child.gameObject.GetComponent<Renderer>().enabled = false;
            else if(child.gameObject.GetComponent<Camera>() != null) child.gameObject.SetActive(false);
            count++;   
        }
    }


}


