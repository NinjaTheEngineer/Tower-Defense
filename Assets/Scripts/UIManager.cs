using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : NinjaMonoBehaviour {
    public GameObject towersUI;
    public TextMeshProUGUI goldAmountText;
    private void Awake() {
        HideInGameUI();
    }
    private void Start() {
        SetOnStartGameEventListeners();
        SetOnGoldEarnedEventListeners();
        UpdateGoldAmountText();
    }
    private void SetOnStartGameEventListeners() {
        string logId = "SetOnStartGameEventListeners";
        logd(logId, "Setting StartGameEvent Listeners");
        GameManager.OnGameStart -= () => ShowInGameUI();
        GameManager.OnGameStart += () => ShowInGameUI();
    }
    private void SetOnGoldEarnedEventListeners() {
        string logId = "SetOnGoldEarnedEventListeners";
        logd(logId, "Setting StartGameEvent Listeners");
        ResourcesManager.OnGoldEarned -= () => UpdateGoldAmountText();
        ResourcesManager.OnGoldEarned += () => UpdateGoldAmountText();
    }
    private void ShowInGameUI() {
        string logId = "ShowInGameUI";
        if(towersUI==null) {
            loge(logId, "TowersUI is null => no-op");
        } else {
            towersUI.SetActive(true);
        }
    }
    private void HideInGameUI() {
        string logId = "HideInGameUI";
        if(towersUI==null) {
            loge(logId, "TowersUI is null => no-op");
        } else {
            towersUI.SetActive(false);
        }
    }
    private void UpdateGoldAmountText() {
        string logId = "UpdateGoldAmountText";
        if(goldAmountText==null) {
            loge(logId,"GoldAmounText is null => no-op");
            return;
        }
        int amountOfGold = ResourcesManager.Instance.CurrentGoldAmount;
        logd(logId,"Setting GoldAmounText to " + amountOfGold);
        goldAmountText.text = amountOfGold.ToString();
    }
}
