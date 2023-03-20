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
    private int enemiesDead = 0;
    private int totalEnemiesToSpawn;
    private EnemyWave currentEnemyWave;
    private List<Enemy> enemies = new List<Enemy>();
    [System.Serializable]
    public struct EnemyWave {
        public Enemy enemy;
        public int amount;
        public float spawnDelay;
        public float nextWaveDelay;
    }
    private void Awake() {
        string logId = "Awake";
        logd(logId, "Registering OnStartGame listeners.");
        path = GameManager.Instance.Path;
        RegisterListeners();
    }
    private void RegisterListeners() {
        string logId = "RegisterListeners";
        logd(logId, " => Registering OnStartGame and OnDefeat events.");
        GameManager.OnStartGame += StartEnemySpawn;
        GameManager.OnDefeat += StopSpawningEnemies;
    }
    private void RemoveListeners() {
        string logId = "OnDestroy";
        logd(logId, " => Removing OnStartGame and OnDefeat events.");
        GameManager.OnStartGame -= StartEnemySpawn;
        GameManager.OnDefeat -= StopSpawningEnemies;
    }
    private void StopSpawningEnemies() => SpawningEnemies = false;
    private void OnDestroy() {
        string logId = "OnDestroy";
        logd(logId, " => Remove Listeners");
        RemoveListeners();
    }
    private void Reset() {
        string logId = "Reset";
        enemiesDead = 0;
        enemiesSpawned = 0;
        totalWavesToSpawn = enemyWaves.Count;
        amountOfWavesLeft = totalWavesToSpawn;
        for (int i = 0; i < totalWavesToSpawn; i++) {
            totalEnemiesToSpawn+=enemyWaves[i].amount;
        }
        enemies = new List<Enemy>();
    }

    private bool _spawningEnemies;
    public bool SpawningEnemies {
        get => _spawningEnemies;
        private set {
            string logId = "SpawningEnemies_set";
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
        Reset();
        if(path==null) {
            logw(logId, "Path is null => no-op");
            return;
        }
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
                logd(logId, "Spawning Enemy="+currentEnemy+" Number="+enemiesSpawnedOnWave+" for Wave="+amountOfWavesLeft);
                SpawnEnemy(currentEnemy);
                yield return new WaitForSeconds(currentEnemyWave.spawnDelay);
            }
            if(enemiesSpawnedOnWave==waveEnemiesSpawnAmount) {
                amountOfWavesLeft--;
                enemiesSpawnedOnWave = 0;
                if(amountOfWavesLeft<=0) {
                    logd(logId, "There are no more waves to spawn. Victory!");
                    //OnAllEnemyWavesSpawned.Invoke(); 
                    SpawningEnemies = false;
                    yield break;
                }
                yield return new WaitForSeconds(currentEnemyWave.nextWaveDelay);
                EnemyWave nextEnemyWave = enemyWaves[totalWavesToSpawn-amountOfWavesLeft];
                logd(logId, "Changing Wave from "+currentEnemyWave+" to "+nextEnemyWave+" while AmountOfWavesLeft="+amountOfWavesLeft);
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
        newEnemy.transform.parent = transform;
        newEnemy.OnDeath -= OnEnemyDeath;
        newEnemy.OnDeath += OnEnemyDeath;
        newEnemy.SetPath(path);
        enemiesSpawned++;
        enemies.Add(newEnemy);
        lastSpawnTime = Time.realtimeSinceStartup;
    }
    private void OnEnemyDeath(Enemy enemy) {
        string logId = "OnEnemyDeath";
        enemiesDead++;
        logd(logId,"EnemiesDead="+enemiesDead+" TotalEnemiesToSpawn="+totalEnemiesToSpawn);
        if(enemiesDead>=totalEnemiesToSpawn) {
            logd(logId," => GameWon");
            GameManager.Instance.GameWon();
        }
    }
}
