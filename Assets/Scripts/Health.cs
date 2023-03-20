using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : NinjaMonoBehaviour {
    [SerializeField]
    private int maxHealth = 50;
    public int MaxHealth => maxHealth;
    private int _currentHealth;
    public int CurrentHealth {
        get => _currentHealth;
        set {
            string logId = "Health_set";
            if(_currentHealth==value) {
                logd(logId, "Tried to set Health to same value of "+value+" => returning");
                return;
            }
            if(value<0) {
                logd(logId, "Tried to set Health below 0 => setting to 0");
                value = 0;
            }
            logd(logId,"Setting Health from "+_currentHealth+" to "+value);
            _currentHealth = value;
        }
    }
    private void Awake() {
        ResetHealth();
    }
    public void ResetHealth() {
        string logId = "ResetHealth";
        logd(logId,"Resetting Health to MaxHealth="+maxHealth);
        _currentHealth = maxHealth;
    }
    
    public virtual void TakeDamage(int damage) {
        string logId = "TakeDamage";
        logd(logId, "Taking damage of "+damage+" while CurrentHealth="+CurrentHealth);
        CurrentHealth -= damage;
        DamageTaken();
        if(_currentHealth<=0) {
            logd(logId, "CurrentHealth<=0 => OnDeath");
            Death();
        }
    }
    public abstract void DamageTaken();
    public abstract void Death();

}
