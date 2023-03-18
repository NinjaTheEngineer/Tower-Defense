using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : Menu {
    private void Start() {
        SetOnEndGameEventListeners();
        Close();
    }
    private void SetOnEndGameEventListeners() {
        string logId = "SetStartGameEventListeners";
        logd(logId, "Setting OnEndGame Listeners");
        GameManager.OnEndGame -= () => Open();
        GameManager.OnEndGame += () => Open();
    }
}
