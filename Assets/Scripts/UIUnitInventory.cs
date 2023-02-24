using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUnitInventory : MonoBehaviour {
    public List<UIUnitInventorySlot> slots;

    public void OnLoad(Unit unit) {
        gameObject.SetActive(true);
        for(int i = 0; i < unit.items.Count; i++) {
            if(unit.items[i] != null) {
                slots[i].SetItem(unit.items[i]);
            }
        }

        for(int i = unit.items.Count; i < slots.Count; i++) {
            slots[i].Clear();
        }
    }
}
