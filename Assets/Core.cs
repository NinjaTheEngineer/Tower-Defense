using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : NinjaMonoBehaviour {
    public int maxHealth;    
    private int _currentHealth;
    public static System.Action<int> OnHealthAmountChange;
    public int CurrentHealth {
        get => _currentHealth;
        private set {
            string logId = "CurrentHealth_set";
            if(value==_currentHealth) {
                logd(logId, "CurrentHealth is already " + value + " => returning");
                return;
            }
            if(value<0) {
                value = 0;
            }
            logd(logId, "Setting CurrentHealth from " + _currentHealth + " to " + value);
            _currentHealth = value;
            if(OnHealthAmountChange!=null) {
                OnHealthAmountChange.Invoke(_currentHealth);
            }
        }
    }
    private void Awake() {
        CurrentHealth = maxHealth;
    }

    public void Damage(int damageAmount) {
        string logId = "Damage";
        if(damageAmount<0) {
            logd(logId, "DamageAmount="+damageAmount+" => returning");
            return;
        }
        logd(logId, "Damaging core with "+damageAmount+" damage");
        CurrentHealth -= damageAmount;
        if(CurrentHealth<=0) {
            logd(logId, "Core destroyed => End game");
            Destroy(gameObject);
        }
    }
}
