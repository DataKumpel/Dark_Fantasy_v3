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

    public void UpdateResourceDisplays(int food, int wood, int stone, int iron, 
                                       int gunpowder, int puresilver, int manaessence, 
                                       int sacrificialblood, int voidcrystal) {
        food_disp.text = food.ToString();
        wood_disp.text = wood.ToString();
        stone_disp.text = stone.ToString();
        iron_disp.text = iron.ToString();
        gunpowder_disp.text = gunpowder.ToString();
        puresilver_disp.text = puresilver.ToString();
        manaessence_disp.text = manaessence.ToString();
        sacrificialblood_disp.text = sacrificialblood.ToString();
        voidcrystal_disp.text = voidcrystal.ToString();
    }

    public void UpdateDisplays(City city) => UpdateResourceDisplays(city.food, city.wood, city.stone, city.iron, 
                                                                    city.gun_powder, city.pure_silver, city.mana_essence,
                                                                    city.sacrificial_blood, city.void_crystal);

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
}
