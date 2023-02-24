using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitComfortZone : MonoBehaviour {
    public Unit unit;

    private UnitSelectionManager unit_selection_manager;

    public void Start() {
        unit_selection_manager = GameObject
            .FindGameObjectWithTag("UnitSelectionManager")
            .GetComponent<UnitSelectionManager>();
    }

    public void OnTriggerEnter(Collider col) {
        if(col.CompareTag("Unit") && col.gameObject != unit.gameObject) {
            Debug.Log($"{col.name} has entered {unit.name}s comfort zone...");
            
            var intruder = col.GetComponent<Unit>();
            var intruder_pos = new Vector2(intruder.transform.position.x,
                                           intruder.transform.position.z);
            var my_pos = new Vector2(transform.position.x, 
                                     transform.position.z);

            // Is it a friend or an enemy?
            if(intruder.owner != unit.owner) {
                Debug.Log("Enemy Encounter!");
                intruder.movement.StopMoving();
                intruder.movement.FaceTowards(my_pos);
                unit.movement.FaceTowards(intruder_pos);
            } else {
                // I really would like to rename the intruder now to be a
                // a friend, but that ist unperformant und takes up 
                // unnecessary amount of memory, so it will stay an intruder
                // (but a welcome one :D )
                Debug.Log("Friendly Encounter :)");
                if(unit.is_target) {
                    intruder.movement.StopMoving();
                    intruder.movement.FaceTowards(my_pos);
                    unit.movement.FaceTowards(intruder_pos);
                }
            }
        }
    }
}
