using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : Health {
    public static System.Action<int> OnHealthAmountChange;
    public static System.Action OnCoreDestroyed;
    public override void DamageTaken() { 
        string logId = "DamageTaken";
        if(OnHealthAmountChange!=null) {
            int currentHealth = CurrentHealth;
            logd(logId, "Invoking OnHealthAmountChange with CurrentHealth="+currentHealth);
            OnHealthAmountChange.Invoke(currentHealth);
        }
    }
    public override void Death() {
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
        ResetHealth();
        OnHealthAmountChange.Invoke(CurrentHealth);
    }
}
