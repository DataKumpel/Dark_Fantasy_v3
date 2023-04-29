using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBuilding : Building {
    public ResourceType type = ResourceType.wood;
    public int amount_per_day = 5;
    public City delivery_target;

    public void DeliverResources() {
        // Don't deliver anywhere, if there is no target:
        if(delivery_target == null) return;

        delivery_target.AddResource(type, amount_per_day);
    }

    public void SelectDeliveryTarget() {
        if(owner == null) return;

        int city_count = 0;
        City only_city = null;
        foreach(var building in owner.buildings) {
            if(building.GetComponent<City>() != null) {
                city_count++;
                
                if(city_count > 1) {
                    // Leave the loop, as there is more than one city:
                     break;
                } else {
                    // If we find only one city, automatically set it as delivery target:
                    only_city = building.GetComponent<City>();
                }
            }
        }

        // Evaluate the search for cities:
        if(city_count == 0) {
            Debug.Log("The player hast no city... nowhere to deliver...");
        } else if(city_count == 1) {
            delivery_target = only_city;
        } else {
            Debug.Log("Choose your delivery target...");
        }
    }

    public new void EnterBuilding(Player player) {
        if(player == owner) {
            Debug.Log("Maybe you want to change your delivery target?");
            SelectDeliveryTarget();
        } else {
            // Remove this building from a players list of buildings, if owned:
            if(owner != null) {
                owner.UnregisterBuilding(this);
            }

            // Add this resource building to the conquering players list:
            owner = player;
            owner.RegisterBuilding(this);
            SwitchLight(true);

            // Let the player choose which city this building is delivering resources to:
            SelectDeliveryTarget();
        }
    }
}
