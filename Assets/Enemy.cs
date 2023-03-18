using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NinjaMonoBehaviour {
    public float speed = 5f;
    public float waypointTargetDistance = 0.25f;
    public float distanceToDamageCore = 1f;
    public int damage;
    [SerializeField]
    private Health _health;
    public Health Health {
        get => _health;
    }
    private Path path;
    private Transform _currentWaypoint;
    public Core core;
    public Transform CurrentWaypoint {
        get => _currentWaypoint;
        private set {
            string logId = "CurrentWaypoint_set";
            if(value==null) {
                logw(logId, "Tried to set CurrentWaypoint from " + _currentWaypoint.logf()+ " to " + value.logf());
                return;
            }
            if(value==_currentWaypoint) {
                logd(logId, "CurrentWaypoint is already " + value.logf() + " => returning");
                return;
            }
            logt(logId, "Setting CurrentWaypoint from " + _currentWaypoint?.name.logf() + " to " + value?.name.logf());
            _currentWaypoint = value;
        }
    }
    private void Awake() {
        _health = _health??GetComponent<Health>();
    }
    private void Start() {
        string logId = "Start";
        if(path==null) {
            logw("Path is null => no-op");
            return;
        }
        core = path.Core;
        CurrentWaypoint = path.NextWaypoint();
        StartCoroutine(CoreAttackRoutine());
    }
    private void Update() {
        string logId = "Update";
        if(!GameManager.Instance.GameStarted) {
            return;
        }
        if(CurrentWaypoint==null) {
            if(path==null) {
                return;
            }
            logd(logId, "CurrentWaypoint is null => Tried to set waypoint");
            CurrentWaypoint = path.NextWaypoint();
            core = path.Core;
            return;
        }
        HandleMovement();
        float distanceToWaypoint = (CurrentWaypoint.position - transform.position).magnitude;
        if(distanceToWaypoint <= waypointTargetDistance) {
            CurrentWaypoint = path.NextWaypoint(CurrentWaypoint);
        }
    }
    private IEnumerator CoreAttackRoutine() {
        string logId = "CoreAttackRoutine";
        while(core) {
            float distanceToCore = DistanceToCore;
            if(distanceToCore < 0) {
                logd(logId,"Distance to core is "+distanceToCore+" => continuing");
                yield return new WaitForSecondsRealtime(0.5f);
                continue;
            }
            if(distanceToCore < distanceToDamageCore) {
                logd(logId,"Distance to core is "+distanceToCore+" => Damaging core and destroying self.");
                core.TakeDamage(damage);
                Destroy(gameObject);
            } else {
                logt(logId,"Distance to core is "+distanceToCore+" => continuing");
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
        logd(logId,"Core is null => Breaking routine.");
    }
    private float DistanceToCore {
        get {
            string logId = "DistanceToCore_get";
            if(core==null) {
                logd(logId, "Core is null => returning -1");
                return -1;    
            }
            float distanceToCore = (core.transform.position - transform.position).magnitude;
            logt(logId, "Returning "+distanceToCore);
            return distanceToCore;
        }
    }
    private void HandleMovement() {
        Vector3 direction = CurrentWaypoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
    public void TakeDamage(int damage) {
        string logId = "TakeDamage";
        if(_health==null) {
            logd(logId, "Health component is missing => no-op");
            return;
        }
        logd(logId, "Taking damage of "+damage);
        _health.CurrentHealth -= damage;
        int currentHealth = _health.CurrentHealth;
        if(currentHealth<=0) {
            logd(logId, "Enemy is dead => Destroying self");
            Destroy(gameObject);
        }
    }

    public void SetPath(Path path)  {
        string logId = "SetPath";
        if(path==null) {
            logd(logId, "Tried to set path to null => no-op");
            return;
        }
        this.path = path;
    }
}
