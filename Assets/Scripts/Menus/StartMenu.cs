using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : Menu {

    private void Start() {
        SetStartGameEventListeners();
    }
    private void SetStartGameEventListeners() {
        string logId = "SetStartGameEventListeners";
        logd(logId, "Setting StartGameEvent Listeners");
        GameManager.OnStartGame -= () => Close();
        GameManager.OnStartGame += () => Close();
    }
    public void OnStartButtonClick() {
        string logId = "OnStartButtonClick";
        logd(logId, "StartGame");
        GameManager.Instance.StartGame();
    }
}
