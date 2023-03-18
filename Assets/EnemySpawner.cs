using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : NinjaMonoBehaviour {
    public Path path;
    private int enemiesSpawned = 0;
    private float lastSpawnTime;
    public List<EnemyWave> enemyWaves;
    private int amountOfWavesLeft;
    private int totalWavesToSpawn;
    private EnemyWave currentEnemyWave;
    [System.Serializable]
    public struct EnemyWave {
        public Enemy enemy;
        public int amount;
        public float spawnDelay;
        public float nextWaveDelay;
    }
    private void Awake() {
        string logId = "Start";
        logd(logId, "Registering OnStartGame listeners.");
        path = GameManager.Instance.Path;
        GameManager.OnStartGame -= () => StartEnemySpawn();
        GameManager.OnStartGame += () => StartEnemySpawn();
        GameManager.OnEndGame -= () => SpawningEnemies = false;
        GameManager.OnEndGame += () => SpawningEnemies = false;
    }

    private bool _spawningEnemies;
    public bool SpawningEnemies {
        get => _spawningEnemies;
        private set {
            string logId  = "SpawningEnemies_set";
            if(value==_spawningEnemies) {
                logd(logId, "Trying to set SpawningEnemies to same value of "+value+" => Returning");
                return;
            }
            logd(logId, "Setting SpawningEnemies from "+_spawningEnemies+" to "+value);
            _spawningEnemies = value;

        }
    }
    private void StartEnemySpawn() {
        string logId = "StartEnemySpawn";
        path = GameManager.Instance.Path;
        if(path==null) {
            logw(logId, "Path is null => no-op");
            return;
        }
        totalWavesToSpawn = enemyWaves.Count;
        amountOfWavesLeft = totalWavesToSpawn;
        if(totalWavesToSpawn<=0) {
            logw(logId, "Not enough waves to start.");
            return;
        }
        currentEnemyWave = enemyWaves[0];
        logd(logId, "Starting with "+totalWavesToSpawn+" waves.");
        if(!SpawningEnemies) {
            StartCoroutine(EnemySpawningRoutine());
        }
    }
    private IEnumerator EnemySpawningRoutine() {
        string logId = "EnemySpawningRoutine";
        int enemiesSpawnedOnWave = 0;
        logd(logId, "Starting "+logId);
        SpawningEnemies = true;
        while(SpawningEnemies) {
            int waveEnemiesSpawnAmount = currentEnemyWave.amount;
            if(enemiesSpawnedOnWave<=waveEnemiesSpawnAmount) {
                enemiesSpawnedOnWave++;
                Enemy currentEnemy = currentEnemyWave.enemy;
                logd("Spawning Enemy="+currentEnemy+" Number="+enemiesSpawnedOnWave+" for Wave="+amountOfWavesLeft);
                SpawnEnemy(currentEnemy);
                yield return new WaitForSeconds(1f);
            }
            if(enemiesSpawnedOnWave==waveEnemiesSpawnAmount) {
                amountOfWavesLeft--;
                enemiesSpawnedOnWave = 0;
                if(amountOfWavesLeft<0) {
                    logd("There are no more waves to spawn. Victory!");
                    //OnAllEnemyWavesSpawned.Invoke(); 
                    SpawningEnemies = false;
                    yield break;
                }
                yield return new WaitForSeconds(1f);
                EnemyWave nextEnemyWave = enemyWaves[totalWavesToSpawn-amountOfWavesLeft];
                logd("Changing Wave from "+currentEnemyWave+" to "+nextEnemyWave+" while AmountOfWavesLeft="+amountOfWavesLeft);
                currentEnemyWave = nextEnemyWave;
                continue;
            }
        }
        logd(logId, "RunningSpawn is false => breaking routine");
        SpawningEnemies = false;
    }
    private void SpawnEnemy(Enemy enemy) {
        string logId = "SpawnEnemy";
        if(path==null || enemy==null) {
            logw(logId, "Path="+path.logf()+" Enemy="+enemy+" => no-op");
            return;
        }
        Transform firstWaypoint = path.waypoints[0];
        Enemy newEnemy = Instantiate(enemy, firstWaypoint.position, Quaternion.identity);
        newEnemy.SetPath(path);
        enemiesSpawned++;
        lastSpawnTime = Time.realtimeSinceStartup;
    }
}
