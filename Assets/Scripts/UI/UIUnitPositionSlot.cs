using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitPositionSlot : MonoBehaviour {
    public Image unit_image;
    public Text unit_number;
    public Creature creature_in_slot;

    public void SetCreature(Creature creature) {
        creature_in_slot = creature;
        unit_image.sprite = creature.icon;
        unit_number.text = creature.number.ToString();
    }

    public void UnsetCreature() {
        creature_in_slot = null;
        unit_image.sprite = null;
        unit_number.text = "";
    }
}
