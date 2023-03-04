using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : NinjaMonoBehaviour {
    public int maxGoldAmount;
    public float goldEarnTime;
    private float goldLastEarnTime = 0;
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
        }
    } 
    public static System.Action OnGoldEarned;
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

    private void HandleGoldGeneration() {
        string logId = "HandleGoldGeneration";
        float timeSinceGoldEarned = Time.realtimeSinceStartup-goldLastEarnTime; 
        if(goldEarnTime < timeSinceGoldEarned) {
            CurrentGoldAmount += 1;
            logd(logId, "TimeSinceGoldEarned="+timeSinceGoldEarned+ " CurrentGoldAmount="+CurrentGoldAmount+" => Invoke OnGoldEarned");
            OnGoldEarned.Invoke();
            goldLastEarnTime = Time.realtimeSinceStartup;
        } else {
            logt(logId, "GoldLastEarnTime="+goldLastEarnTime+" TimeSinceGoldEarned="+timeSinceGoldEarned+ " CurrentGoldAmount="+CurrentGoldAmount+" => Not generating gold");
        }
    }
}
