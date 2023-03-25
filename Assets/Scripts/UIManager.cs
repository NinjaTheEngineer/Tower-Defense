using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : NinjaMonoBehaviour {
    public GameObject towersUI;
    public GameObject goldUI;
    public TextMeshProUGUI goldAmountText;
    public GameObject coreHealthUI;
    public TextMeshProUGUI coreHealthAmountText;
    private void Awake() {
        HideInGameUI();
    }
    private void Start() {
        SetOnStartGameEventListeners();
        SetOnGoldEarnedEventListeners();
        SetOnCoreHealthChangeEventListeners();
        UpdateGoldAmountText();
    }
    private void SetOnStartGameEventListeners() {
        string logId = "SetOnStartGameEventListeners";
        logd(logId, "Setting StartGameEvent Listeners");
        GameManager.OnStartGame -= ShowInGameUI;
        GameManager.OnStartGame += ShowInGameUI;
    }
    private void SetOnGoldEarnedEventListeners() {
        string logId = "SetOnGoldEarnedEventListeners";
        logd(logId, "Setting OnGoldUpdated Listeners");
        ResourcesManager.OnGoldUpdated -= UpdateGoldAmountText;
        ResourcesManager.OnGoldUpdated += UpdateGoldAmountText;
    }
    private void SetOnCoreHealthChangeEventListeners() {
        string logId = "SetOnCoreHealthChangeEventListeners";
        logd(logId, "Setting StartGameEvent Listeners");
        Core.OnHealthChange -= UpdateCoreHealthAmountText;
        Core.OnHealthChange += UpdateCoreHealthAmountText;
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
        if(coreHealthUI==null) {
            loge(logId, "CoreHealthUI is null => Can't enable CoreHealthUI.");
        } else {
            coreHealthUI.SetActive(true);
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
        if(coreHealthUI==null) {
            loge(logId, "CoreHealthUI is null => Can't disable CoreHealthUI.");
        } else {
            coreHealthUI.SetActive(false);
        }
    }
    private void UpdateGoldAmountText(int goldAmount = -1) {
        string logId = "UpdateGoldAmountText";
        if(goldAmountText==null) {
            loge(logId,"GoldAmounText is null => no-op");
            return;
        }
        if(goldAmount<=0) {
            logd(logId, "GoldAmount="+goldAmount+" => Fetching from ResourceManager");
            goldAmount = ResourcesManager.Instance.CurrentGoldAmount;
        }
        logt(logId,"Setting GoldAmountText to " + goldAmount);
        goldAmountText.text = goldAmount.ToString();
    }
    private void UpdateCoreHealthAmountText(int healthAmount) {
        string logId = "UpdateCoreHealthAmountText";
        if(coreHealthAmountText==null) {
            loge(logId,"CoreHealthAmountText is null => no-op");
            return;
        }
        logt(logId,"Setting CoreHealthAmountText to " + healthAmount);
        coreHealthAmountText.text = healthAmount.ToString();
    }
}
