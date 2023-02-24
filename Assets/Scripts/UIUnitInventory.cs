using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitInventory : MonoBehaviour {
    public List<UIUnitInventorySlot> slots;

    public void OnLoad(Unit unit) {
        for(int i = 0; i < unit.max_items; i++) {
            slots[i].SetItem(unit.items[i]);
        }
    }
}
