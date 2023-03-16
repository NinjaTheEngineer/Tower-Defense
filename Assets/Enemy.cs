using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NinjaMonoBehaviour {
    public float speed = 5f;
    public int maxHealth = 50;
    public float waypointTargetDistance = 0.25f;
    public float distanceToDamageCore = 1f;
    public int damage;
    private int _currentHealth;
    public int CurrentHealth {
        get => _currentHealth;
        private set {
            string logId = "CurrentHealth_set";
            if(value==_currentHealth) {
                logd(logId, "CurrentHealth is already " + value + " => returning");
                return;
            }
            logd(logId, "Setting CurrentHealth from " + _currentHealth + " to " + value);
            _currentHealth = value;
        }
    }
    private Path path;
    private Transform _currentWaypoint;
    public Core core;
    public Transform CurrentWaypoint {
        get => _currentWaypoint;
        private set {
            string logId = "CurrentWaypoint_set";
            if(_currentWaypoint==null) {
                logw(logId, "Tried to set CurrentWaypoint from " + _currentWaypoint.logf()+ " to " + value.logf());
            }
            if(value==_currentWaypoint) {
                logd(logId, "CurrentWaypoint is already " + value.logf() + " => returning");
                return;
            }
            logd(logId, "Setting CurrentWaypoint from " + _currentWaypoint.logf() + " to " + value.logf());
            _currentWaypoint = value;
        }
    }
    private void Awake() {
        CurrentHealth = maxHealth;
    }
    private void Start() {
        StartCoroutine(IsCloseToCoreRoutine());
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
    private IEnumerator IsCloseToCoreRoutine() {
        string logId = "IsCloseToCoreRoutine";
        while(true) {
            float distanceToCore = DistanceToCore;
            if(distanceToCore < 0) {
                logd(logId,"Distance to core is "+distanceToCore+" => continuing");
                yield return new WaitForSecondsRealtime(0.5f);
                continue;
            }
            if(distanceToCore < distanceToDamageCore) {
                logd(logId,"Distance to core is "+distanceToCore+" => Damaging core and destroying self.");
                core.Damage(damage);
                Destroy(gameObject);
            } else {
                logd(logId,"Distance to core is "+distanceToCore+" => continuing");
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    private float DistanceToCore {
        get {
            string logId = "DistanceToCore_get";
            if(core==null) {
                logd(logId, "Core is null => returning -1");
                return -1;    
            }
            float distanceToCore = (core.transform.position - transform.position).magnitude;
            logd(logId, "Returning "+distanceToCore);
            return distanceToCore;
        }
    }
    private void HandleMovement() {
        Vector3 direction = CurrentWaypoint.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
    public void TakeDamage(int damage) {
        string logId = "TakeDamage";
        if(damage<=0) {
            logd(logId, "Can't take damage="+damage+" => no-op");
            return;
        }
        CurrentHealth -= damage;
        if(CurrentHealth<=0) {
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
