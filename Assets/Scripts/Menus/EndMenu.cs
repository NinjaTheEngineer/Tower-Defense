using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndMenu : Menu {
    public TextMeshProUGUI endText;
    public string victoryText = "Victory!";
    public string defeatText = "Defeat!";
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
        string logId = "SetVictoryUI";
        logd(logId, "Setting text to '"+victoryText+"' of color White => Opening Menu");
        Open();
        endText.SetText(victoryText);
        endText.color = Color.white;
    }
    private void SetDefeatUI() {
        string logId = "SetDefeatUI";
        logd(logId, "Setting text to '"+defeatText+"' of color Red => Opening Menu");
        endText.SetText(defeatText);
        endText.color = Color.red;
        Open();
    }
    public void OnRestartButtonClick() {
        string logId = "OnRestartButtonClick";
        logd(logId, "InitializeGame.");
        GameManager.Instance.InitializeGame();
    }
}
