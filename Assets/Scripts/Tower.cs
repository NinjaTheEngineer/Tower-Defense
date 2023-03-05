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
    public bool IsBeingPlaced => currentState==TowerState.BeingPlaced;
    public bool IsPlaced => currentState==TowerState.Placed;
    public float shootingRadius = 5f;
    public float shootingDelay = 1f;
    public Transform shootingPoint;
    public Projectile projectilePrefab;
    public LayerMask enemyLayer;
    [SerializeField] private Transform shootingTarget;
    public bool canShoot = true;
    private void Awake() {
        currentState = TowerState.BeingPlaced;
    }
    private void Start() {
        RefreshTowerVisu();
    }
    private void Update() {
        if(GameManager.Instance.GameStarted && IsPlaced) {
            shootingTarget = FindClosestEnemy();
            if(shootingTarget!=null && canShoot) {
                StartCoroutine(Shoot());
            } else {
                
            }
        }
    }
    private Transform FindClosestEnemy() {
        string logId = "FindClosestEnemy";
        Collider[] colliders = Physics.OverlapSphere(transform.position, shootingRadius, enemyLayer);
        float minDistance = -1;
        Transform closestEnemy = null;
        int collidersCount = colliders.Length;
        if(collidersCount==0) {
            logt(logId, "Nothing found => returning null");
            return closestEnemy;
        }
        for (int i = 0; i < collidersCount; i++) {
            Collider currentCollider = colliders[i];
            float distance = Vector3.Distance(transform.position, currentCollider.transform.position);
            if(minDistance==-1 || distance < minDistance) {
                minDistance = distance;
                closestEnemy = currentCollider.transform;
            }
        }
        return closestEnemy;
    }

    private IEnumerator Shoot() {
        string logId = "Shoot";
        logd(logId, "Starting Shoot routine");
        canShoot = false;
        Projectile projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity).GetComponent<Projectile>();
        projectile.SetTarget(shootingTarget);
        yield return new WaitForSeconds(shootingDelay);
        logd(logId, "Setting canShoot to true");
        canShoot = true;
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
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}
