using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : NinjaMonoBehaviour {
    [SerializeField]
    private int minDamage = 1;
    [SerializeField]
    private float minSpeed = 2f;
    private int _damage;
    public int Damage {
        get => _damage;
        private set {
            string logId = "Damage_set";
            if(_damage==value) {
                logd(logId, "Tried to set Damage to same value of "+value+" => returning");
                return;
            }
            if(value<=0) {
                logd(logId,"Tried to set Damage to "+value+" => Setting to "+minDamage+" instead.");
                value = minDamage;
            }
            logd(logId,"Setting Damage from "+_damage+" to "+value);
            _damage = value;
        }
    }
    private float _speed;
    public float Speed {
        get => _speed; 
        private set {
            string logId = "Speed_set";
            if(_speed==value) {
                logd(logId, "Tried to set Speed to same value of "+value+" => returning");
                return;
            }
            if(value<=0) {
                logd(logId,"Tried to set Speed to "+value+" => Setting to "+minSpeed+" instead.");
                value = minSpeed;
            }
            logd(logId,"Setting Speed from "+_speed+" to "+value);
            _speed = value;
        }
    }
    public float collisionCheckDelay = 0.1f;
    private float lastCollisionCheckTime;
    private Transform target;
    public void InitializeProjectile(Transform target, int damage, float speed) {
        string logId = "InitializeProjectile";
        logd(logId, "Initializing Projectile with Damage="+damage+" Speed="+speed);
        Damage = damage;
        Speed = speed;
        SetTarget(target);
    }
    private void Update() {
        string logId = "Update";
        if(!GameManager.Instance.GameStarted) {
            return;
        }
        if(target==null || _damage==0 || _damage<minDamage) {
            logd(logId, "Target="+target.logf()+" Damage="+_damage+" MinDamage="+minDamage+" => Destroying self");
            Destroy(gameObject);
            return;
        }
        float timeSinceLastCollisionCheck = Time.realtimeSinceStartup-lastCollisionCheckTime; 
        if(collisionCheckDelay < timeSinceLastCollisionCheck) {
            logt(logId, "LastCollisionCheckTime="+lastCollisionCheckTime+" TimeSinceLastCollisionCheck="+timeSinceLastCollisionCheck+ " TimeSinceLastCollisionCheck="+timeSinceLastCollisionCheck+" => HandleEnemyCollision");
            HandleEnemyCollision();
        } else {
            logt(logId, "LastCollisionCheckTime="+lastCollisionCheckTime+" TimeSinceLastCollisionCheck="+timeSinceLastCollisionCheck+ " TimeSinceLastCollisionCheck="+timeSinceLastCollisionCheck);
        }
        HandleMovement();
    }
    private void HandleMovement() {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * _speed * Time.deltaTime, Space.World);
    }
    private void HandleEnemyCollision() {
        string logId = "HandleEnemyCollision";
        float distance = (target.position - transform.position).magnitude;
        if(distance < 0.15f) {
            Enemy currentEnemy = target.GetComponent<Enemy>();
            if(currentEnemy==null) {
                logw(logId, "Target doesn't is NOT an Enemy!");
            } else {
                logd(logId, "Target Enemy="+currentEnemy+" damaging with "+_damage);
                currentEnemy.Health.TakeDamage(_damage);
            }
            logd(logId, "Target hit distance="+distance+" => Destroying self");
            Destroy(gameObject);
        } else {
            logt(logId, "Target not hit distance="+distance);
        }
        lastCollisionCheckTime = Time.realtimeSinceStartup;
    }

    public void SetTarget(Transform newTarget) {
        string logId = "SetTarget";
        if(newTarget==null) {
            logw(logId, "NewTarget is null => no-op");
            return;
        }
        if(newTarget==target) {
            logd(logId, "Tried to set target to same target=" + newTarget +" => returning");
            return;
        }
        logd(logId, "Setting target from "+target+" to "+newTarget);
        target = newTarget;
    }
}
