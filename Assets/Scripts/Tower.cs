using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tower : NinjaMonoBehaviour {
    
    public TowerState currentState;
    public enum TowerState {
        BeingPlaced,
        Placed
    }
    public GameObject blueprintVisu;
    public GameObject placedVisu;
    public bool BeingPlaced => currentState==TowerState.BeingPlaced;
    private void Awake() {
        currentState = TowerState.BeingPlaced;
    }
    private void Start() {
        RefreshTowerVisu();
    }
    private void RefreshTowerVisu() {
        string logId = "RefreshTowerVisu";
        bool visuActive = placedVisu.activeInHierarchy;
        if(currentState==TowerState.BeingPlaced && visuActive) {
            logd(logId, "CurrentState="+currentState+" VisuActive="+visuActive+" => Setting blueprintVisu");
            blueprintVisu.SetActive(true);
            placedVisu.SetActive(false);
        }
        if(currentState==TowerState.Placed && !placedVisu.activeInHierarchy) {
            logd(logId, "CurrentState="+currentState+" VisuActive="+visuActive+" => Setting placedVisu");
            blueprintVisu.SetActive(false);
            placedVisu.SetActive(true);    
        }
    }
    public void Place() {
        string logId = "Place";
        logd(logId, "Setting current tower state to Placed.");
        currentState = TowerState.Placed;
        RefreshTowerVisu();
    }
}
