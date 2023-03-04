using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : NinjaMonoBehaviour {
    public GameObject towersUI;
    public GameObject goldUI;
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
        GameManager.OnStartGame -= () => ShowInGameUI();
        GameManager.OnStartGame += () => ShowInGameUI();
    }
    private void SetOnGoldEarnedEventListeners() {
        string logId = "SetOnGoldEarnedEventListeners";
        logd(logId, "Setting StartGameEvent Listeners");
        ResourcesManager.OnGoldUpdated -= () => UpdateGoldAmountText();
        ResourcesManager.OnGoldUpdated += () => UpdateGoldAmountText();
    }
    private void ShowInGameUI() {
        string logId = "ShowInGameUI";
        if(towersUI==null) {
            loge(logId, "TowersUI is null => Can't enable TowersUI.");
        } else {
            towersUI.SetActive(true);
        }
        if(goldUI==null) {
            loge(logId, "GoldUI is null => Can't enable GoldUI.");
        } else {
            goldUI.SetActive(true);
        }
    }
    private void HideInGameUI() {
        string logId = "HideInGameUI";
        if(towersUI==null) {
            loge(logId, "TowersUI is null => Can't disable TowersUI.");
        } else {
            towersUI.SetActive(false);
        }
        if(goldUI==null) {
            loge(logId, "GoldUI is null => Can't disable GoldUI.");
        } else {
            goldUI.SetActive(false);
        }
    }
    private void UpdateGoldAmountText() {
        string logId = "UpdateGoldAmountText";
        if(goldAmountText==null) {
            loge(logId,"GoldAmounText is null => no-op");
            return;
        }
        int amountOfGold = ResourcesManager.Instance.CurrentGoldAmount;
        logd(logId,"Setting GoldAmountText to " + amountOfGold);
        goldAmountText.text = amountOfGold.ToString();
    }
}
