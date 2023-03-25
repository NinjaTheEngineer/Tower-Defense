using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularIndicator : NinjaMonoBehaviour {
    [SerializeField]
    private Renderer imageRenderer;
    [SerializeField]
    private Color primaryColor;
    [SerializeField]
    private Color secondaryColor;
    private Transform _followTarget;
    private Vector3 _originalScale;
    public Vector3 OriginalScale {
        get => _originalScale;
        private set {
            string logId = "OriginalScale";
            if(_originalScale==value) {
                logd(logId, "Tried to set scale to same value of "+value);
                return;
            }
            logd(logId, "Setting OriginalScale from "+_originalScale+" to "+value);
            _originalScale = value;
        }
    }
    public Transform FollowTarget {
        get => _followTarget;
        set {
            string logId = "FollowTarget_set";
            if(_followTarget==value) {
                logt(logId,"Tried to set FollowTarget to same value of "+value.logf()+" => returning");
                return;
            }
            logd(logId,"Setting FollowTarget from "+_followTarget.logf()+" to "+value.logf());
            _followTarget = value;
        }
    }
    private void Awake() {
        OriginalScale = transform.localScale;
        Deactivate();
    }
    public void Activate() {
        string logId = "Activate";
        gameObject.SetActive(true);
        logd(logId, "Activating "+name);
        SetPrimaryColor();
    }
    public void Deactivate() {
        string logId = "Deactivate";
        FollowTarget = null;
        logd(logId, "Deactivating "+name);
        gameObject.SetActive(false);    
    }
    public void SetColor(Color color) {
        string logId = "SetColor";
        if(imageRenderer==null) {
            logw(logId, "Renderer="+imageRenderer.logf()+" => no-op");
            return;
        }
        imageRenderer.material.color = color;    
    }
    public void SetPrimaryColor() => SetColor(primaryColor);
    public void SetSecondaryColor() => SetColor(secondaryColor);
    public void SetSize(float size) {
        string logId = "SetSize";
        transform.localScale = _originalScale;
        Vector3 localScale = transform.localScale;
        var newScale = localScale*size*2f;
        logd(logId, "Setting localScale from "+transform.localScale+" to "+newScale+" with size="+size);
        transform.localScale = newScale;
    }
    private void Update() {
        if(_followTarget) {
            transform.position = _followTarget.transform.position;
        }
    }
}
