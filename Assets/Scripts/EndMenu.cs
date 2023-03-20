using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : Menu {
    public TextMeshProUGUI endText;
    private void Start() {
        SetStartGameEventListeners();
        SetOnEndGameEventListeners();
        Close();
    }
    private void SetStartGameEventListeners() {
        string logId = "SetStartGameEventListeners";
        logd(logId, "Setting StartGameEvent Listeners");
        GameManager.OnStartGame -= Close;
        GameManager.OnStartGame += Close;
    }
    private void SetOnEndGameEventListeners() {
        string logId = "SetStartGameEventListeners";
        logd(logId, "Setting OnEndGame Listeners");
        GameManager.OnVictory -= SetVictoryUI;
        GameManager.OnVictory += SetVictoryUI;
        GameManager.OnDefeat -= SetDefeatUI;
        GameManager.OnDefeat += SetDefeatUI;
    }
    private void SetVictoryUI() {
        endText.text = "Victory!";
        endText.color = Color.white;
        Open();
    }
    private void SetDefeatUI() {
        endText.text = "Defeat!";
        endText.color = Color.red;
        Open();
    }
    public void OnRestartButtonClick() {
        string logId = "OnRestartButtonClick";
        logd(logId, "InitializeGame.");
        GameManager.Instance.InitializeGame();
    }
}
