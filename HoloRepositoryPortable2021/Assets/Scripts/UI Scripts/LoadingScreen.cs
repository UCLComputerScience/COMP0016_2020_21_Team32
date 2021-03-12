using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start(){
        StartCoroutine(waitForModel());
    }
    private IEnumerator waitForModel(){
        yield return new WaitUntil(() => ModelHandler.current.modelRadius != 0);
        this.gameObject.SetActive(false);
    }
}
