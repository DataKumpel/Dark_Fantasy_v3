using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuldingEntrance : MonoBehaviour {
    public Building building;

    private UnitSelectionManager selection_manager;

    public void Start() {
        selection_manager = UnitSelectionManager.Connect();
    }

    public void VisitCity(Unit unit) {
        // Entering a city:
        var city = building.GetComponent<City>();

        // Check if there is already a visitor in town:
        if(city.army_visitor != null) {
            print($"There is already {city.army_visitor} in town... move it away!");
            return;
            // TODO (28.3.2023): Show this message in a dialog!
            // IDEA: Maybe automatically remove the other unit from being visitor???
        }

        // Enter the building and open city dialog:
        city.EnterBuilding(unit.owner);

        // Set entering army as visitor:
        city.UnitEnterBuilding(unit);

        // Deselect the entering unit to avoid unwanted pathing on UI interaction:
        selection_manager.current_selection.GetComponent<HighlightEffect>().Deselect();
    }

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject == selection_manager.current_selection && building.is_target) {
            var unit = other.GetComponent<Unit>();
            
            if(building.GetComponent<City>() != null) {
                VisitCity(unit);
            } else if(building.GetComponent<ResourceBuilding>() != null) {
                // Entering a resource building:
                var resource_building = building.GetComponent<ResourceBuilding>();
                resource_building.EnterBuilding(unit.owner);
            } else {
                // Anything else:
                building.EnterBuilding(unit.owner);
            }
        }
    }

    public void OnTriggerExit(Collider other) {
        // Only GameObjects with a Unit component can trigger:
        var unit = other.GetComponent<Unit>();
        if(unit == null) return;

        if(building.GetComponent<City>() != null) {
            // Leaving a city:
            var city = building.GetComponent<City>();

            // Only visiting armies can leave the city:
            if(city.army_visitor == unit.gameObject) {
                print($"{unit} left city {city}");
                city.army_visitor = null;
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
