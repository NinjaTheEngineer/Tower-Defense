using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerPlacer : NinjaMonoBehaviour {
    public static TowerPlacer Instance;
    public Tower towerGO;
    public int towerPrice = 5;
    public float nearTowerDistance = 0.5f;
    public EventSystem eventSystem;
    private Tower towerBlueprint;
    public LayerMask placeableGroundLayer;
    public LayerMask groundLayer;
    public System.Action OnTowerPlaced;
    public List<Tower> placedTowers;
    public CircularIndicator placementIndicator;
    private bool nearAnyTower;
    private bool hasEnoughGoldForTower;
    private void Awake() {
        string logId = "Awake";
        if(Instance==null) {
            logd(logId, "Setting Singleton Instance to GameObject.");
            Instance = this;
        } else {
            logw(logId, "Singleton Instance already set => Destroying this GameObject.");
            Destroy(gameObject);
        }
        if(eventSystem==null) {
            logw(logId,"EventSystem="+eventSystem.logf()+" => Setting to EventSystem.current");
            eventSystem = EventSystem.current;
        }
        RegisterListeners();
    }
    public void RegisterListeners() {
        GameManager.OnStartGame += DestroyPlacedTowers;
        ResourcesManager.OnGoldUpdated += (int goldAmount) => hasEnoughGoldForTower = towerPrice < goldAmount;
    }
    public void RemoveListeners() {
        GameManager.OnStartGame -= DestroyPlacedTowers;
        ResourcesManager.OnGoldUpdated -= (int goldAmount) => hasEnoughGoldForTower = towerPrice < goldAmount;
    }
    private void DestroyPlacedTowers() {
        string logId = "DestroyPlacedTowers";
        int towersPlacedCount = placedTowers.Count;
        if(towersPlacedCount==0) {
            logd(logId, "No towers to destroy => returning");
            return;
        }
        for (int i = 0; i < towersPlacedCount; i++) {
            Tower currentTower = placedTowers[i];
            if(currentTower==null || !currentTower.isActiveAndEnabled) {
                logd(logId, "Skipping Tower="+currentTower.logf()+" on index="+i);
                continue;
            }
            Destroy(currentTower.gameObject);
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
            logd(logId, "Mouse 0 down TowerBlueprint="+towerBlueprint.logf()+" => PlaceTower");
            PlaceTower();
        }
        if(Input.GetMouseButtonDown(1) && towerBlueprint!=null) {
            logd(logId, "Mouse 1 down TowerBlueprint="+towerBlueprint.logf()+" => Cleaning tower");
            Destroy(towerBlueprint.gameObject);
            towerBlueprint=null;
        }
    }
    public void InstantiateTowerBlueprint() {
        string logId = "InstantiateTowerBlueprint";
        if(towerBlueprint!=null) {
            logw(logId, "Already placing a TowerBlueprint="+towerBlueprint.logf()+" => no-op");
            return;
        }
        logd(logId, "TowerBlueprint="+towerBlueprint.logf()+" => Instatiate TowerBlueprint");
        towerBlueprint = Instantiate(towerGO).GetComponent<Tower>();
        SetIndicators();
        StartCoroutine(CheckTowersNearbyRoutine());
    }
    private void SetIndicators() {
        placementIndicator.Activate();
        placementIndicator.SetSize(nearTowerDistance);
        placementIndicator.FollowTarget = towerBlueprint.transform;
    }
    private IEnumerator CheckTowersNearbyRoutine() {
        string logId = "CheckTowersNearbyRoutine";
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
        int placedTowersCount = placedTowers.Count;
        while(placedTowersCount>0 && towerBlueprint && towerBlueprint.IsBeingPlaced) {
            nearAnyTower = false;
            if(placedTowersCount==0) {
                yield break;
            }
            for (int i = 0; i < placedTowersCount; i++) {
                Tower currentPlacedTower = placedTowers[i];
                if(currentPlacedTower==null) {
                    logw(logId, "CurrentPlacedTower="+currentPlacedTower.logf()+" => Continuing");
                    yield return  waitForSeconds;
                    continue;
                }
                float distanceToTower = (currentPlacedTower.transform.position - towerBlueprint.transform.position).magnitude;
                if(nearTowerDistance > distanceToTower) {
                    logd(logId, "CurrentPlacedTower="+currentPlacedTower.logf()+" TowerBlueprint="+towerBlueprint.logf()+" Distance="+distanceToTower+" => Continuing");
                    nearAnyTower = true;
                    break;
                }
            }
            yield return waitForSeconds;
        }
        logd(logId, "PlacedTowersCount="+placedTowersCount+" TowerBluePrint="+towerBlueprint.logf()+" => Breaking routine");
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
        bool isPlaceable = towerBlueprint.CanBePlaced;
        bool canPlaceTower = !nearAnyTower && hasEnoughGoldForTower;
        if(canPlaceTower && Physics.Raycast(ray, out hitInfo, float.MaxValue, placeableGroundLayer)) {
            towerBlueprint.transform.position = hitInfo.point;
            if(!towerBlueprint.CanBePlaced) {
                logd(logId, "Tower="+towerBlueprint+" NearAnyTower="+nearAnyTower+"HasEnoughGoldForTower="+hasEnoughGoldForTower+" Ray="+ray.logf()+" HitInfo="+hitInfo.logf()+" => Can be Placed", true);
                towerBlueprint.CanBePlaced = true;
                placementIndicator.SetPrimaryColor();
            }
        } else if(Physics.Raycast(ray, out hitInfo, float.MaxValue, groundLayer) || !canPlaceTower) {
            towerBlueprint.transform.position = hitInfo.point;
            logd(logId, "Tower="+towerBlueprint+" NearAnyTower="+nearAnyTower+" HasEnoughGoldForTower="+hasEnoughGoldForTower+" Ray="+ray.logf()+" HitInfo="+hitInfo.logf()+" => Cannot be Placed.", true);
            towerBlueprint.CanBePlaced = false;
            placementIndicator.SetSecondaryColor();
        }
    }

    private void PlaceTower() {
        string logId = "PlaceTower";
        if(towerBlueprint==null) {
            logw(logId, "TowerBlueprint is null => no-op");
            return;
        }
        if(towerBlueprint.CanBePlaced && ResourcesManager.Instance.SpendGold(towerPrice)) {
            logd(logId, "TowerBlueprint="+towerBlueprint+" => Place");
            //InvokeOnTowerPlaced();
            bool placed = towerBlueprint.Place();
            if(placed) {
                logd(logId, "Placing tower="+towerBlueprint.logf());
                placedTowers.Add(towerBlueprint);
                placementIndicator.Deactivate();
                towerBlueprint = null;
            } else {
                logw(logId, "It wasn't possible to place tower="+towerBlueprint.logf());
            }
        } else {
            logd(logId, "Not even gold for tower => no-op");
        }
    }
    private void InvokeOnTowerPlaced() {
        string logId = "InvokeOnTowerPlaced";
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
    private void OnDestroy() {
        RemoveListeners();
    }
}
