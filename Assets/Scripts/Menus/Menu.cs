using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : NinjaMonoBehaviour {
    
    public virtual void Open() {
        string logId = "Open";
        logd(logId, "Setting TimeScale to 0 and activating "+name);
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public virtual void Close() {
        string logId = "Close";
        logd(logId, "Setting TimeScale to 1 and deactivating "+name);
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
