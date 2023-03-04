using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaMonoBehaviour : MonoBehaviour {

    public void logd(string id=null, string message=null) {
        if(string.IsNullOrEmpty(id) && string.IsNullOrEmpty(message)) {
            logw();
            return;
        }
        if(string.IsNullOrEmpty(id)){
            Debug.Log(name + "::" + message);
        } else if(string.IsNullOrEmpty(message)) {
            Debug.Log(name + "::" + id + "-> No message in log.");
        } else {
            Debug.Log(name + "::" + id + "->" + message);
        }
    }
    
    public void logw(string id=null, string message=null) {
        if(string.IsNullOrEmpty(id) && string.IsNullOrEmpty(message)) {
            Debug.LogWarning(name + "::" + "Nothing to log");
            return;
        }
        if(string.IsNullOrEmpty(id)){
            Debug.LogWarning(name + "::" + message);
        } else if(string.IsNullOrEmpty(message)) {
            Debug.LogWarning(name + "::" + id + "-> No message in log.");
        } else {
            Debug.LogWarning(name + "::" + id + "->" + message);
        }
    }
    
    public void loge(string id=null, string message=null) {
        if(string.IsNullOrEmpty(id) && string.IsNullOrEmpty(message)) {
            logw();
            return;
        }
        if(string.IsNullOrEmpty(id)){
            Debug.LogError(name + "::" + message);
        } else if(string.IsNullOrEmpty(message)) {
            Debug.LogError(name + "::" + id + "-> No message in log.");
        } else {
            Debug.LogError(name + "::" + id + "->" + message);
        }
    }
    public void logt(string id=null, string message=null) {
        return;
    }
}