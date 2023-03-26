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
    public Transform shootingPoint;
    public Projectile projectilePrefab;
    public LayerMask enemyLayer;
    [Header("Shooting")]
    public bool canShoot = true;
    public int shootingDamage = 5;
    public float shootingSpeed = 10;
    public float shootingRadius = 5f;
    public float shootingDelay = 1f;
    public CircularIndicator attackRangeIndicator;
    [SerializeField] private Transform shootingTarget;
    [SerializeField] private Transform turretHolder;
    private Renderer[] _renderers;
    public Renderer[] Renderers {
        get {
            if(IsBeingPlaced) {
                _renderers = blueprintVisu.GetComponentsInChildren<Renderer>();
            } else {
                _renderers = placedVisu.GetComponentsInChildren<Renderer>();
            }
            return _renderers;
        }
    }
    private bool _canBePlaced;
    public bool CanBePlaced {
        get => _canBePlaced;
        set {
            string logId = "CanBePlaced_set";
            if(_canBePlaced==value) {
                logt(logId, "Tried to set CanBePlaced to same value of "+value);
                return;
            }
            logd(logId, "Setting CanBePlaced from "+_canBePlaced+" to "+value);
            _canBePlaced = value;
            //MeshRenderer renderer = blueprintVisu.GetComponent<MeshRenderer>();
            Renderer[] renderers = Renderers;
            int renderersCount = renderers.Length;
            if(renderersCount==0) {
                logw(logId, "RenderersCount="+renderersCount+" Renderers="+renderers.logf()+" => returning");
                return;
            }
            
        }
    }
    private void Awake() {
        currentState = TowerState.BeingPlaced;
    }
    private void Start() {
        RefreshTowerVisu();
        CanBePlaced = true;
        StartIndicators();
    }
    private void StartIndicators() {
        attackRangeIndicator.Activate();
        attackRangeIndicator.SetSize(shootingRadius);
        attackRangeIndicator.FollowTarget = transform;
    }
    private void Update() {
        if(GameManager.Instance.GameStarted && IsPlaced) {
            shootingTarget = FindClosestEnemy();
            if(shootingTarget!=null && canShoot) {
                StartCoroutine(ShootRoutine());
                StartCoroutine(AlignTurretRoutine());
            } else {
                
            }
        }
        attackRangeIndicator.SetSize(shootingRadius);
        attackRangeIndicator.FollowTarget = attackRangeIndicator.transform;
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
    private IEnumerator AlignTurretRoutine() {
        string logId = "AlignTurretRoutine";
        logd(logId, "Starting routine while ShootingTarget="+shootingTarget.logf()+" TurretHolder="+turretHolder.logf());
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.075f);
        while(shootingTarget && turretHolder) {
            turretHolder.LookAt(shootingTarget);
            yield return waitForSeconds;
        }
        logd(logId, "Breaking routine while ShootingTarget="+shootingTarget.logf()+" TurretHolder="+turretHolder.logf());
    }
    private IEnumerator ShootRoutine() {
        string logId = "ShootRoutine";
        logd(logId, "Starting Shoot routine");
        canShoot = false;
        Projectile projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity).GetComponent<Projectile>();
        projectile.InitializeProjectile(shootingTarget, shootingDamage, shootingSpeed);
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
    public bool Place() {
        string logId = "Place";
        if(!_canBePlaced) {
            logd(logId, "Tried to place tower on unplacable position. => returning false");
            return false;
        }
        logd(logId, "Setting current tower state to Placed. => returning true");
        currentState = TowerState.Placed;
        RefreshTowerVisu();
        return true;
    }
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}
