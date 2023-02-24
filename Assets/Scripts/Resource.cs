using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {
    public ResourceType type = ResourceType.wood;
    public bool random_amount = true;
    public int fixed_amount = 5;
    public int range_amount_lowest = 0;
    public int range_amount_highest = 10;
    public Transform marker_pos;

    [SerializeReference] public Collectable collectable = new();

    [HideInInspector] public bool is_target = false;

    private UnitSelectionManager selection_manager;

    public void Start() {
        selection_manager = UnitSelectionManager.Connect();
    }

    public void Collect(Unit collector) {
        var real_amount = fixed_amount;
        if(random_amount) {
            real_amount = Random.Range(range_amount_lowest, 
                                       range_amount_highest + 1);
        }
        // Deprecated: Resources are now stored locally within units...
        //collector.owner.AddResource(type, real_amount);
        collectable.quantity = real_amount;
        collector.Collect(collectable);
    }

    public Vector3 GetMarkerPos() {
        return marker_pos.position;
    }

    public void OnTriggerEnter(Collider other) {
        if(other.gameObject == selection_manager.current_selection && is_target) {
            Collect(other.GetComponent<Unit>());
            Destroy(gameObject);
        }
    }
}

public enum ResourceType {
    wood,
    stone,
    food,
    iron,
    void_crystal,
    gun_powder,
    sacrificial_blood,
    mana_essence,
    pure_silver,
}