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
    public static System.Action OnStartGame;

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
        currentState = GameState.Started;
        InvokeOnStartGame();
    }
    private void InvokeOnStartGame() {
        string logId = "InvokeOnStartGame";
        if(OnStartGame==null) {
            logw(logId, "No listeneres registered for OnStartGame event => no-op");
            return;
        }
        logd(logId, "Invoking OnStartGame");
        OnStartGame.Invoke();
    }
    private void Update() {
        //string logId = "Update";
        if(currentState!=GameState.Started) {
            return;
        }
    }
}
