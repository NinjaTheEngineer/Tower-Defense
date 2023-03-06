using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : NinjaMonoBehaviour {
    public Path path;
    public Enemy enemyPrefab;
    public int maxEnemiesToSpawn = 10;
    private int enemiesSpawned = 0;
    public float enemySpawnDelay = 1f;
    private float lastSpawnTime;

    private void Update() {
        if(!GameManager.Instance.GameStarted) {
            return;
        } 

        float timeSinceLastSpawn = Time.realtimeSinceStartup-lastSpawnTime; 
        if(enemiesSpawned < maxEnemiesToSpawn && enemySpawnDelay < timeSinceLastSpawn) {
            SpawnEnemy();
        }
    }
    private void SpawnEnemy() {
        string logId = "SpawnEnemy";
        if(path==null || enemyPrefab==null) {
            logd(logId, "Path="+path.logf()+" EnemyPrefab="+enemyPrefab+" => no-op");
            return;
        }
        Transform firstWaypoint = path.waypoints[0];
        Enemy newEnemy = Instantiate(enemyPrefab, firstWaypoint.position, Quaternion.identity);
        newEnemy.SetPath(path);
        enemiesSpawned++;
        lastSpawnTime = Time.realtimeSinceStartup;
    }
}
