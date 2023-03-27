using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthGUI : NinjaMonoBehaviour {
    public Image healthAmountImage;
    [SerializeField]
    private Health health;
    private float fillAmount;
    private void Start() {
        health = GetComponentInParent<Health>();
        StartCoroutine(UpdateHealthAmountRoutine());
    }
    private IEnumerator UpdateHealthAmountRoutine() {
        string logId = "UpdateHealthAmountRoutine";
        logd(logId, "Starting "+logId);
        WaitForSeconds waitSeconds = new WaitForSeconds(0.15f);
        while(health!=null) {
            int currentHealth = health.CurrentHealth;
            int maxHealth = health.MaxHealth;
            if(currentHealth>maxHealth) {
                logw(logId, "CurrentHealth="+currentHealth+" > MaxHealth="+maxHealth+" => breaking routine");
                yield break;
            }
            float currentFillAmount = (float)currentHealth/maxHealth;
            if(fillAmount==currentFillAmount) {
                logt(logId, "Tried to update to same value => continuing");
                yield return waitSeconds;
                continue;
            }
            
            logd(logId, "Setting FillAmount from "+fillAmount+" to "+currentFillAmount+" while CurrentHealth="+currentHealth+" MaxHealth="+maxHealth);
            fillAmount = currentFillAmount;    
            healthAmountImage.fillAmount = fillAmount;
            yield return waitSeconds;
        }
        logd(logId, "Health is null => Breaking routine!");
    }
}
