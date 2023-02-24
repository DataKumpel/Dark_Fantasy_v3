using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerRotation : MonoBehaviour {
    public float rotation_speed = 1.0f;


    public void Update() {
        transform.Rotate(Vector3.up, rotation_speed * Time.deltaTime, Space.World);
    }
}
