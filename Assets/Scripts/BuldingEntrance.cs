using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldingEntrance : MonoBehaviour {
    public Building building;

    private UnitSelectionManager selection_manager;

    public void Start() {
        selection_manager = UnitSelectionManager.Connect();
    }

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject == selection_manager.current_selection && building.is_target) {
            var unit_owner = other.GetComponent<Unit>().owner;
            
            if(building.GetComponent<City>() != null) {
                // Entering a city:
                var city = building.GetComponent<City>();
                city.EnterBuilding(unit_owner);

                // Deselect the entering unit to avoid unwanted pathing on UI interaction:
                selection_manager.current_selection.GetComponent<HighlightEffect>().Deselect();
            } else if(building.GetComponent<ResourceBuilding>() != null) {
                // Entering a resource building:
                var resource_building = building.GetComponent<ResourceBuilding>();
                resource_building.EnterBuilding(unit_owner);
            } else {
                // Anything else:
                building.EnterBuilding(unit_owner);
            }
        }
    }

    // We need to handle the situation, where the unit is already inside the trigger, but reenters:
    public void OnTriggerStay(Collider other) {
        if(other.gameObject == selection_manager.current_selection && building.is_target) {
            // TODO...
        }
    }
}
