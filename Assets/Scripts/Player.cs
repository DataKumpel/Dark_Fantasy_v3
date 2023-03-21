using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [Header("General Infos")]
    public bool is_ai = false;
    public string player_name = string.Empty;
    public Faction faction = Faction.factionless;
    public Color player_color = Color.black;
    public Vector3 cam_start_pos;

    [Header("Resources")]
    public int wood = 0;
    public int stone = 0;
    public int food = 0;
    public int iron = 0;
    public int void_crystal = 0;
    public int gun_powder = 0;
    public int sacrificial_blood = 0;
    public int mana_essence = 0;
    public int pure_silver = 0;

    [Header("Units")]
    public List<Unit> units = new();

    [Header("Buildings")]
    public List<Building> buildings = new();

    [Header("Torches")]
    public List<Torch> torches = new();

    [HideInInspector] public UIResourceDisplay res_disp;
    [HideInInspector] public Vector3 last_cam_pos;

    private CameraMovement cam_move;
    private RoundManager round_manager;

    public void Start() {
        // res_disp = GameObject
        //     .FindGameObjectWithTag("UI_ResourceDisplay")
        //     .GetComponent<UIResourceDisplay>();

        // Connect to camera:
        cam_move = CameraMovement.Connect();

        last_cam_pos = cam_start_pos;

        // Connect to round manager:
        round_manager = RoundManager.Connect();
        
        // Note: Registering Players on their own leads to an unpredictable order...
        //round_manager.RegisterPlayer(this);
    }

    public void AddResource(ResourceType type, int amnt) {
        // Deprecated: Resources are managed locally in cities and buildings...
        switch(type) {
            case ResourceType.wood: wood += amnt; break;
            case ResourceType.food: food += amnt; break;
            case ResourceType.iron: iron += amnt; break;
            case ResourceType.stone: stone += amnt; break;
            case ResourceType.gun_powder: gun_powder += amnt; break;
            case ResourceType.pure_silver: pure_silver += amnt; break;
            case ResourceType.void_crystal: void_crystal += amnt; break;
            case ResourceType.mana_essence: mana_essence += amnt; break;
            case ResourceType.sacrificial_blood: sacrificial_blood += amnt; break;
        }
    }

    public void HarvestFromBuildings() {
        foreach(var building in buildings) {
            if(building.gameObject.GetComponent<ResourceBuilding>() != null) {
                var resource_building = building.gameObject
                                                .GetComponent<ResourceBuilding>();
                
                // Resource Buildings deliver their Resources now on their own:
                resource_building.DeliverResources();
                //AddResource(resource_building.type, 
                //            resource_building.amount_per_day);
            }
        }
    }

    public void LightBuildings(bool val) {
        foreach(var building in buildings) {
            if(building == null) continue;
            
            building.SwitchLight(val);
        }
    }

    public void LightUnits(bool val) {
        foreach(var unit in units) {
            if(unit == null) continue;
            
            unit.SwitchLight(val);
        }
    }

    public void LightTorches(bool val) {
        foreach(var torch in torches) {
            if(torch == null) continue;

            torch.SwitchLight(val);
        }
    }

    public void LightAll(bool val) {
        LightBuildings(val);
        LightUnits(val);
        LightTorches(val);
    }

    public void DeselectAll() {
        foreach(var unit in units) {
            unit.Unselect();
        }
    }

    public void RefreshUnits() {
        foreach(var unit in units) {
            unit.Refresh();
        }
    }

    public void RefreshCities() {
        foreach(var builing in buildings) {
            var city = builing.GetComponent<City>();
            if(city != null) {
                city.Refresh();
            }
        }
    }

    public void RegisterBuilding(Building building) {
        buildings.Add(building);
    }

    public void UnregisterBuilding(Building building) {
        buildings.Remove(building);
    }

    public void RegisterUnit(Unit unit) {
        units.Add(unit);
    }

    public void UnregisterUnit(Unit unit) {
        units.Remove(unit);
    }

    public void RegisterTorch(Torch torch) {
        // TODO: Maybe let the register methods handle setting ownership of the object?
        torches.Add(torch);
    }

    public void UnregisterTorch(Torch torch) {
        torches.Remove(torch);
    }

    public void SaveCamPos() {
        last_cam_pos = cam_move.transform.position;
    }

    public void LoadCamPos() {
        // TODO: It may be inefficient to spawn a dummy gameObject...
        var obj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), 
                              last_cam_pos, Quaternion.identity);
        cam_move.FocusOn(obj, false);
        Destroy(obj);
    }

    public void MakeAITurn() {
        // TODO... this will gonna be hard :[
    }
}

public enum Faction {
    factionless = 0,
    empire,
    church,
    soulless,
    cult,
    order,
}