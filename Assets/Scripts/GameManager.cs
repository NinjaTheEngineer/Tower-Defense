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
    public static System.Action OnEndGame;
    [SerializeField]
    private Path _path;
    public Path Path => _path;
    [SerializeField]
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

    public void StartGame() {
        string logId = "StartGame";
        if(OnStartGame==null) {
            logw(logId, "No listeneres registered for OnStartGame event => no-op");
            return;
        }
        if(_path && enemySpawner) {
            logd(logId, "Instantiating EnemySpawner.");
            enemySpawner = Instantiate(enemySpawner);
        } else {
            logd(logId, "Path="+_path+" EnemySpawner="+enemySpawner.logf()+" => no-op");
        }
        Core.OnCoreDestroyed -= () => EndGame();
        Core.OnCoreDestroyed += () => EndGame();
        logd(logId, "Invoke OnStartGame");
        OnStartGame.Invoke();
        currentState = GameState.Started;
    }
    public void EndGame() {
        string logId = "EndGame";
        currentState = GameState.Ended;
        if(OnEndGame==null) {
            logw(logId, "No listeneres registered for OnEndGame event => no-op");
            return;
        }
        logd(logId, "Invoke OnEndGame");
        OnEndGame.Invoke();
    }
    private void Update() {
        //string logId = "Update";
        if(currentState!=GameState.Started) {
            return;
        }
    }
}
