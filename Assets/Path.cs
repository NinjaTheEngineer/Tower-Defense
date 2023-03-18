using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : NinjaMonoBehaviour {
    public List<Transform> waypoints;
    [SerializeField]
    private Core _core;
    public Core Core {
        get {
            string logId = "Core_get";
            if(_core!=null) {
                logd(logId, "Returning "+_core.name+".");
                return _core;
            }
            logd(logId, "Core is null => searching for core.");
            int waypointsCount = waypoints.Count;
            if(waypointsCount<=0) {
                logw(logId, "Tried to get next waypoint while waypointsCount="+waypointsCount+"=> returning null");
                return null;
            }
            Transform coreWaypoint = waypoints[waypointsCount-1];
            _core = coreWaypoint.GetComponent<Core>();
            if(_core==null) {
                logd(logId, "Core not found on last waypoint => Searching in all waypoints.");
                for (int i = 0; i < waypointsCount; i++) {
                    coreWaypoint = waypoints[i];
                    _core = coreWaypoint.GetComponent<Core>();
                    if(_core!=null) {
                        logd(logId, "Core found at "+coreWaypoint+" => breaking search.");
                        break;
                    }
                }
            }
            logd(logId, "Returning "+coreWaypoint.name+" as Core");
            return _core;
        }
    }
    private void Start() {
        FetchWaypoints();
    }

    private void FetchWaypoints() {
        string logId = "FetchWaypoints";
        Transform[] foundTransforms = GetComponentsInChildren<Transform>();
        int foundTransformsCount = foundTransforms.Length;
        if(foundTransformsCount<=0) {
            logw(logId, "No transforms found");
            return;
        }
        for (int i = 1; i < foundTransformsCount; i++) {
            waypoints.Add(foundTransforms[i]);
        }
    }
    public Transform NextWaypoint(Transform currentWaypoint=null) {
        string logId = "NextWaypoint";
        int waypointsCount = waypoints.Count;
        if(waypointsCount<=0) {
            logw(logId, "Tried to get next waypoint while waypointsCount="+waypointsCount+"=> returning null");
            return null;
        }
        if(currentWaypoint==null || !waypoints.Contains(currentWaypoint)) {
            logt(logId, "Path doesn't contain current waypoint => returning first waypoint");
            return waypoints[0];
        }
        int currentIndex = waypoints.IndexOf(currentWaypoint);
        if(currentIndex==waypointsCount-1) {
            logw(logId, "Current index is the last index => returning null");
            return currentWaypoint;
        }
        Transform nextWaypoint = waypoints[currentIndex+1];
        logt(logId, "CurrentIndex="+currentIndex+ " returning NextWaypoint="+nextWaypoint);
        return nextWaypoint;
    }
}
