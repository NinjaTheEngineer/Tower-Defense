using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : NinjaMonoBehaviour {
    private Camera mainCamera;
    private void Start() {
        string logId = "Start"; 
        mainCamera = Camera.main;
        if(mainCamera==null) {
            logd(logId, "MainCamera is missing => no-op");
            return;
        }
        StartCoroutine(ResolveRotationRoutine());
    }
    
    private IEnumerator ResolveRotationRoutine() {
        string logId = "ResolveRotationRoutine";
        logd(logId, "Starting "+logId);
        WaitForSeconds waitSeconds = new WaitForSeconds(0.015f);
        while(mainCamera!=null) {
            transform.forward = mainCamera.transform.forward;
            yield return waitSeconds;
        }
    }
}
