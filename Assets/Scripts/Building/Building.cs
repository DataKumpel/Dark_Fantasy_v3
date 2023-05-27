using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {
    public Player owner;
    public Light reveal_light;
    public float reveal_radius = 1f;
    public GameObject entrance_trigger;

    [HideInInspector] public bool is_target = false;

    public void Start() {
        // On Start switch all lights off:
        SwitchLight(false);
        if(owner != null) {
            owner.RegisterBuilding(this);
        }
    }

    public void SwitchLight(bool val) {
        reveal_light.range = val ? reveal_radius : 0f;
    }

    public void EnterBuilding(Player player) {
        if(player == owner) {
            print($"You visited your building {gameObject.name}.");
        } else {
            print($"You conquered the building {gameObject.name}!");
            
            // Remove this building from a players list of buildings, if owned:
            if(owner != null) {
                owner.UnregisterBuilding(this);
            }
            owner = player;
            owner.RegisterBuilding(this);
            SwitchLight(true);
        }
    }

    public Vector3 GetMarkerPos() {
        return entrance_trigger.transform.position;
    }

    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(reveal_light.transform.position, reveal_radius);
    }
}
