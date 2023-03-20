using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Core : NinjaMonoBehaviour {
    public static System.Action<int> OnHealthChange;
    public static System.Action OnCoreDestroyed;
    [SerializeField]
    private Health _health;
    public Health Health {
        get => _health; 
        private set {
            string logId = "Health_set";
            if(_health==value) {
                logd(logId, "Tried to set Health to same value of "+value.logf());
                return;
            }
            logd(logId, "Setting Health from "+_health+" to "+value);
            _health=value;
        }
    }
    private void Start() {
        if(!_health) {
            _health = GetComponent<Health>();
        }
        _health.OnDamageTaken -= DamageTaken;
        _health.OnDamageTaken += DamageTaken;
    }
    public void DamageTaken() { 
        string logId = "DamageTaken";
        if(OnHealthChange!=null) {
            int currentHealth = _health.CurrentHealth;
            logd(logId, "Invoking OnHealthAmountChange with CurrentHealth="+currentHealth);
            OnHealthChange.Invoke(currentHealth);
            if(currentHealth<=0) {
                Death();
            }
        }
    }
    public void Death() {
        string logId = "Death";
        if(OnCoreDestroyed==null) {
            logw(logId, "No listeneres registered for OnCoreDestroyed event => no-op");
        } else {
            logd(logId, "Core lost all health => Invoking OnCoreDestroyed and disabling self.");
            OnCoreDestroyed.Invoke();
        }
        gameObject.SetActive(false);
    }
    public void Restart() {
        string logId = "Restart";
        logd(logId, "Restarting Core => Activating GameObject and Resetting Health.");
        gameObject.SetActive(true);
        if(_health) {
            logd(logId, "Health="+_health.logf()+" => Resetting Health and Invoking HealthChange");
            _health.ResetHealth();
            OnHealthChange.Invoke(_health.CurrentHealth);
        } else {
            logd(logId, "Health="+_health.logf()+" => No health component found.");
        }
    }
}
