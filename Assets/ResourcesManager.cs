using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : NinjaMonoBehaviour {
    public int maxGoldAmount;
    public float goldEarnTime;
    private float lastGoldEarnTime = 0;
    private int _currentGoldAmount;
    public int CurrentGoldAmount {
        get => _currentGoldAmount;
        private set {
            string logId = "CurrentGoldAmount_set";
            if(value==_currentGoldAmount){
                logd(logId, "Tried to set CurrentGoldAmount to same value of " + value + "=> returning");
                return;
            }
            _currentGoldAmount = value;
            InvokeOnGoldUpdated();
        }
    } 
    public static System.Action OnGoldUpdated;
    public static ResourcesManager Instance;
    private void Awake() {
        string logId = "Awake";
        if(Instance==null) {
            logd(logId, "Setting Singleton Instance to GameObject.");
            Instance = this;
        } else {
            logw(logId, "Singleton Instance already set => Destroying this GameObject.");
            Destroy(gameObject);
        }
    }
    private void Update() {
        if(GameManager.Instance.GameStarted) {
            if(CurrentGoldAmount < maxGoldAmount) {
                HandleGoldGeneration();
            }
        }
    }
    public bool SpendGold(int goldToSpend) {
        string logId = "SpendGold";
        if(goldToSpend<=0 || goldToSpend>CurrentGoldAmount) {
            logd(logId, "GoldToSpend="+goldToSpend+" CurrentGoldAmount="+CurrentGoldAmount+" => no-op");
            return false;
        }
        CurrentGoldAmount-= goldToSpend;
        return true;
    }
    private void HandleGoldGeneration() {
        string logId = "HandleGoldGeneration";
        float timeSinceGoldEarned = Time.realtimeSinceStartup-lastGoldEarnTime; 
        if(goldEarnTime < timeSinceGoldEarned) {
            CurrentGoldAmount += 1;
            logt(logId, "TimeSinceGoldEarned="+timeSinceGoldEarned+ " CurrentGoldAmount="+CurrentGoldAmount+" => Invoke OnGoldEarned");
            lastGoldEarnTime = Time.realtimeSinceStartup;
        } else {
            logt(logId, "LastGoldEarnTime="+lastGoldEarnTime+" TimeSinceGoldEarned="+timeSinceGoldEarned+ " CurrentGoldAmount="+CurrentGoldAmount+" => Not generating gold");
        }
    }
    private void InvokeOnGoldUpdated() {
        string logId = "InvokeOnGoldUpdated";
        if(OnGoldUpdated==null) {
            logw(logId, "No listeneres registered for OnGameStart event => no-op");
            return;
        }
        OnGoldUpdated.Invoke();
    }
}
