using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : NinjaMonoBehaviour {
    public Tower towerGO;
    public EventSystem eventSystem;
    private Tower towerBlueprint;
    public LayerMask placeableLayer;
    private void Update() {
        string logId = "Update";
        if(!GameManager.Instance.GameStarted) {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && towerBlueprint==null) {
            InstantiateTowerBlueprint();
        }
        if(towerBlueprint && towerBlueprint.BeingPlaced) {
            HandleTowerBlueprintPosition();
        }
        if(Input.GetMouseButtonDown(0) && towerBlueprint!=null) {
            logd(logId, "Mouse 0 down TowerBlueprint="+towerBlueprint.logf()+"=> PlaceTower");
            PlaceTower();
        }
    }
    public void InstantiateTowerBlueprint() {
        string logId = "InstantiateTowerBlueprint";
        if(towerBlueprint!=null) {
            logw(logId, "Already placing a TowerBlueprint="+towerBlueprint.logf()+"=> no-op");
            return;
        }
        logd(logId, "TowerBlueprint="+towerBlueprint.logf()+"=>Instatiate TowerBlueprint");
        towerBlueprint = Instantiate(towerGO).GetComponent<Tower>();
    }
    private void HandleTowerBlueprintPosition() {
        string logId = "HandleTowerBlueprintPosition";
        if(towerBlueprint==null) {
            logw(logId, "TowerBeingPlaced is null => no-op");
            return;
        }
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, float.MaxValue, placeableLayer)) {
            logt(logId, "Ray="+ray+" HitInfo=" + hitInfo);
            towerBlueprint.transform.position = hitInfo.point;
        }
    }

    private void PlaceTower() {
        string logId = "PlaceTower";
        if(towerBlueprint==null) {
            logw(logId, "TowerBlueprint is null => no-op");
            return;
        }
        logd(logId, "TowerBlueprint="+towerBlueprint+" => Place");
        towerBlueprint.Place();
        towerBlueprint = null;
    }

    private bool IsMouseOverUI() {
        string logId = "IsMouseOverUI";
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        if(results.Count > 0) {
            logw(logId, "Not placing Tower because mouse is hovering the UI.");
            return true;
        }
        return false;
    }
}
