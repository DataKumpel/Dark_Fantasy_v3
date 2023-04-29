using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {
    public GameObject resource_panel;
    public GameObject city_panel;
    public GameObject unit_inventory_panel;
    public GameObject recruitation_panel;
    public GameObject city_unit_exchange_panel;
    
    [HideInInspector] public bool is_over_ui = false;
    
    private int ui_layer;

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

    public static UIRecruitmentDialog ConnectRecruitmentDialog() {
        return UIManager
                .Connect()
                .recruitation_panel
                .GetComponent<UIRecruitmentDialog>();
    }

    public static UICityUnitExchange ConnectCityUnitExchange() {
        return UIManager
                .Connect()
                .city_unit_exchange_panel
                .GetComponent<UICityUnitExchange>();
    }

    public void Start() {
        ui_layer = LayerMask.NameToLayer("UI");
    }

    public void Update() {
        is_over_ui = IsPointerOverUIElement();
        // print(is_over_ui ? "Over UI" : "Not over UI");
    }
 
    // Returns true if we touched or hovering on UI element:
    private bool IsPointerOverUIElement() {
        var event_data = new PointerEventData(EventSystem.current);
        var results = new List<RaycastResult>();
        
        event_data.position = Input.mousePosition;
        EventSystem.current.RaycastAll(event_data, results);

        foreach(var result in results) {
            if(result.gameObject.layer == ui_layer) return true;
        }
        return false;
    }
}
