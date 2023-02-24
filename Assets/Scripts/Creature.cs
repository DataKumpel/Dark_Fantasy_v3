using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {
    public string creature_name = string.Empty;
    public CreatureTier tier = CreatureTier.tier_i;
    public int number = 1;

    [SerializeReference] public CreatureStats stats = new();
}


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
}


public enum CreatureTier {
    tier_i,
    tier_ii,
    tier_iii,
    tier_iv,
    hero,
}
