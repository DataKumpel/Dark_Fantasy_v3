using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour {
    [Header("Speed Settings")]
    public float speed_normal = 1f;
    public float speed_fast = 2f;
    public float smoothing_speed = 1f;
    public float smoothing_offset = 0.1f;
    public bool mouse_scrolling_activated = false;

    [Header("Rotation Settings")]
    public bool rotation_enabled = false;
    public float rotation_speed = 1f;

    [Header("Zoom Settings")]
    public GameObject cam;
    public float min_dist = 1f;
    public float max_dist = 10f;
    public float zoom_speed = 1f;
    public float zoom_offset = 0.1f;

    private readonly Keyboard keyboard = Keyboard.current;
    private readonly Mouse mouse = Mouse.current;
    private Vector3 destination;
    private Vector3 zoom_destination;
    private Vector3 zoom_dest_min;
    private Vector3 zoom_dest_max;
    private Quaternion rot_destination;
    private bool is_focused = false;
    private GameObject focus_obj;

    public void Start() {
        destination = transform.position;
        rot_destination = transform.rotation;
        zoom_destination = cam.transform.localPosition;
        zoom_dest_min = new Vector3(0, min_dist, -min_dist);
        zoom_dest_max = new Vector3(0, max_dist, -max_dist);
    }

    public static CameraMovement Connect() {
        return GameObject.FindGameObjectWithTag("MainCam").GetComponent<CameraMovement>();
    }

    private void ZoomCam() {
        var origin = cam.transform.localPosition;
        zoom_destination += mouse.scroll.y.ReadValue() 
                                * Time.deltaTime 
                                * zoom_speed 
                                * cam.transform.forward;

        // Check limits:
        if(zoom_destination.y < min_dist && zoom_destination.z > -min_dist) {
            zoom_destination = zoom_dest_min;
        }
        if(zoom_destination.y > max_dist && zoom_destination.z < -max_dist) {
            zoom_destination = zoom_dest_max;
        }

        // Smooth zooming:
        if(Vector3.Distance(origin, zoom_destination) > zoom_offset) {
            cam.transform.localPosition = Vector3.Slerp(origin, 
                                                        zoom_destination, 
                                                        Time.deltaTime * zoom_speed);
        }
    }

    private void CorrectToTerrain() {
        var layers = ~(LayerMask.NameToLayer("Terrain") | LayerMask.NameToLayer("Water"));
        if(Physics.Raycast(transform.position + transform.up * 50, -transform.up, 
                           out RaycastHit hit, 100f, layers)) {
            destination = new Vector3(destination.x, hit.point.y, destination.z);
        }
    }

    private void FollowSmooth() {
        if(Vector3.Distance(transform.position, destination) > smoothing_offset) {
            transform.position = Vector3.Lerp(transform.position, 
                                              destination, 
                                              Time.deltaTime * smoothing_speed);
        }
    }

    private void MoveCam() {
        // Movement speed:
        var multiplier = keyboard.shiftKey.isPressed ? 
                            Time.deltaTime * speed_fast : 
                            Time.deltaTime * speed_normal;

        // Movement via keyboards WASD:
        if(keyboard.wKey.isPressed) {
            destination += transform.forward * multiplier;
            FocusOn(null, false);
        }
        if(keyboard.sKey.isPressed) {
            destination -= transform.forward * multiplier;
            FocusOn(null, false);
        }
        if(keyboard.aKey.isPressed) {
            destination -= transform.right * multiplier;
            FocusOn(null, false);
        }
        if(keyboard.dKey.isPressed) {
            destination += transform.right * multiplier;
            FocusOn(null, false);
        }

        CorrectToTerrain();
        FollowSmooth();
    }

    private void RotateCam() {
        // Rotation via mid mouse button:
        // TODO: Maybe rotation should be performed via Q and E rather than middle mouse button?
        if(mouse.middleButton.isPressed && rotation_enabled) {
            var rot = transform.rotation.eulerAngles;
            rot_destination = Quaternion.Euler(
                rot.x,
                rot.y + mouse.delta.ReadValue().x * rotation_speed,
                rot.z
            );
        }

        // Smooth rotation:
        if(rotation_enabled) {
            transform.rotation = Quaternion.Lerp(transform.rotation, 
                                                 rot_destination, 
                                                 Time.deltaTime);
        }
    }

    public void FocusOn(GameObject focus, bool tracking) {
        if(focus == null) {
            is_focused = false;
            focus_obj = null;
            return;
        }

        destination.x = focus.transform.position.x;
        destination.z = focus.transform.position.z;
        CorrectToTerrain();
        FollowSmooth();

        if(tracking) {
            is_focused = true;
            focus_obj = focus;
        }
    }

    public void SetZoom(float value) {
        zoom_destination = new Vector3(0, value, -value);
    }

    public float GetZoom() {
        return zoom_destination.y;
    }

    public void Update() {
        RotateCam();
        MoveCam();
        ZoomCam();

        if(is_focused) {
            FocusOn(focus_obj, true);
        }
    }
}
