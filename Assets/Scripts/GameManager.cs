using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NinjaMonoBehaviour {
    public GameState currentState;
    public enum GameState {
        None,
        Started,
        Paused,
        Finished
    }
    public bool GameStarted => currentState==GameState.Started;
    public static GameManager Instance;
    public static System.Action OnGameStart;

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
        logd(logId, "Invoking StartGameEvent");
        OnGameStart.Invoke();
        currentState = GameState.Started;
    }

    private void Update() {
        string logId = "Update";
        if(currentState!=GameState.Started) {
            return;
        }
    }
}
