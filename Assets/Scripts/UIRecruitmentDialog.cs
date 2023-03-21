using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRecruitmentDialog : MonoBehaviour {
    public Button all_btn;
    public Button buy_btn;
    public Button cancel_btn;
    public Image creature_image;
    public Slider recruit_slider;
    public Text stock_display;
    public Text recruit_display;

    [SerializeReference] public UICostPerUnit cost_per_unit = new();
    [SerializeReference] public UITotalCost total_cost = new();

    private CityResources city_resources;
    private Creature creature;

    public void OpenDialog(City city, CreatureType type) {
        city_resources = city.resources;
        switch(type) {
            case CreatureType.creature_tier_i_1: {
                creature = city.recr_creature_i_1;
                stock_display.text = Mathf.FloorToInt(city.creature_i_1_stock).ToString();
                break;
            }
            case CreatureType.creature_tier_i_2: {
                creature = city.recr_creature_i_2;
                stock_display.text = Mathf.FloorToInt(city.creature_i_2_stock).ToString();
                break;
            }
            case CreatureType.creature_tier_ii_1: {
                creature = city.recr_creature_ii_1;
                stock_display.text = Mathf.FloorToInt(city.creature_ii_1_stock).ToString();
                break;
            }
            case CreatureType.creature_tier_ii_2: {
                creature = city.recr_creature_ii_2;
                stock_display.text = Mathf.FloorToInt(city.creature_ii_2_stock).ToString();
                break;
            }
            case CreatureType.creature_tier_iii_1: {
                creature = city.recr_creature_iii_1;
                stock_display.text = Mathf.FloorToInt(city.creature_iii_1_stock).ToString();
                break;
            }
            case CreatureType.creature_tier_iii_2: {
                creature = city.recr_creature_iii_2;
                stock_display.text = Mathf.FloorToInt(city.creature_iii_2_stock).ToString();
                break;
            }
            case CreatureType.creature_tier_iv: {
                if(city.creatures_iv_1.is_built) {
                    creature = city.recr_creature_iv_1;
                    stock_display.text = Mathf.FloorToInt(city.creature_iv_1_stock).ToString();
                } else {
                    creature = city.recr_creature_iv_2;
                    stock_display.text = Mathf.FloorToInt(city.creature_iv_2_stock).ToString();
                }
                break;
            }
        }

        UpdateCostPerUnit();
        UpdateTotalCost(0);
    }

    public void OnBuy() {

    }

    public void OnAll() {

    }

    public void OnCancel() {

    }

    public void UpdateCostPerUnit() => cost_per_unit.UpdateDisplays(creature.costs);
    public void UpdateTotalCost(int amnt) => total_cost.UpdateDisplays(creature.costs, city_resources, amnt);
}

public class UICostPerUnit {
    public Text wood_disp;
    public Text stone_disp;
    public Text food_disp;
    public Text iron_disp;
    public Text gunpowder_disp;
    public Text pure_silver_disp;
    public Text mana_essence_disp;
    public Text sacrificial_blood_disp;
    public Text voidcrystal_disp;

    public void UpdateDisplays(CreatureCosts costs) {
        wood_disp.text = costs.wood.ToString();
        stone_disp.text = costs.stone.ToString();
        food_disp.text = costs.food.ToString();
        iron_disp.text = costs.iron.ToString();
        gunpowder_disp.text = costs.gun_powder.ToString();
        pure_silver_disp.text = costs.pure_silver.ToString();
        mana_essence_disp.text = costs.mana_essence.ToString();
        sacrificial_blood_disp.text = costs.sacrificial_blood.ToString();
        voidcrystal_disp.text = costs.void_crystal.ToString();
    }
}

public class UITotalCost {
    public Text wood_disp;
    public Text stone_disp;
    public Text food_disp;
    public Text iron_disp;
    public Text gunpowder_disp;
    public Text pure_silver_disp;
    public Text mana_essence_disp;
    public Text sacrificial_blood_disp;
    public Text voidcrystal_disp;

    public Color default_color;
    public Color available_color = Color.green;
    public Color not_available_color = Color.red;

    public void UpdateDisplays(CreatureCosts costs, CityResources resources, int amnt) {
        // Write total resource requirements into displays:
        SetTexts(costs, resources, amnt);

        // Colorize resources that exceed city resource limits:
        Colorize(costs, resources, amnt);
    }
    
    public void SetTexts(CreatureCosts costs, CityResources resources, int amnt) {
        wood_disp.text = $"{costs.wood * amnt}/{resources.wood}";
        stone_disp.text = $"{costs.stone * amnt}/{resources.stone}";
        food_disp.text = $"{costs.food * amnt}/{resources.food}";
        iron_disp.text = $"{costs.iron * amnt}/{resources.iron}";
        gunpowder_disp.text = $"{costs.gun_powder * amnt}/{resources.gun_powder}";
        pure_silver_disp.text = $"{costs.pure_silver * amnt}/{resources.pure_silver}";
        mana_essence_disp.text = $"{costs.mana_essence * amnt}/{resources.mana_essence}";
        sacrificial_blood_disp.text = $"{costs.sacrificial_blood * amnt}/{resources.sacrificial_blood}";
        voidcrystal_disp.text = $"{costs.void_crystal * amnt}/{resources.void_crystal}";
    }

    public void Colorize(CreatureCosts costs, CityResources resources, int amnt) {
        // Wood:
        if(costs.wood * amnt == 0) {
            wood_disp.color = default_color;
        } else if(costs.wood * amnt <= resources.wood) {
            wood_disp.color = available_color;
        } else {
            wood_disp.color = not_available_color;
        }

        // Stone:
        if(costs.stone * amnt == 0) {
            stone_disp.color = default_color;
        } else if(costs.stone * amnt <= resources.stone) {
            stone_disp.color = available_color;
        } else {
            stone_disp.color = not_available_color;
        }

        // Food:
        if(costs.food * amnt == 0) {
            food_disp.color = default_color;
        } else if(costs.food * amnt <= resources.food) {
            food_disp.color = available_color;
        } else {
            food_disp.color = not_available_color;
        }

        // Iron:
        if(costs.iron * amnt == 0) {
            iron_disp.color = default_color;
        } else if(costs.iron * amnt <= resources.iron) {
            iron_disp.color = available_color;
        } else {
            iron_disp.color = not_available_color;
        }

        // Gunpowder:
        if(costs.gun_powder * amnt == 0) {
            gunpowder_disp.color = default_color;
        } else if(costs.gun_powder * amnt <= resources.gun_powder) {
            gunpowder_disp.color = available_color;
        } else {
            gunpowder_disp.color = not_available_color;
        }

        // Pure Silver:
        if(costs.pure_silver * amnt == 0) {
            pure_silver_disp.color = default_color;
        } else if(costs.pure_silver * amnt <= resources.pure_silver) {
            pure_silver_disp.color = available_color;
        } else {
            pure_silver_disp.color = not_available_color;
        }

        // Mana Essence:
        if(costs.mana_essence * amnt == 0) {
            mana_essence_disp.color = default_color;
        } else if(costs.mana_essence * amnt <= resources.mana_essence) {
            mana_essence_disp.color = available_color;
        } else {
            mana_essence_disp.color = not_available_color;
        }

        // Sacrificial Blood:
        if(costs.sacrificial_blood * amnt == 0) {
            sacrificial_blood_disp.color = default_color;
        } else if(costs.sacrificial_blood * amnt <= resources.sacrificial_blood) {
            sacrificial_blood_disp.color = available_color;
        } else {
            sacrificial_blood_disp.color = not_available_color;
        }

        // Void Crystal:
        if(costs.void_crystal * amnt == 0) {
            voidcrystal_disp.color = default_color;
        } else if(costs.void_crystal * amnt <= resources.void_crystal) {
            voidcrystal_disp.color = available_color;
        } else {
            voidcrystal_disp.color = not_available_color;
        }
    }
}
