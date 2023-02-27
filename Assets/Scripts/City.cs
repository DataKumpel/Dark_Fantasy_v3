using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class City : Building {
    public Faction faction = Faction.empire;
    public string city_name = string.Empty;
    public GameObject focus_point;
    public float zoom_value = 20f;

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

    [Header("Main Building")]
    [SerializeReference] public CityBuilding camp = new();
    [SerializeReference] public CityBuilding village = new();
    [SerializeReference] public CityBuilding town = new();
    [SerializeReference] public CityBuilding city = new();
    [SerializeReference] public CityBuilding monopolis = new();

    [Header("Mage Tower")]
    [SerializeReference] public CityBuilding mage_tower_lvl_1 = new();
    [SerializeReference] public CityBuilding mage_tower_lvl_2 = new();
    [SerializeReference] public CityBuilding mage_tower_lvl_3 = new();
    [SerializeReference] public CityBuilding mage_tower_lvl_4 = new();
    [SerializeReference] public CityBuilding mage_tower_lvl_5 = new();
    [SerializeReference] public CityBuilding mage_tower_lvl_6 = new();
    [SerializeReference] public CityBuilding mage_tower_lvl_7 = new();
    [SerializeReference] public CityBuilding mage_tower_lvl_8 = new();

    [Header("Main District")]
    [SerializeReference] public CityBuilding blacksmith = new();
    [SerializeReference] public CityBuilding market = new();
    [SerializeReference] public CityBuilding hero_altar = new();

    [Header("District I")]
    [SerializeReference] public CityBuilding district_i = new();
    [SerializeReference] public CityBuilding creatures_i_1 = new();
    [SerializeReference] public CityBuilding creatures_i_2 = new();
    [SerializeReference] public CityBuilding basic_resources_i = new();

    [Header("District II")]
    [SerializeReference] public CityBuilding district_ii = new();
    [SerializeReference] public CityBuilding creatures_ii_1 = new();
    [SerializeReference] public CityBuilding creatures_ii_2 = new();
    [SerializeReference] public CityBuilding basic_resources_ii = new();

    [Header("District III")]
    [SerializeReference] public CityBuilding district_iii = new();
    [SerializeReference] public CityBuilding creatures_iii_1 = new();
    [SerializeReference] public CityBuilding creatures_iii_2 = new();
    [SerializeReference] public CityBuilding special_resources_i = new();

    [Header("District IV")]
    [SerializeReference] public CityBuilding district_iv = new();
    [SerializeReference] public CityBuilding creatures_iv_1 = new();
    [SerializeReference] public CityBuilding creatures_iv_2 = new();
    [SerializeReference] public CityBuilding special_resources_ii = new();
    [SerializeReference] public CityBuilding faction_special = new();

    [Header("Defence")]
    [SerializeReference] public CityDefence fort2 = new();
    [SerializeReference] public CityDefence citadel2 = new();
    [SerializeReference] public CityDefence stronghold2 = new();

    [HideInInspector] public bool has_built_this_round = false;
    [HideInInspector] public List<CityBuilding> buildings = new();
    
    private UICityMenue city_menue;
    private CameraMovement cam_movemement;
    private float cam_zoom_backup;
    private bool mouse_over = false;
    private Mouse mouse = Mouse.current;
    private RoundManager round_manager;
    private UnitSelectionManager selection_manager;

    public new void Start() {
        // Connect to unit selection manager:
        selection_manager = UnitSelectionManager.Connect();
        
        // Connect to round manager:
        round_manager = RoundManager.Connect();
        
        // Connect to the main cam:
        cam_movemement = CameraMovement.Connect();
        
        // Connect to UI:
        city_menue = UIManager.ConnectCityMenue();
        
        // Adjust reveal range according to base buildings:
        AdjustLightRange();
        
        // On Start switch all lights off:
        SwitchLight(false);
        if(owner != null) {
            owner.RegisterBuilding(this);
        }
        
        // Add all buildings to a comprehensive list:
        buildings.Add(camp);
        buildings.Add(village);
        buildings.Add(town);
        buildings.Add(city);
        buildings.Add(monopolis);
        buildings.Add(mage_tower_lvl_1);
        buildings.Add(mage_tower_lvl_2);
        buildings.Add(mage_tower_lvl_3);
        buildings.Add(mage_tower_lvl_4);
        buildings.Add(mage_tower_lvl_5);
        buildings.Add(mage_tower_lvl_6);
        buildings.Add(mage_tower_lvl_7);
        buildings.Add(mage_tower_lvl_8);
        buildings.Add(blacksmith);
        buildings.Add(market);
        buildings.Add(hero_altar);
        buildings.Add(district_i);
        buildings.Add(creatures_i_1);
        buildings.Add(creatures_i_2);
        buildings.Add(basic_resources_i);
        buildings.Add(district_ii);
        buildings.Add(creatures_ii_1);
        buildings.Add(creatures_ii_2);
        buildings.Add(basic_resources_ii);
        buildings.Add(district_iii);
        buildings.Add(creatures_iii_1);
        buildings.Add(creatures_iii_2);
        buildings.Add(special_resources_i);
        buildings.Add(district_iv);
        buildings.Add(creatures_iv_1);
        buildings.Add(creatures_iv_2);
        buildings.Add(special_resources_ii);
        buildings.Add(faction_special);

        // Show only buildings that are built:
        foreach(CityBuilding building in buildings) {
            building.representation.SetActive(building.is_built);
        }

        // Walls are treated differently, so handle them here:
        // Show fort walls:
        foreach(GameObject wall in fort2.walls) {
            wall.SetActive(fort2.is_built);
        }

        // Show citadel walls:
        foreach(GameObject wall in citadel2.walls) {
            wall.SetActive(citadel2.is_built);
        }

        // Show stronghold walls:
        foreach(GameObject wall in stronghold2.walls) {
            wall.SetActive(stronghold2.is_built);
        }
    }

    public void AdjustLightRange() {
        reveal_radius = 10;
        if(camp.is_built) reveal_radius = 20;
        if(village.is_built) reveal_radius = 30;
        if(town.is_built) reveal_radius = 40;
        if(city.is_built) reveal_radius = 50;
        if(monopolis.is_built) reveal_radius = 60;
        SwitchLight(true);
    }

    public void AddResource(ResourceType type, int amnt) {
        switch(type) {
            case ResourceType.wood: wood += amnt; break;
            case ResourceType.food: food += amnt; break;
            case ResourceType.iron: iron += amnt; break;
            case ResourceType.stone: stone += amnt; break;
            case ResourceType.gun_powder: gun_powder += amnt; break;
            case ResourceType.pure_silver: pure_silver += amnt; break;
            case ResourceType.mana_essence: mana_essence += amnt; break;
            case ResourceType.void_crystal: void_crystal += amnt; break;
            case ResourceType.sacrificial_blood: sacrificial_blood += amnt; break;
            default: break;
        }

        // Update Resource Display:
        // TODO...
    }

    public bool CanBuild(CityBuildingCost costs) {
        // Check if resources are missing:
        if(wood < costs.cost_wood) return false;
        if(food < costs.cost_food) return false;
        if(iron < costs.cost_iron) return false;
        if(stone < costs.cost_stone) return false;
        if(gun_powder < costs.cost_gunpowder) return false;
        if(pure_silver < costs.cost_puresilver) return false;
        if(mana_essence < costs.cost_manaessence) return false;
        if(void_crystal < costs.cost_voidcrystal) return false;
        if(sacrificial_blood < costs.cost_sacrificialblood) return false;

        // All checks pass, building can be acquired:
        return true;
    }

    public void PayResources(CityBuildingCost costs) {
        AddResource(ResourceType.wood, -costs.cost_wood);
        AddResource(ResourceType.food, -costs.cost_food);
        AddResource(ResourceType.iron, -costs.cost_iron);
        AddResource(ResourceType.stone, -costs.cost_stone);
        AddResource(ResourceType.gun_powder, -costs.cost_gunpowder);
        AddResource(ResourceType.pure_silver, -costs.cost_puresilver);
        AddResource(ResourceType.mana_essence, -costs.cost_manaessence);
        AddResource(ResourceType.void_crystal, -costs.cost_voidcrystal);
        AddResource(ResourceType.sacrificial_blood, -costs.cost_sacrificialblood);
    }

    public void PayBuild(CityBuilding building) {
        // Don't pay anything if the building already exists:
        if(building.is_built) return;
        
        PayResources(building.costs);
        building.Build();
    }

    public void PayDefenseBuild(CityDefence building) {
        // Don't pay anything if the building already exists:
        if(building.is_built) return;

        PayResources(building.costs);
        building.Build();
    }

    public void Build(CityBuildingType type) {
        switch(type) {
            case CityBuildingType.camp: PayBuild(camp); break;
            case CityBuildingType.village: PayBuild(village); break;
            case CityBuildingType.town: PayBuild(town); break;
            case CityBuildingType.city: PayBuild(city); break;
            case CityBuildingType.monopolis: PayBuild(monopolis); break;
            case CityBuildingType.mage_tower_lvl_1: PayBuild(mage_tower_lvl_1); break;
            case CityBuildingType.mage_tower_lvl_2: PayBuild(mage_tower_lvl_2); break;
            case CityBuildingType.mage_tower_lvl_3: PayBuild(mage_tower_lvl_3); break;
            case CityBuildingType.mage_tower_lvl_4: PayBuild(mage_tower_lvl_4); break;
            case CityBuildingType.mage_tower_lvl_5: PayBuild(mage_tower_lvl_5); break;
            case CityBuildingType.mage_tower_lvl_6: PayBuild(mage_tower_lvl_6); break;
            case CityBuildingType.mage_tower_lvl_7: PayBuild(mage_tower_lvl_7); break;
            case CityBuildingType.mage_tower_lvl_8: PayBuild(mage_tower_lvl_8); break;
            case CityBuildingType.blacksmith: PayBuild(blacksmith); break;
            case CityBuildingType.market: PayBuild(market); break;
            case CityBuildingType.hero_altar: PayBuild(hero_altar); break;
            case CityBuildingType.district_i: PayBuild(district_i); break;
            case CityBuildingType.creatures_i_1: PayBuild(creatures_i_1); break;
            case CityBuildingType.creatures_i_2: PayBuild(creatures_i_2); break;
            case CityBuildingType.basic_resources_i: PayBuild(basic_resources_i); break;
            case CityBuildingType.district_ii: PayBuild(district_ii); break;
            case CityBuildingType.creatures_ii_1: PayBuild(creatures_ii_1); break;
            case CityBuildingType.creatures_ii_2: PayBuild(creatures_ii_2); break;
            case CityBuildingType.basic_resources_ii: PayBuild(basic_resources_ii); break;
            case CityBuildingType.district_iii: PayBuild(district_iii); break;
            case CityBuildingType.creatures_iii_1: PayBuild(creatures_iii_1); break;
            case CityBuildingType.creatures_iii_2: PayBuild(creatures_iii_2); break;
            case CityBuildingType.special_resources_i: PayBuild(special_resources_i); break;
            case CityBuildingType.district_iv: PayBuild(district_iv); break;
            case CityBuildingType.creatures_iv_1: PayBuild(creatures_iv_1); break;
            case CityBuildingType.creatures_iv_2: PayBuild(creatures_iv_2); break;
            case CityBuildingType.special_resources_ii: PayBuild(special_resources_ii); break;
            case CityBuildingType.faction_special: PayBuild(faction_special); break;
            case CityBuildingType.fort: PayDefenseBuild(fort2); break;
            case CityBuildingType.citadel: PayDefenseBuild(citadel2); break;
            case CityBuildingType.stronghold: PayDefenseBuild(stronghold2); break;
        }
        has_built_this_round = true;
    }

    public void CamToFocus() {
        cam_zoom_backup = cam_movemement.GetZoom();
        cam_movemement.FocusOn(focus_point, false);
        cam_movemement.SetZoom(zoom_value);
    }

    public new void EnterBuilding(Player player) {
        if(player == owner) {
            CamToFocus();
            city_menue.OnEnter(this);
        } else {
            Debug.Log($"You conquered the building {gameObject.name}!");
            
            // Remove this building from a players list of buildings, if owned:
            if(owner != null) {
                owner.UnregisterBuilding(this);
            }
            
            // Change the owner of the city to be the conqueror:
            owner = player;
            owner.RegisterBuilding(this);

            // Light the city:
            SwitchLight(true);

            // Also enter the city menue:
            CamToFocus();
            city_menue.OnEnter(this);
        }
    }

    public void UnitEnterBuilding(Unit unit) {
        // A unit enters the city and opens the city-unit interface:
        
    }

    public void ExitBuilding() {
        cam_movemement.SetZoom(cam_zoom_backup);
        // TODO: Maybe focus on the entrance trigger afterwards...
    }

    public void OnMouseEnter() {
        mouse_over = true;
    }

    public void OnMouseExit() {
        mouse_over = false;
    }

    public void Update() {
        // If there is a unit selected, don't be able to enter city menue by clicking:
        if(selection_manager.current_selection != null) return;

        // City selection by click:
        if(mouse_over && mouse.leftButton.wasPressedThisFrame) {
            if(round_manager.current_player == this.owner) {
                EnterBuilding(round_manager.current_player);
            } else {
                Debug.Log("This is NOT your city!");
            }
        }
    }
}

public class CityBuilding {
    public GameObject representation;
    public string building_name = string.Empty;
    public bool is_built = false;
    public CityBuildingType type;
    [SerializeReference] public CityBuildingCost costs = new();

    public void Build() {
        representation.SetActive(true);
        is_built = true;
    }
}

public class CityDefence {
    public List<GameObject> walls;
    public string building_name = string.Empty;
    public bool is_built = false;
    public CityBuildingType type = CityBuildingType.fort;
    [SerializeReference] public CityBuildingCost costs = new();

    public void Build() {
        foreach(var wall in walls) {
            wall.SetActive(true);
        }
        is_built = true;
    }
}

public class CityBuildingCost {
    public int cost_wood = 0;
    public int cost_food = 0;
    public int cost_iron = 0;
    public int cost_stone = 0;
    public int cost_gunpowder = 0;
    public int cost_puresilver = 0;
    public int cost_voidcrystal = 0;
    public int cost_manaessence = 0;
    public int cost_sacrificialblood = 0;
}

[System.Serializable]
public enum CityBuildingType {
    camp,
    village,
    town,
    city,
    monopolis,
    mage_tower_lvl_1,
    mage_tower_lvl_2,
    mage_tower_lvl_3,
    mage_tower_lvl_4,
    mage_tower_lvl_5,
    mage_tower_lvl_6,
    mage_tower_lvl_7,
    mage_tower_lvl_8,
    blacksmith,
    market,
    hero_altar,
    district_i,
    creatures_i_1,
    creatures_i_2,
    basic_resources_i,
    district_ii,
    creatures_ii_1,
    creatures_ii_2,
    basic_resources_ii,
    district_iii,
    creatures_iii_1,
    creatures_iii_2,
    special_resources_i,
    district_iv,
    creatures_iv_1,
    creatures_iv_2,
    special_resources_ii,
    faction_special,
    fort,
    citadel,
    stronghold,
}