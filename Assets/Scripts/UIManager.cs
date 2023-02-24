using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public GameObject resource_panel;
    public GameObject city_panel;
    public GameObject unit_inventory_panel;

    public static UIManager Connect() {
        return GameObject
                .FindGameObjectWithTag("UIManager")
                .GetComponent<UIManager>();
    }

    public static UICityMenue ConnectCityMenue() {
        return UIManager
                .Connect()
                .city_panel
                .GetComponent<UICityMenue>();
    }

    public static UIUnitInventory ConnectUnitInventory() {
        return UIManager
                .Connect()
                .unit_inventory_panel
                .GetComponent<UIUnitInventory>();
    }
}
