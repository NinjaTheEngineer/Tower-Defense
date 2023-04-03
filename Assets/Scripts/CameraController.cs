using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NinjaTools;

public class CameraController : NinjaMonoBehaviour {
    public TerrainCollider terrainCollider;
    public float movementSpeed;
    public float rotationSpeed;
    Vector3 minPos;
    Vector3 maxPos;
    private void Start() {
        string logId = "Start";
        if(terrainCollider==null) {
            logw(logId, "TerrainCollider not asigned => no-op. Destroying CameraController");
            Destroy(gameObject);
            return;
        }
        Bounds terrainBounds = terrainCollider.bounds;
        minPos = terrainBounds.min;
        maxPos = terrainBounds.max;
    }
    private void Update() {
        float vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float rotation = 0;
        if(Input.GetKey(KeyCode.Q)) {
            rotation = rotationSpeed * Time.deltaTime;
        } else if(Input.GetKey(KeyCode.E)) {
            rotation = -rotationSpeed * Time.deltaTime;
        }
        transform.Translate(horizontal, 0, vertical);
        transform.position = Maths.Clamp(transform.position, minPos, maxPos);
        transform.Rotate(0, rotation, 0);
    }
}
