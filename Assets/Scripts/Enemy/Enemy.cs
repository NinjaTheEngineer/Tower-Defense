using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : NinjaMonoBehaviour {
    public float speed = 5f;
    public float waypointTargetDistance = 0.25f;
    public float distanceToDamageCore = 1f;
    public int damage;
    [SerializeField]
    private Health _health;
    public Health Health {
        get => _health; 
        private set {
            string logId = "Health_set";
            if(_health==value) {
                logd(logId, "Tried to set Health to same value of "+value.logf());
                return;
            }
            logd(logId, "Setting Health from "+_health+" to "+value);
            _health=value;
        }
    }
    private Path path;
    private Transform _currentWaypoint;
    public Core core;
    public System.Action<Enemy> OnDeath;
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
    private void Start() {
        string logId = "Start";
        if(path==null) {
            logw(logId,"Path is null => no-op");
            return;
        }
        if(!_health) {
            _health = GetComponent<Health>();
        }
        Health.OnDamageTaken -= DamageTaken;
        Health.OnDamageTaken += DamageTaken;

        core = path.Core;
        CurrentWaypoint = path.NextWaypoint();
        StartCoroutine(SetWaypointRoutine());
        StartCoroutine(CoreAttackRoutine());
    }
    public void DamageTaken() { 
        string logId = "DamageTaken";
        int currentHealth = _health.CurrentHealth;
        logd(logId, "Took damage CurrentHealth="+currentHealth);
        if(currentHealth<=0) {
            logd(logId, "Took damage CurrentHealth="+currentHealth+" => Death");
            Death();
        }
    }
    private void Update() {
        string logId = "Update";
        if(!GameManager.Instance.GameStarted) {
            return;
        }
        HandleMovement();
    }
    private IEnumerator SetWaypointRoutine() {
        string logId = "SetWaypointRoutine";
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);
        while(gameObject.activeInHierarchy) {
            bool gameStarted = GameManager.Instance.GameStarted;
            if(!gameStarted) {
                logd(logId, "GameStarted="+gameStarted+" => Continuing");
                yield return waitForSeconds;
                continue;
            }
            if(CurrentWaypoint==null) {
                if(path==null) {
                    logd(logId, "CurrentWaypoint="+CurrentWaypoint.logf()+" Path="+path.logf()+" => Continuing");
                    yield return waitForSeconds;
                    continue;
                }
                logd(logId, "CurrentWaypoint is null => Tried to set waypoint");
                CurrentWaypoint = path.NextWaypoint();
                core = path.Core;
                logd(logId, "CurrentWaypoint="+CurrentWaypoint.logf()+" Path="+path.logf()+" Core="+core.logf()+" => Continuing");
                yield return waitForSeconds;
                continue;
            }
            float distanceToWaypoint = (CurrentWaypoint.position - transform.position).magnitude;
            if(distanceToWaypoint <= waypointTargetDistance) {
                logd(logId, "OurPosition="+transform.position+" CurrentWaypoint="+CurrentWaypoint.logf()+" DistanceToWaypoint="+distanceToWaypoint+" Path="+path.logf()+" Core="+core.logf()+" => Setting new Waypoint.");
                CurrentWaypoint = path.NextWaypoint(CurrentWaypoint);
            }
            logt(logId, "OurPosition="+transform.position+" CurrentWaypoint="+CurrentWaypoint.logf()+" DistanceToWaypoint="+distanceToWaypoint+" Path="+path.logf()+" Core="+core.logf()+" => Continuing.");
            yield return waitForSeconds;
        }
        logd(logId, "Breaking routine.");
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
                core.Health.TakeDamage(damage);
                Death();
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
    public void Death() {
        string logId = "Death";
        logd(logId,"Invoking OnDeath => Destroying GameObject");
        OnDeath.Invoke(this);   
        Destroy(gameObject);
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
