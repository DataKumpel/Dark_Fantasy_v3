using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResourceDisplay : MonoBehaviour {
    [Header("Resource Storage Displays")]
    public Text food_disp;
    public Text wood_disp;
    public Text stone_disp;
    public Text iron_disp;
    public Text gunpowder_disp;
    public Text puresilver_disp;
    public Text manaessence_disp;
    public Text sacrificialblood_disp;
    public Text voidcrystal_disp;

    [Header("Resource Cost Displays")]
    public Color can_afford_color = Color.green;
    public Color cannot_afford_color = Color.red;
    public GameObject cost_food_disp;
    public GameObject cost_wood_disp;
    public GameObject cost_stone_disp;
    public GameObject cost_iron_disp;
    public GameObject cost_gunpowder_disp;
    public GameObject cost_puresilver_disp;
    public GameObject cost_manaessence_disp;
    public GameObject cost_sacrificialblood_disp;
    public GameObject cost_voidcrystal_disp;

    public void UpdateResourceDisplays(CityResources resources) {
        food_disp.text = resources.food.ToString();
        wood_disp.text = resources.wood.ToString();
        stone_disp.text = resources.stone.ToString();
        iron_disp.text = resources.iron.ToString();
        gunpowder_disp.text = resources.gun_powder.ToString();
        puresilver_disp.text = resources.pure_silver.ToString();
        manaessence_disp.text = resources.mana_essence.ToString();
        sacrificialblood_disp.text = resources.sacrificial_blood.ToString();
        voidcrystal_disp.text = resources.void_crystal.ToString();
    }

    public void UpdateDisplays(City city) => UpdateResourceDisplays(city.resources);

    public void HideCosts() {
        cost_food_disp.SetActive(false);
        cost_wood_disp.SetActive(false);
        cost_iron_disp.SetActive(false);
        cost_stone_disp.SetActive(false);
        cost_gunpowder_disp.SetActive(false);
        cost_puresilver_disp.SetActive(false);
        cost_manaessence_disp.SetActive(false);
        cost_voidcrystal_disp.SetActive(false);
        cost_sacrificialblood_disp.SetActive(false);
    }

    public void UpdateCostDisplay(GameObject cost_disp, Text storage, int cost) {
        // Only show the display, when there are costs:
        cost_disp.SetActive(cost > 0);

        // Write the costs into the display:
        var cost_text = cost_disp.transform.GetChild(0).GetComponent<Text>();
        cost_text.text = (-cost).ToString();

        // Decide whether the costs are affordable (green) or too high (red):
        var resource_amount = Int32.Parse(storage.text);
        cost_text.color = resource_amount < cost ? cannot_afford_color : can_afford_color;
    }

    public void ShowCosts(CityBuildingCost costs) {
        // Clean up everything and start on blank canvas:
        HideCosts();

        // Update every cost display:
        UpdateCostDisplay(cost_food_disp, food_disp, costs.cost_food);
        UpdateCostDisplay(cost_wood_disp, wood_disp, costs.cost_wood);
        UpdateCostDisplay(cost_iron_disp, iron_disp, costs.cost_iron);
        UpdateCostDisplay(cost_stone_disp, stone_disp, costs.cost_stone);
        UpdateCostDisplay(cost_gunpowder_disp, gunpowder_disp, costs.cost_gunpowder);
        UpdateCostDisplay(cost_puresilver_disp, puresilver_disp, costs.cost_puresilver);
        UpdateCostDisplay(cost_manaessence_disp, manaessence_disp, costs.cost_manaessence);
        UpdateCostDisplay(cost_voidcrystal_disp, voidcrystal_disp, costs.cost_voidcrystal);
        UpdateCostDisplay(cost_sacrificialblood_disp, sacrificialblood_disp, costs.cost_sacrificialblood);
    }

    public void ShowCosts(CreatureCosts costs) {
        // Clean up everything and start on blank canvas:
        HideCosts();

        // Update every cost display:
        UpdateCostDisplay(cost_food_disp, food_disp, costs.food);
        UpdateCostDisplay(cost_wood_disp, wood_disp, costs.wood);
        UpdateCostDisplay(cost_iron_disp, iron_disp, costs.iron);
        UpdateCostDisplay(cost_stone_disp, stone_disp, costs.stone);
        UpdateCostDisplay(cost_gunpowder_disp, gunpowder_disp, costs.gun_powder);
        UpdateCostDisplay(cost_puresilver_disp, puresilver_disp, costs.pure_silver);
        UpdateCostDisplay(cost_manaessence_disp, manaessence_disp, costs.mana_essence);
        UpdateCostDisplay(cost_voidcrystal_disp, voidcrystal_disp, costs.void_crystal);
        UpdateCostDisplay(cost_sacrificialblood_disp, sacrificialblood_disp, costs.sacrificial_blood);
    }
}
