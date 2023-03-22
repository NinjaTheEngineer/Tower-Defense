using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NinjaMonoBehaviour {
    public GameState currentState;
    public enum GameState {
        None,
        Started,
        Paused,
        Ended
    }
    public bool GameStarted => currentState==GameState.Started;
    public static GameManager Instance;
    public static System.Action OnStartGame;
    public static System.Action OnPauseGame;
    public static System.Action OnResumeGame;
    public static System.Action OnVictory;
    public static System.Action OnDefeat;
    [SerializeField]
    private Path _path;
    public Path Path => _path;
    [SerializeField]
    private EnemySpawner enemySpawnerPrefab;
    private EnemySpawner enemySpawner;

    private void Awake() {
        string logId = "Awake";
        if(Instance==null) {
            logd(logId, "Setting Singleton Instance to GameObject.");
            Instance = this;
        } else {
            logw(logId, "Singleton Instance already set => Destroying this GameObject.");
            Destroy(gameObject);
        }
        currentState = GameState.None;
    }
    public void InitializeGame() {
        string logId = "InitializeGame";
        if(OnStartGame==null) {
            logw(logId, "No listeneres registered for OnStartGame event => no-op");
            return;
        }
        if(enemySpawner) {
            Destroy(enemySpawner);
        }
        if(_path && enemySpawnerPrefab) {
            logd(logId, "Instantiating EnemySpawner.");
            enemySpawner = Instantiate(enemySpawnerPrefab);
            enemySpawner.name = "EnemySpawner_"+Time.deltaTime;    
        } else {
            logd(logId, "Path="+_path+" EnemySpawner="+enemySpawner.logf()+" => no-op");
        }
        _path.Core.Restart();
        Core.OnCoreDestroyed -= GameLost;
        Core.OnCoreDestroyed += GameLost;
        logd(logId, "Invoke OnStartGame");
        OnStartGame.Invoke();
        currentState = GameState.Started;
    }
    public void ResumeGame() {
        string logId = "ResumeGame";
        Time.timeScale = 1f;
        currentState = GameState.Started;
        if(OnResumeGame==null) {
            logw(logId, "No listeneres registered for OnResumeGame event => no-op");
            return;
        }
        logd(logId, "Invoke OnResumeGame");
        OnResumeGame.Invoke();
    }
    public void PauseGame() {
        string logId = "PauseGame";
        Time.timeScale = 0f;
        currentState = GameState.Paused;
        if(OnResumeGame==null) {
            logw(logId, "No listeneres registered for OnResumeGame event => no-op");
            return;
        }
        logd(logId, "Invoke OnResumeGame");
        OnPauseGame.Invoke();
    }
    public void GameWon() {
        string logId = "GameWon";
        OnGameEnded();
        if(OnVictory==null) {
            logw(logId, "No listeneres registered for OnVictory event => no-op");
            return;
        }
        logd(logId, "Invoke OnVictory");
        OnVictory.Invoke();
    }
    public void GameLost() {
        string logId = "EndGame";
        OnGameEnded();
        if(OnDefeat==null) {
            logw(logId, "No listeneres registered for OnDefeat event => no-op");
            return;
        }
        logd(logId, "Invoke OnDefeat");
        OnDefeat.Invoke(); 
    }
    private void OnGameEnded() {
        string logId = "OnGameEnded";
        logd(logId, "Destroy EnemySpawner="+enemySpawner.logf()+" => Set CurrentState to Ended.");
        currentState = GameState.Ended;
        Destroy(enemySpawner.gameObject);
    }
    
    private void Update() {
        if(currentState==GameState.Started) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                PauseGame();
            }
            return;
        } else if(currentState==GameState.Paused) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                ResumeGame();
            }
        }
    }
}
