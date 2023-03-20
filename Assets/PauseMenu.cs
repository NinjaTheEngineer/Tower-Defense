using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu {
    private void Start() {
        GameManager.OnPauseGame -= Open;
        GameManager.OnPauseGame += Open;
        GameManager.OnResumeGame -= Close;
        GameManager.OnResumeGame += Close;
        Close();
    }
    private void SetStartGameEventListeners() {
        GameManager.OnStartGame -= Close;
        GameManager.OnStartGame += Close;
    }
    public void OnResumeButtonClick() {
        string logId = "OnResumeButtonClick";
        logd(logId, "ResumeGame");
        GameManager.Instance.ResumeGame();
    }
}
