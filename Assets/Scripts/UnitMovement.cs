using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : MonoBehaviour {
    public GameObject way_marker_prefab;

    [Header("Path Marker Options")]
    public bool use_path_markers = true;
    public GameObject path_marker_prefab;
    public Material in_range_mat;
    public Material out_of_range_mat;
    public float path_marker_dist = 1f;
    public float path_marker_ground_offset = 0.5f;

    [Header("Rotation")]
    public float smooth_facing_speed = 3f;

    [HideInInspector] public bool is_moving = false;
    [HideInInspector] public bool is_exhausted = false;

    private NavMeshAgent agent;
    private NavMeshPath path;
    private HighlightEffect highlight;
    private Mouse mouse;
    private Keyboard keyboard;
    private GameObject way_marker;
    private bool marker_on_map = false;
    private bool just_selected = true;
    private bool markers_active = false;
    private List<GameObject> path_markers = new();
    private CameraMovement cam_movement;
    private FogOfWar fog_of_war;
    private GameObject target;
    private bool is_facing = false;
    private Quaternion target_rotation;

    public void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        highlight = GetComponent<HighlightEffect>();
        path = new NavMeshPath();
        mouse = Mouse.current;
        keyboard = Keyboard.current;
        cam_movement = CameraMovement.Connect();
        fog_of_war = GameObject
            .FindGameObjectWithTag("FogOfWar")
            .GetComponent<FogOfWar>();
    }

    public void PlaceMarker(Vector3 pos) {
        if(marker_on_map) {
            way_marker.transform.position = pos;
        } else {
            way_marker = Instantiate(way_marker_prefab, 
                                     pos, 
                                     way_marker_prefab
                                        .transform.rotation);
            marker_on_map = true;
        }
        way_marker.GetComponent<PathMarkerDisabeling>().ref_unit = gameObject;
        way_marker.GetComponent<MeshRenderer>().enabled = true;

        // End this function here, if no pathmarkers shall 
        // be spawned:
        if(!use_path_markers)
            return;

        // Clear all path markers for new calculation:
        foreach(var marker in path_markers) {
            if(marker != null) {
                Destroy(marker);
            }
        }
        path_markers = new List<GameObject>();

        FindPath();
    }

    private void Untarget() {
        if(target != null) {
            // Untarget buildings:
            if(target.GetComponent<Building>() != null)
                target.GetComponent<Building>().is_target = false;
            
            // Untarget resources:
            if(target.GetComponent<Resource>() != null)
                target.GetComponent<Resource>().is_target = false;
            
            // Untraget units:
            if(target.GetComponent<Unit>() != null)
                target.GetComponent<Unit>().is_target = false;
            
            // Untarget collectables:
            // TODO...

            // Reset target gameobject to null:
            target = null;
        }
    }

    public void HandlePlayerSelection(RaycastHit info) {
        // Untarget the target :D
        Untarget();
        
        if(info.collider.gameObject == gameObject) {
            
            // Clicked on self...
            var ui = UIManager.ConnectUnitInventory();
            ui.OnLoad(GetComponent<Unit>());

        } else if(info.collider.CompareTag("Building")) {
            
            var building = info.collider.GetComponent<Building>();
            building.is_target = true;
            target = building.gameObject;
            PlaceMarker(building.GetMarkerPos());

        } else if(info.collider.CompareTag("Unit")) {
            
            // Clicked on another unit...
            var unit = info.collider.GetComponent<Unit>();
            unit.is_target = true;
            target = unit.gameObject;

            var marker = info.collider.GetComponent<HighlightEffect>().marker;
            PlaceMarker(unit.transform.position);

        } else if(info.collider.CompareTag("Resource")) {
            
            var resource = info.collider.GetComponent<Resource>();
            resource.is_target = true;
            target = resource.gameObject;
            PlaceMarker(resource.GetMarkerPos());

        } else if(info.collider.CompareTag("Collectable")) {
            
            // Clicked on a collectable or item...
            // TODO...

        } else {
            PlaceMarker(info.point);
        }
    }

    private void ColorizeMarker(int index, float total_dist) {
        // Colorizes a marker with a given total distance 
        // from the unit:
        if(total_dist > GetComponent<Unit>().walk_distance) {
            path_markers[index].GetComponent<MeshRenderer>().material = out_of_range_mat;
        } else {
            path_markers[index].GetComponent<MeshRenderer>().material = in_range_mat;
        }
    }

    public void ColorizePath() {
        if(path_markers.Count == 0) return;
        
        var total_dist = 0f;

        // Add distance from unit to first marker:
        total_dist += Vector3.Distance(transform.position, 
                                       path_markers[0].transform.position);
        ColorizeMarker(0, total_dist);

        for(int i = 1; i < path_markers.Count; i++) {
            total_dist += Vector3.Distance(path_markers[i].transform.position, 
                                           path_markers[i - 1].transform.position);
            ColorizeMarker(i, total_dist);
        }
    }

    public void FindPath() {
        // Path can only be found, if a marker is on the map:
        if(!marker_on_map)
            return;

        if(NavMesh.CalculatePath(transform.position, way_marker.transform.position,
                                 NavMesh.AllAreas, path)) {
            GameObject obj;
            for(int i = 0; i < path.corners.Length - 1; i++) {
                var corner_start = path.corners[i];
                var corner_end = path.corners[i + 1];
                var direction = (corner_end - corner_start).normalized * path_marker_dist;
                var dist = Vector3.Distance(corner_start, corner_end);
                var current_pos = corner_start;

                while(dist > 0f) {
                    // Correct position down to terrain:
                    var layers = ~(LayerMask.NameToLayer("Terrain") | LayerMask.NameToLayer("Water"));
                    if(Physics.Raycast(current_pos + Vector3.up * 50, Vector3.down, 
                                       out RaycastHit hit, 100f, layers)) {
                        current_pos = new Vector3(current_pos.x, 
                                                  hit.point.y + path_marker_ground_offset, 
                                                  current_pos.z);
                    }

                    obj = Instantiate(path_marker_prefab, current_pos, 
                                      path_marker_prefab.transform.rotation);
                    obj.GetComponent<PathMarkerDisabeling>().ref_unit = gameObject;
                    path_markers.Add(obj);
                    current_pos += direction;
                    dist -= path_marker_dist;
                }
            }

            // Check if markers are actually reachable:
            ColorizePath();
        } else {
            Debug.Log(path.status.ToString());
        }
    }

    public void OnMarkerReached(GameObject marker) {
        //path_markers.RemoveAt(0);
        path_markers.Remove(marker);
    }

    public void OnDestinationReached() {
        is_moving = false;
        agent.isStopped = true;
        
        // Clean up missed markers in the end...
        if(path_markers.Count != 0) {
            foreach(var marker in path_markers) {
                marker.GetComponent<PathMarkerDisabeling>().DestroyMarker();
            }
        }
    }

    public void SetMarkersActive(bool active) {
        if(markers_active != active) {
            way_marker.SetActive(active);
            foreach(var marker in path_markers) {
                marker.SetActive(active);
            }
            ColorizePath();
            markers_active = active;
        }
    }

    public void StopMoving() {
        agent.isStopped = true;
        is_moving = false;
    }

    public void FaceTowards(Vector2 pos) {
        is_facing = true;
        
        // Calculate target rotation:
        var dir = new Vector3(pos.x, this.transform.position.y, pos.y) - transform.position;
        target_rotation = Quaternion.LookRotation(dir, Vector3.up);
    }

    public void Update() {
        if(is_moving) {
            fog_of_war.UpdateFogMesh();
        }

        if(is_facing) {
            // Smoothly face target:
            transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                  target_rotation, 
                                                  Time.deltaTime * smooth_facing_speed);
            if(Quaternion.Angle(transform.rotation, target_rotation) < 1f) {
                is_facing = false;
            }
        }

        // Following inputs are only allowed once this unit has been selected:
        if(highlight.is_selected) {
            if(marker_on_map) {
                SetMarkersActive(true);
            }

            // This prevents the marker spawning directly onto the selected unit:
            if(just_selected) {
                just_selected = false;
                return;
            }

            // Place a marker by clicking left:
            if(mouse.leftButton.wasPressedThisFrame) {
                // Stop movement for the path would be displayed incorrectly:
                if(is_moving) {
                    StopMoving();
                }
                if(Physics.Raycast(Camera.main.ScreenPointToRay(mouse.position.ReadValue()),
                                   out RaycastHit hit)) {
                    HandlePlayerSelection(hit);
                }
            }

            // Tell the unit to march towards destination by pressing SPACE:
            if(keyboard.spaceKey.wasPressedThisFrame && !is_exhausted) {
                agent.destination = way_marker.transform.position;
                agent.isStopped = !agent.isStopped;
                is_moving = !is_moving;
                cam_movement.FocusOn(gameObject, true);
            }
        } else {
            // Cleaning up:
            if(marker_on_map) {
                SetMarkersActive(false);
            }

            if(is_moving) {
                StopMoving();
            }

            just_selected = true;
        }
    }
}
