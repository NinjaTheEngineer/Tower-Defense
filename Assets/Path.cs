using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : NinjaMonoBehaviour {
    public List<Transform> waypoints;

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
            logw(logId, "Path doesn't contain current waypoint => returning first waypoint");
            return waypoints[0];
        }
        int currentIndex = waypoints.IndexOf(currentWaypoint);
        if(currentIndex==waypointsCount-1) {
            logw(logId, "Current index is the last index => returning null");
            return null;
        }
        Transform nextWaypoint = waypoints[currentIndex+1];
        logd(logId, "CurrentIndex="+currentIndex+ " returning NextWaypoint="+nextWaypoint);
        return nextWaypoint;
    }
}
