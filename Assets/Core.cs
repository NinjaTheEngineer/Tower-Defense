using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : NinjaMonoBehaviour {
    public static System.Action<int> OnHealthAmountChange;
    public static System.Action OnCoreDestroyed;
    [SerializeField]
    private Health _health;
    public Health Health {
        get => _health;
    }
    private void Awake() {
        _health = _health??GetComponent<Health>();
    }
    public void TakeDamage(int damage) {
        string logId = "TakeDamage";
        if(_health==null) {
            logd(logId, "Health component is missing => no-op");
            return;
        }
        logd(logId, "Taking damage of "+damage);
        _health.CurrentHealth -= damage;
        int currentHealth = _health.CurrentHealth;
        if(OnHealthAmountChange!=null) {
            logd(logId, "Invoking OnHealthAmountChange with CurrentHealth="+currentHealth);
            OnHealthAmountChange.Invoke(currentHealth);
        }
        if(currentHealth<=0) {
            if(OnCoreDestroyed==null) {
                logw(logId, "No listeneres registered for OnCoreDestroyed event => no-op");
            } else {
                logd(logId, "Core lost all health => Invoking OnCoreDestroyed and disabling self.");
                OnCoreDestroyed.Invoke();
            }
            gameObject.SetActive(false);
        }
    }
    public void Restart() {
        string logId = "Restart";
        logd(logId, "Restarting Core => Activating and resetting health.");
        gameObject.SetActive(true);
        _health.Reset();
        OnHealthAmountChange.Invoke(_health.CurrentHealth);
    }
}
