using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaMonoBehaviour : MonoBehaviour {

    public void logd(string id, string message) {
        Debug.Log(name + "::" + id + "->" + message);
    }
    
    public void logw(string id, string message) {
        Debug.LogWarning(name + "::" + id + "->" + message);
    }
    
    public void loge(string id=null, string message=null) {
        Debug.LogError(name + "::" + id + "->" + message);

    }
    public void logt(string id=null, string message=null) {
        return;
    }
}