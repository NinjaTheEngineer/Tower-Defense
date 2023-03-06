using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : NinjaMonoBehaviour {
    public float speed = 10f;
    public int damage = 10;
    public float collisionCheckDelay = 0.1f;
    private float lastCollisionCheckTime;
    private Transform target;
    private void Update() {
        string logId = "Update";
        if(!GameManager.Instance.GameStarted) {
            return;
        }
        if(target==null) {
            logd(logId, "Target is null => Destroying self");
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
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
    }
    private void HandleEnemyCollision() {
        string logId = "HandleEnemyCollision";
        float distance = (target.position - transform.position).magnitude;
        if(distance < 0.15f) {
            Enemy currentEnemy = target.GetComponent<Enemy>();
            if(currentEnemy==null) {
                logw(logId, "Target doesn't is NOT an Enemy!");
            } else {
                currentEnemy.TakeDamage(damage);
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
