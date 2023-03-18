using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : NinjaMonoBehaviour {

    [SerializeField]
    private int maxHealth = 50;
    public int MaxHealth => maxHealth;
    private int _currentHealth;
    private void Start() {
        _currentHealth = maxHealth;
    }
    public int CurrentHealth {
        get => _currentHealth;
        set {
            string logId = "Health_set";
            if(_currentHealth==value) {
                logd(logId, "Tried to set Health to same value of "+value+" => returning");
                return;
            }
            if(value < 0) {
                logd(logId, "Tried to set Health below 0 => setting to 0");
                value = 0;
            }
            logd(logId,"Setting Health from "+_currentHealth+" to "+value);
            _currentHealth = value;
        }
    }

}
