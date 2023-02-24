using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMarkerDisabeling : MonoBehaviour {
    public GameObject ref_unit;
    public bool is_way_marker = false;

    public void DestroyMarker() {
        GetComponent<MeshRenderer>().enabled = false;

        if(is_way_marker) {
            ref_unit.GetComponent<UnitMovement>().OnDestinationReached();
            return;
        }
        
        ref_unit
            .GetComponent<UnitMovement>()
            .OnMarkerReached(this.gameObject);
        Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider col) {
        if(col.gameObject == ref_unit) {
            DestroyMarker();
        }
    }
}
