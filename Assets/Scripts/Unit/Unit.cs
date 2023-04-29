using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Player owner;
    public Light reveal_light;
    public float reveal_radius = 1f;
    public float max_walk_distance = 10f;

    [Header("Army")]
    public int max_army_size = 8;  // Not needed... handled by list attribute .Count
    public List<Creature> army = new(8);
    
    [Header("Torches")]
    // TODO: Change torches to be a collectable in inventory???
    public int num_torches = 5; 
    public GameObject torch_prefab;
    public Transform torch_placeing_point;
    public float reveal_radius_normal = 1f;
    public Color reveal_light_color_normal = Color.white;
    public float reveal_radius_dark = 0f;
    public Color reveal_light_color_dark = Color.blue;

    [Header("Inventory")]
    public int max_items = 5 * 2;  // 5 rows, 2 columns
    public List<Collectable> items = new();

    [HideInInspector] public float walk_distance;
    [HideInInspector] public UnitMovement movement;
    [HideInInspector] public bool is_target = false;

    private Vector3 prev_pos;

    public void Start() {
        movement = GetComponent<UnitMovement>();
        walk_distance = max_walk_distance;
        prev_pos = transform.position;
        SwitchLight(false);
        if(owner != null) {
            owner.RegisterUnit(this);
        }
        
        // Automatically set every creatures army index:
        for(int i = 0; i < max_army_size; i++) {
            if(army[i] == null) continue;
            army[i].army_index = i;
        }
    }

    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(reveal_light.transform.position,
                              reveal_radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(reveal_light.transform.position, 
                              reveal_radius_dark);
    }

    public void SwitchLight(bool val) {
        reveal_light.range = val ? reveal_radius : 0f;
    }

    public void Unselect() {
        GetComponent<HighlightEffect>().Deselect();
    }

    public void Refresh() {
        walk_distance = max_walk_distance;
        movement.is_exhausted = false;
    }

    public void PlaceTorch() {
        // If there are no torches, none can be placed:
        // TODO: Look for torches in inventory...
        if(num_torches == 0) {
            Debug.Log("No torches to place...");
            return;
        }

        // Place a torch in the world, to banish the darkness:
        var torch = Instantiate(torch_prefab, 
                                torch_placeing_point.position, 
                                torch_placeing_point.rotation)
                        .GetComponent<Torch>();
        torch.owner = owner;
        owner.RegisterTorch(torch);
        num_torches -= 1;

        // If the last torch is placed, it gets dark for the unit:
        if(num_torches == 0) {
            reveal_radius = reveal_radius_dark;
            reveal_light.color = reveal_light_color_dark;
            SwitchLight(true);
        }
    }

    public bool Collect(Collectable item) {
        // Look if we can add the collected item to an 
        // existing stack:
        foreach(var inv_item in items) {
            if(item.item_name == inv_item.item_name) {
                // Skip full stacks:
                if(inv_item.GetDifference() == 0) continue;
                
                if(inv_item.GetDifference() >= item.quantity) {
                    // Item quantity fits, so add to stack:
                    inv_item.quantity += item.quantity;
                    return true;
                } else {
                    // Item quantity doesn't fit, so create 
                    // another stack:
                    item.quantity -= inv_item.GetDifference();
                    inv_item.quantity = inv_item.max_stack_size;
                    
                    // Check if we can add another item:
                    if(items.Count < max_items) {
                        items.Add(item);
                        return true;
                    } else {
                        Debug.Log("Inventory full...");
                        return false;
                    }
                }
            }
        }

        // If there was no item with this name in the inventory,
        // try adding it:
        if(items.Count < max_items) {
            items.Add(item);
            return true;
        } else {
            Debug.Log("Inventory full...");
            return false;
        }
    }

    public void AddCreature(Creature creature, int amnt) {
        // Look, if there is already a same name creature in the army:
        foreach(var army_creature in army) {
            // Skip empty slots:
            if(army_creature == null) continue;

            // Look for creatures with same army name:
            if(army_creature.creature_name == creature.creature_name) {
                army_creature.number += amnt;
                return;
            }
        }

        // If no fitting creature was found, try to create a new one:
        if(army.Count == max_army_size) throw new ArmyFullException();

        // Add the creature to the army:
        army.Add(creature);
    }

    public void Update() {
        if(movement.is_moving) {
            walk_distance -= Vector3.Distance(prev_pos, 
                                              transform.position);
            prev_pos = transform.position;

            if(walk_distance <= 0f) {
                movement.is_exhausted = true;
                movement.StopMoving();
            }
        }
    }
}

[System.Serializable]
public class ArmyFullException : System.Exception
{
    public ArmyFullException() { }
    public ArmyFullException(string message) : base(message) { }
    public ArmyFullException(string message, System.Exception inner) : base(message, inner) { }
    protected ArmyFullException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
