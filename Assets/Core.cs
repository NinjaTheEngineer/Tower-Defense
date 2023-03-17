using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : NinjaMonoBehaviour {
    public static System.Action<int> OnHealthAmountChange;
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
            logd(logId, "Enemy is dead => Destroying self");
            Destroy(gameObject);
        }
    }
}
