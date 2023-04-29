using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICityUnitExchange : MonoBehaviour {
    public List<UIUnitPositionSlot> upper_slots = new List<UIUnitPositionSlot>(8);
    public List<UIUnitPositionSlot> lower_slots = new List<UIUnitPositionSlot>(8);

    public void UnsetSlots(List<UIUnitPositionSlot> slots) {
        foreach(var slot in slots) {
            slot.UnsetCreature();
        }
    }

    public void UnitToSlots(Unit unit, List<UIUnitPositionSlot> slots) {
        foreach(var creature in unit.army) {
            slots[creature.army_index].SetCreature(creature);
        }
    }

    public void OnEnter(City city) {

    }

    public void OnEnterUnit(City city, Unit unit) {

    }

    public void Start() {
        UnsetSlots(upper_slots);
        UnsetSlots(lower_slots);
    }
}
