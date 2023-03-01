using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Creature : MonoBehaviour {
    public string creature_name = string.Empty;
    public CreatureTier tier = CreatureTier.tier_i;
    public int number = 1;
    public GameObject representation;
    public Sprite icon;

    [SerializeReference] public CreatureStats stats = new();
    [SerializeReference] public CreatureCosts costs = new();
}

[System.Serializable]
public class CreatureStats {
    public int attack = 0;
    public int defense = 0;
    public int damage_min = 0;
    public int damage_max = 0;
    public int spellpower = 0;
    public int resistence = 0;
    public int max_health = 0;
    public int cur_health = 0;
    public int max_mana = 0;
    public int cur_mana = 0;
    public int speed = 0;
    public int reproduction = 0;  // Weekly (8 days)
}

[System.Serializable]
public class CreatureCosts {
    public int wood = 0;
    public int stone = 0;
    public int iron = 0;
    public int food = 0;
    public int void_crystal = 0;
    public int pure_silver = 0;
    public int gun_powder = 0;
    public int mana_essence = 0;
    public int sacrificial_blood = 0;
}


public enum CreatureTier {
    tier_i,
    tier_ii,
    tier_iii,
    tier_iv,
    hero,
}
