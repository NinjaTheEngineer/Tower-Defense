using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : NinjaMonoBehaviour {
    public static TowerPlacer Instance;
    public Tower towerGO;
    public int towerPrice = 5;
    public EventSystem eventSystem;
    private Tower towerBlueprint;
    public LayerMask placeableLayer;
    public System.Action OnTowerPlaced;
    public bool HasEnoughGoldForTower => ResourcesManager.Instance.CurrentGoldAmount >= towerPrice;
    private void Awake() {
        string logId = "Awake";
        if(Instance==null) {
            logd(logId, "Setting Singleton Instance to GameObject.");
            Instance = this;
        } else {
            logw(logId, "Singleton Instance already set => Destroying this GameObject.");
            Destroy(gameObject);
        }
    }
    private void Update() {
        string logId = "Update";
        if(!GameManager.Instance.GameStarted) {
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) && towerBlueprint==null) {
            InstantiateTowerBlueprint();
        }
        if(towerBlueprint && towerBlueprint.IsBeingPlaced) {
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
        if(ResourcesManager.Instance.SpendGold(towerPrice)) {
            logd(logId, "TowerBlueprint="+towerBlueprint+" => Place");
            InvokeOnTowerPlaced();
            towerBlueprint.Place();
            towerBlueprint = null;
        } else {
            logd(logId, "Not even gold for tower => no-op");
        }
    }
    private void InvokeOnTowerPlaced() {
        string logId = "InvokeOnGoldUpdated";
        if(OnTowerPlaced==null) {
            logw(logId, "No listeneres registered for OnTowerPlaced event => no-op");
            return;
        }
        OnTowerPlaced.Invoke();
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
