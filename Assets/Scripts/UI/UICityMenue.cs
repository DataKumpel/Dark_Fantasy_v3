using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UICityMenue : MonoBehaviour {
    public Text name_display;
    public GameObject main_group;
    
    [Header("Building Sprites")]
    public Sprite can_build_sprite;
    public Sprite cannot_build_sprite;
    public Sprite already_built_sprite;
    public Sprite wait_build_sprite;

    [Header("Recruitation Sprites")]
    public Sprite can_recruit_sprite;
    public Sprite cannot_recruit_sprite;
    public Sprite soldout_sprite;

    [Header("Resource Display")]
    public UIResourceDisplay resource_display;

    [Header("City Unit Exchange Display")]
    public GameObject city_unit_exchange_display;

    [Header("UI Groups")]
    [SerializeReference] public UICityMenueTexts ui_texts = new();
    [SerializeReference] public UICityMenueButtons ui_buttons = new();
    [SerializeReference] public UICityMenueItems ui_items = new();

    private GameObject active_group;
    private City current_city;
    private Mouse mouse = Mouse.current;

    public void Start() {
        active_group = main_group;
    }

    public void ToGroup(GameObject group) {
        // Active group might be null on first start of the game:
        if(active_group != null) active_group.SetActive(false);

        active_group = group;
        active_group.SetActive(true);
    }

    public void ConstructBuilding(CityBuildingType type) {
        current_city.Build(type);

        // Change the appearance of the build button to already built:
        ui_buttons.ChangeButtonSprite(type, already_built_sprite);

        // There can only be one construction per turn, 
        // so disable all buttons after construction:
        UpdateAllButtons();

        // Update UI items dependency:
        ui_items.UpdateStatus(current_city);

        // Update the resource display with what is left:
        resource_display.UpdateDisplays(current_city);
    }

    // Since Buttons don't recognize enums (08.02.2023) create a function for every enum value:
    public void ConstructCamp() => ConstructBuilding(CityBuildingType.camp);
    public void ConstructVillage() => ConstructBuilding(CityBuildingType.village);
    public void ConstructTown() => ConstructBuilding(CityBuildingType.town);
    public void ConstructCity() => ConstructBuilding(CityBuildingType.city);
    public void ConstructMonopolis() => ConstructBuilding(CityBuildingType.monopolis);
    public void ConstructMageTowerLvl_1() => ConstructBuilding(CityBuildingType.mage_tower_lvl_1);
    public void ConstructMageTowerLvl_2() => ConstructBuilding(CityBuildingType.mage_tower_lvl_2);
    public void ConstructMageTowerLvl_3() => ConstructBuilding(CityBuildingType.mage_tower_lvl_3);
    public void ConstructMageTowerLvl_4() => ConstructBuilding(CityBuildingType.mage_tower_lvl_4);
    public void ConstructMageTowerLvl_5() => ConstructBuilding(CityBuildingType.mage_tower_lvl_5);
    public void ConstructMageTowerLvl_6() => ConstructBuilding(CityBuildingType.mage_tower_lvl_6);
    public void ConstructMageTowerLvl_7() => ConstructBuilding(CityBuildingType.mage_tower_lvl_7);
    public void ConstructMageTowerLvl_8() => ConstructBuilding(CityBuildingType.mage_tower_lvl_8);
    public void ConstructBlacksmith() => ConstructBuilding(CityBuildingType.blacksmith);
    public void ConstructMarket() => ConstructBuilding(CityBuildingType.market);
    public void ConstructHeroAltar() => ConstructBuilding(CityBuildingType.hero_altar);
    public void ConstructDistrict_I() => ConstructBuilding(CityBuildingType.district_i);
    public void ConstructDistrict_II() => ConstructBuilding(CityBuildingType.district_ii);
    public void ConstructDistrict_III() => ConstructBuilding(CityBuildingType.district_iii);
    public void ConstructDistrict_IV() => ConstructBuilding(CityBuildingType.district_iv);
    public void ConstructCreatures_I_1() => ConstructBuilding(CityBuildingType.creatures_i_1);
    public void ConstructCreatures_I_2() => ConstructBuilding(CityBuildingType.creatures_i_2);
    public void ConstructCreatures_II_1() => ConstructBuilding(CityBuildingType.creatures_ii_1);
    public void ConstructCreatures_II_2() => ConstructBuilding(CityBuildingType.creatures_ii_2);
    public void ConstructCreatures_III_1() => ConstructBuilding(CityBuildingType.creatures_iii_1);
    public void ConstructCreatures_III_2() => ConstructBuilding(CityBuildingType.creatures_iii_2);
    public void ConstructCreatures_IV_1() => ConstructBuilding(CityBuildingType.creatures_iv_1);
    public void ConstructCreatures_IV_2() => ConstructBuilding(CityBuildingType.creatures_iv_2);
    public void ConstructBasicResource_I() => ConstructBuilding(CityBuildingType.basic_resources_i);
    public void ConstructBasicResource_II() => ConstructBuilding(CityBuildingType.basic_resources_ii);
    public void ConstructSpecialResource_I() => ConstructBuilding(CityBuildingType.special_resources_i);
    public void ConstructSpecialResource_II() => ConstructBuilding(CityBuildingType.special_resources_ii);
    public void ConstructFactionSpecial() => ConstructBuilding(CityBuildingType.faction_special);
    public void ConstructFort() => ConstructBuilding(CityBuildingType.fort);
    public void ConstructCitadel() => ConstructBuilding(CityBuildingType.citadel);
    public void ConstructStronghold() => ConstructBuilding(CityBuildingType.stronghold);

    // Also have a function for every construction cost info:
    public void ShowCampCosts() => resource_display.ShowCosts(current_city.camp.costs);
    public void ShowVillageCosts() => resource_display.ShowCosts(current_city.village.costs);
    public void ShowTownCosts() => resource_display.ShowCosts(current_city.town.costs);
    public void ShowCityCosts() => resource_display.ShowCosts(current_city.city.costs);
    public void ShowMonopolisCosts() => resource_display.ShowCosts(current_city.monopolis.costs);
    public void ShowMageTowerLvl_1Costs() => resource_display.ShowCosts(current_city.mage_tower_lvl_1.costs);
    public void ShowMageTowerLvl_2Costs() => resource_display.ShowCosts(current_city.mage_tower_lvl_2.costs);
    public void ShowMageTowerLvl_3Costs() => resource_display.ShowCosts(current_city.mage_tower_lvl_3.costs);
    public void ShowMageTowerLvl_4Costs() => resource_display.ShowCosts(current_city.mage_tower_lvl_4.costs);
    public void ShowMageTowerLvl_5Costs() => resource_display.ShowCosts(current_city.mage_tower_lvl_5.costs);
    public void ShowMageTowerLvl_6Costs() => resource_display.ShowCosts(current_city.mage_tower_lvl_6.costs);
    public void ShowMageTowerLvl_7Costs() => resource_display.ShowCosts(current_city.mage_tower_lvl_7.costs);
    public void ShowMageTowerLvl_8Costs() => resource_display.ShowCosts(current_city.mage_tower_lvl_8.costs);
    public void ShowBlacksmithCosts() => resource_display.ShowCosts(current_city.blacksmith.costs);
    public void ShowMarketCosts() => resource_display.ShowCosts(current_city.market.costs);
    public void ShowHeroAltarCosts() => resource_display.ShowCosts(current_city.hero_altar.costs);
    public void ShowDistrict_ICosts() => resource_display.ShowCosts(current_city.district_i.costs);
    public void ShowDistrict_IICosts() => resource_display.ShowCosts(current_city.district_ii.costs);
    public void ShowDistrict_IIICosts() => resource_display.ShowCosts(current_city.district_iii.costs);
    public void ShowDistrict_IVCosts() => resource_display.ShowCosts(current_city.district_iv.costs);
    public void ShowCreatures_I_1Costs() => resource_display.ShowCosts(current_city.creatures_i_1.costs);
    public void ShowCreatures_I_2Costs() => resource_display.ShowCosts(current_city.creatures_i_2.costs);
    public void ShowCreatures_II_1Costs() => resource_display.ShowCosts(current_city.creatures_ii_1.costs);
    public void ShowCreatures_II_2Costs() => resource_display.ShowCosts(current_city.creatures_ii_2.costs);
    public void ShowCreatures_III_1Costs() => resource_display.ShowCosts(current_city.creatures_iii_1.costs);
    public void ShowCreatures_III_2Costs() => resource_display.ShowCosts(current_city.creatures_iii_2.costs);
    public void ShowCreatures_IV_1Costs() => resource_display.ShowCosts(current_city.creatures_iv_1.costs);
    public void ShowCreatures_IV_2Costs() => resource_display.ShowCosts(current_city.creatures_iv_2.costs);
    public void ShowBasicResource_ICosts() => resource_display.ShowCosts(current_city.basic_resources_i.costs);
    public void ShowBasicResource_IICosts() => resource_display.ShowCosts(current_city.basic_resources_ii.costs);
    public void ShowSpecialResource_ICosts() => resource_display.ShowCosts(current_city.special_resources_i.costs);
    public void ShowSpecialResource_IICosts() => resource_display.ShowCosts(current_city.special_resources_ii.costs);
    public void ShowFactionSpecialCosts() => resource_display.ShowCosts(current_city.faction_special.costs);
    public void ShowFortCosts() => resource_display.ShowCosts(current_city.fort2.costs);
    public void ShowCitadelCosts() => resource_display.ShowCosts(current_city.citadel2.costs);
    public void ShowStrongholdCosts() => resource_display.ShowCosts(current_city.stronghold2.costs);

    // Functions for creature recruitation costs:
    public void ShowRecruitCreature_I_1Costs() => resource_display.ShowCosts(current_city.recr_creature_i_1.costs);
    public void ShowRecruitCreature_I_2Costs() => resource_display.ShowCosts(current_city.recr_creature_i_2.costs);
    public void ShowRecruitCreature_II_1Costs() => resource_display.ShowCosts(current_city.recr_creature_ii_1.costs);
    public void ShowRecruitCreature_II_2Costs() => resource_display.ShowCosts(current_city.recr_creature_ii_2.costs);
    public void ShowRecruitCreature_III_1Costs() => resource_display.ShowCosts(current_city.recr_creature_iii_1.costs);
    public void ShowRecruitCreature_III_2Costs() => resource_display.ShowCosts(current_city.recr_creature_iii_2.costs);
    public void ShowRecruitCreature_IV_Costs() {
        if(current_city.creatures_iv_1.is_built) {
            resource_display.ShowCosts(current_city.recr_creature_iv_1.costs);
        } else {
            resource_display.ShowCosts(current_city.recr_creature_iv_2.costs);
        }
    }

    public void HideCosts() => resource_display.HideCosts();

    // For simplicity of life :D 
    public void UpdateAllButtons() => ui_buttons.UpdateAllButtonStatus(current_city, can_build_sprite, 
                                                                       cannot_build_sprite, already_built_sprite, 
                                                                       wait_build_sprite);
    public void UpdateAllRecruitButtons() => ui_buttons.UpdateAllRecruitButtons(current_city, can_recruit_sprite,
                                                                                cannot_recruit_sprite,
                                                                                soldout_sprite);

    public void Recruit(CreatureType type) {
        // Show recruitment dialog:
        UIManager.ConnectRecruitmentDialog().OpenDialog(current_city, type);
    }

    // Same Problem with the enums:
    public void RecruitCreature_I_1() => Recruit(CreatureType.creature_tier_i_1);
    public void RecruitCreature_I_2() => Recruit(CreatureType.creature_tier_i_2);
    public void RecruitCreature_II_1() => Recruit(CreatureType.creature_tier_i_1);
    public void RecruitCreature_II_2() => Recruit(CreatureType.creature_tier_i_2);
    public void RecruitCreature_III_1() => Recruit(CreatureType.creature_tier_i_1);
    public void RecruitCreature_III_2() => Recruit(CreatureType.creature_tier_i_2);
    public void RecruitCreature_IV() => Recruit(CreatureType.creature_tier_iv);
    
    public void PrepareUI() {
        // Display the name of the current city:
        name_display.text = current_city.city_name;

        // Update all displayed text to the faction names of current city:
        ui_texts.UpdateTextDisplays(current_city);

        // Update all build buttons status for this city:
        UpdateAllButtons();

        // Update all recruit buttons status for this city:
        UpdateAllRecruitButtons();

        // Show or hide elements of the GUI dependent on the current build status:
        ui_items.UpdateStatus(current_city);

        // Also show the resource panel:
        resource_display.UpdateDisplays(current_city);
        resource_display.HideCosts();
        resource_display.gameObject.SetActive(true);
    }

    public void OnEnter(City city) {
        // Get the new reference city to build the UI on:
        current_city = city;

        // Update the display by informations of the new city:
        PrepareUI();

        // Show the UI always on main group:
        gameObject.SetActive(true);
        ToGroup(main_group);

        // Show city to unit exchange menue:
        city_unit_exchange_display.SetActive(true);
    }

    public void OnExit() {
        current_city.ExitBuilding();
        resource_display.HideCosts();
        resource_display.gameObject.SetActive(false);
        gameObject.SetActive(false);
        city_unit_exchange_display.SetActive(false);
    }

    public void Update() {
        if(gameObject.activeSelf) {
            if(mouse.rightButton.wasPressedThisFrame) {
                OnExit();
            }
        }
    }
}

//===== UI DISPLAY ITEM ===============================================================================================

// TODO: This class maybe better deposited in a uitility module... (how do you even do that?!?)
public class UIDisplayItem {
    public List<GameObject> subitems = new();

    public void SetActive(bool val) {
        foreach(var item in subitems) {
            item.SetActive(val);
        }
    }
}

//===== UI CITY MENUE ITEMS ===========================================================================================

public class UICityMenueItems {
    [Header("Main Building Elements")]
    [SerializeReference] public UIDisplayItem camp = new();
    [SerializeReference] public UIDisplayItem village = new();
    [SerializeReference] public UIDisplayItem town = new();
    [SerializeReference] public UIDisplayItem city = new();
    [SerializeReference] public UIDisplayItem monopolis = new();

    [Header("Mage Tower Elements")]
    [SerializeReference] public UIDisplayItem mage_tower_group = new();
    [SerializeReference] public UIDisplayItem mage_tower_lvl_1 = new();
    [SerializeReference] public UIDisplayItem mage_tower_lvl_2 = new();
    [SerializeReference] public UIDisplayItem mage_tower_lvl_3 = new();
    [SerializeReference] public UIDisplayItem mage_tower_lvl_4 = new();
    [SerializeReference] public UIDisplayItem mage_tower_lvl_5 = new();
    [SerializeReference] public UIDisplayItem mage_tower_lvl_6 = new();
    [SerializeReference] public UIDisplayItem mage_tower_lvl_7 = new();
    [SerializeReference] public UIDisplayItem mage_tower_lvl_8 = new();

    [Header("Main District Elements")]
    [SerializeReference] public UIDisplayItem blacksmith = new();
    [SerializeReference] public UIDisplayItem market = new();
    [SerializeReference] public UIDisplayItem hero_altar = new();

    [Header("District I Elements")]
    [SerializeReference] public UIDisplayItem district_i = new();
    [SerializeReference] public UIDisplayItem creatures_i_1 = new();
    [SerializeReference] public UIDisplayItem creatures_i_2 = new();
    [SerializeReference] public UIDisplayItem basic_resources_i = new();

    [Header("District II Elements")]
    [SerializeReference] public UIDisplayItem district_ii = new();
    [SerializeReference] public UIDisplayItem creatures_ii_1 = new();
    [SerializeReference] public UIDisplayItem creatures_ii_2 = new();
    [SerializeReference] public UIDisplayItem basic_resources_ii = new();

    [Header("District III Elements")]
    [SerializeReference] public UIDisplayItem district_iii = new();
    [SerializeReference] public UIDisplayItem creatures_iii_1 = new();
    [SerializeReference] public UIDisplayItem creatures_iii_2 = new();
    [SerializeReference] public UIDisplayItem special_resources_i = new();

    [Header("District IV Elements")]
    [SerializeReference] public UIDisplayItem district_iv = new();
    [SerializeReference] public UIDisplayItem creatures_iv_1 = new();
    [SerializeReference] public UIDisplayItem creatures_iv_2 = new();
    [SerializeReference] public UIDisplayItem special_resources_ii = new();
    [SerializeReference] public UIDisplayItem faction_special = new();

    [Header("Defense Elements")]
    [SerializeReference] public UIDisplayItem fortification_group = new();
    [SerializeReference] public UIDisplayItem fort = new();
    [SerializeReference] public UIDisplayItem citadel = new();
    [SerializeReference] public UIDisplayItem stronghold = new();

    [Header("Recruitation Elements")]
    [SerializeReference] public UIDisplayItem recruite_creatures_i_1 = new();
    [SerializeReference] public UIDisplayItem recruite_creatures_i_2 = new();
    [SerializeReference] public UIDisplayItem recruite_creatures_ii_1 = new();
    [SerializeReference] public UIDisplayItem recruite_creatures_ii_2 = new();
    [SerializeReference] public UIDisplayItem recruite_creatures_iii_1 = new();
    [SerializeReference] public UIDisplayItem recruite_creatures_iii_2 = new();
    [SerializeReference] public UIDisplayItem recruite_creatures_iv = new();

    public void UpdateStatus(City city) {
        // Update visibility of UI items depending on building status:
        
        // The camp is always visible (first possible building)
        camp.SetActive(true);

        // Base dependency:
        village.SetActive(city.camp.is_built);
        town.SetActive(city.village.is_built);
        this.city.SetActive(city.town.is_built);
        monopolis.SetActive(city.city.is_built);
        
        // MainDistrict dependency:
        market.SetActive(city.village.is_built);
        blacksmith.SetActive(city.village.is_built);
        hero_altar.SetActive(city.village.is_built);

        // District I dependency:
        district_i.SetActive(city.village.is_built);
        creatures_i_1.SetActive(city.district_i.is_built);
        creatures_i_2.SetActive(city.district_i.is_built);
        basic_resources_i.SetActive(city.district_i.is_built);
        district_ii.SetActive(city.district_i.is_built);

        // District II dependency:
        district_ii.SetActive(city.town.is_built && city.district_i.is_built);
        creatures_ii_1.SetActive(city.district_ii.is_built);
        creatures_ii_2.SetActive(city.district_ii.is_built);
        basic_resources_ii.SetActive(city.district_ii.is_built);

        // District III dependency:
        district_iii.SetActive(city.city.is_built && city.district_ii.is_built);
        creatures_iii_1.SetActive(city.district_iii.is_built);
        creatures_iii_2.SetActive(city.district_iii.is_built);
        special_resources_i.SetActive(city.district_iii.is_built);

        // District IV dependency:
        district_iv.SetActive(city.monopolis.is_built && city.district_iii.is_built);
        creatures_iv_1.SetActive(city.district_iv.is_built && !city.creatures_iv_2.is_built);
        creatures_iv_2.SetActive(city.district_iv.is_built && !city.creatures_iv_1.is_built);
        special_resources_ii.SetActive(city.district_iv.is_built);
        faction_special.SetActive(city.district_iv.is_built);

        // Magetower dependency:
        mage_tower_group.SetActive(city.village.is_built);
        mage_tower_lvl_1.SetActive(city.village.is_built);
        mage_tower_lvl_2.SetActive(city.mage_tower_lvl_1.is_built);
        mage_tower_lvl_3.SetActive(city.mage_tower_lvl_2.is_built && city.town.is_built);
        mage_tower_lvl_4.SetActive(city.mage_tower_lvl_3.is_built);
        mage_tower_lvl_5.SetActive(city.mage_tower_lvl_4.is_built && city.city.is_built);
        mage_tower_lvl_6.SetActive(city.mage_tower_lvl_5.is_built);
        mage_tower_lvl_7.SetActive(city.mage_tower_lvl_6.is_built && city.monopolis.is_built);
        mage_tower_lvl_8.SetActive(city.mage_tower_lvl_7.is_built);

        // Fotification dependency:
        fortification_group.SetActive(city.town.is_built);
        fort.SetActive(city.town.is_built);
        citadel.SetActive(city.fort2.is_built && city.city.is_built);
        stronghold.SetActive(city.citadel2.is_built && city.monopolis.is_built);

        // Recruitation dependency:
        recruite_creatures_i_1.SetActive(city.creatures_i_1.is_built);
        recruite_creatures_i_2.SetActive(city.creatures_i_2.is_built);
        recruite_creatures_ii_1.SetActive(city.creatures_ii_1.is_built);
        recruite_creatures_ii_2.SetActive(city.creatures_ii_2.is_built);
        recruite_creatures_iii_1.SetActive(city.creatures_iii_1.is_built);
        recruite_creatures_iii_2.SetActive(city.creatures_iii_2.is_built);
        recruite_creatures_iv.SetActive(city.creatures_iv_1.is_built || city.creatures_iv_2.is_built);
    }
}

//===== UI CITY MENUE BUTTONS =========================================================================================

public class UICityMenueButtons {
    [Header("Main Building Build Buttons")]
    public Button camp_build_btn;
    public Button village_build_btn;
    public Button town_build_btn;
    public Button city_build_btn;
    public Button monopolis_build_btn;

    [Header("Mage Tower Build Buttons")]
    public Button mage_tower_lvl_1_build_btn;
    public Button mage_tower_lvl_2_build_btn;
    public Button mage_tower_lvl_3_build_btn;
    public Button mage_tower_lvl_4_build_btn;
    public Button mage_tower_lvl_5_build_btn;
    public Button mage_tower_lvl_6_build_btn;
    public Button mage_tower_lvl_7_build_btn;
    public Button mage_tower_lvl_8_build_btn;

    [Header("Main District Build Buttons")]
    public Button blacksmith_build_btn;
    public Button market_build_btn;
    public Button hero_altar_build_btn;

    [Header("District I")]
    public Button district_i_build_btn;
    public Button creatures_i_1_build_btn;
    public Button creatures_i_2_build_btn;
    public Button basic_resources_i_build_btn;

    [Header("District II")]
    public Button district_ii_build_btn;
    public Button creatures_ii_1_build_btn;
    public Button creatures_ii_2_build_btn;
    public Button basic_resources_ii_build_btn;

    [Header("District III")]
    public Button district_iii_build_btn;
    public Button creatures_iii_1_build_btn;
    public Button creatures_iii_2_build_btn;
    public Button special_resources_i_build_btn;

    [Header("District IV")]
    public Button district_iv_build_btn;
    public Button creatures_iv_1_build_btn;
    public Button creatures_iv_2_build_btn;
    public Button special_resources_ii_build_btn;
    public Button faction_special_build_btn;

    [Header("Defense")]
    public Button fort_build_btn;
    public Button citadel_build_btn;
    public Button stronghold_build_btn;

    [Header("Recruitation")]
    public Button creature_i_1_recruite_btn;
    public Button creature_i_2_recruite_btn;
    public Button creature_ii_1_recruite_btn;
    public Button creature_ii_2_recruite_btn;
    public Button creature_iii_1_recruite_btn;
    public Button creature_iii_2_recruite_btn;
    public Button creature_iv_recruite_btn;

    public void ChangeButtonSprite(CityBuildingType type, Sprite sprite) {
        ChangeButtonSprite(GetButtonFromType(type), sprite);
    }

    public void ChangeButtonSprite(Button btn, Sprite sprite) {
        var btn_img = btn.transform.GetChild(0).GetComponent<Image>();
        btn_img.sprite = sprite;
    }

    public Button GetButtonFromType(CityBuildingType type) {
        switch(type) {
            case CityBuildingType.camp: return camp_build_btn;
            case CityBuildingType.village: return village_build_btn;
            case CityBuildingType.town: return town_build_btn;
            case CityBuildingType.city: return city_build_btn;
            case CityBuildingType.monopolis: return monopolis_build_btn;
            case CityBuildingType.mage_tower_lvl_1: return mage_tower_lvl_1_build_btn;
            case CityBuildingType.mage_tower_lvl_2: return mage_tower_lvl_2_build_btn;
            case CityBuildingType.mage_tower_lvl_3: return mage_tower_lvl_3_build_btn;
            case CityBuildingType.mage_tower_lvl_4: return mage_tower_lvl_4_build_btn;
            case CityBuildingType.mage_tower_lvl_5: return mage_tower_lvl_5_build_btn;
            case CityBuildingType.mage_tower_lvl_6: return mage_tower_lvl_6_build_btn;
            case CityBuildingType.mage_tower_lvl_7: return mage_tower_lvl_7_build_btn;
            case CityBuildingType.mage_tower_lvl_8: return mage_tower_lvl_8_build_btn;
            case CityBuildingType.blacksmith: return blacksmith_build_btn;
            case CityBuildingType.market: return market_build_btn;
            case CityBuildingType.hero_altar: return hero_altar_build_btn;
            case CityBuildingType.district_i: return district_i_build_btn;
            case CityBuildingType.creatures_i_1: return creatures_i_1_build_btn;
            case CityBuildingType.creatures_i_2: return creatures_i_2_build_btn;
            case CityBuildingType.basic_resources_i: return basic_resources_i_build_btn;
            case CityBuildingType.district_ii: return district_ii_build_btn;
            case CityBuildingType.creatures_ii_1: return creatures_ii_1_build_btn;
            case CityBuildingType.creatures_ii_2: return creatures_ii_2_build_btn;
            case CityBuildingType.basic_resources_ii: return basic_resources_ii_build_btn;
            case CityBuildingType.district_iii: return district_iii_build_btn;
            case CityBuildingType.creatures_iii_1: return creatures_iii_1_build_btn;
            case CityBuildingType.creatures_iii_2: return creatures_iii_2_build_btn;
            case CityBuildingType.special_resources_i: return special_resources_i_build_btn;
            case CityBuildingType.district_iv: return district_iv_build_btn;
            case CityBuildingType.creatures_iv_1: return creatures_iv_1_build_btn;
            case CityBuildingType.creatures_iv_2: return creatures_iv_2_build_btn;
            case CityBuildingType.special_resources_ii: return special_resources_ii_build_btn;
            case CityBuildingType.faction_special: return faction_special_build_btn;
            case CityBuildingType.fort: return fort_build_btn;
            case CityBuildingType.citadel: return citadel_build_btn;
            case CityBuildingType.stronghold: return stronghold_build_btn;
            default: return camp_build_btn;  // Required but never reached...
        }
    }

    public void UpdateDefenceButtonStatus(City city, CityDefence city_defence, 
                                          Sprite can_build, Sprite cannot_build, 
                                          Sprite already_built, Sprite wait_built) {
        var btn = GetButtonFromType(city_defence.type);
            
        if(city_defence.is_built) {
            // Buildings that are already built:
            ChangeButtonSprite(city_defence.type, already_built);
            btn.interactable = false;
        } else if(city.has_built_this_round) {
            // Buildings on wait this round:
            ChangeButtonSprite(city_defence.type, wait_built);
            btn.interactable = false;
        } else if(city.CanBuild(city_defence.costs)) {
            // Buildings that can be built this round:
            ChangeButtonSprite(city_defence.type, can_build);
            btn.interactable = true;
        } else {
            // Buildings that connot be built due to the lack of resources:
            ChangeButtonSprite(city_defence.type, cannot_build);
            btn.interactable = false;
        }
    }

    public void UpdateAllButtonStatus(City city, Sprite can_build, Sprite cannot_build, Sprite already_built, Sprite wait_built) {
        foreach(var building in city.buildings) {
            var btn = GetButtonFromType(building.type);
            
            if(building.is_built) {
                // Buildings that are already built:
                ChangeButtonSprite(building.type, already_built);
                btn.interactable = false;
            } else if(city.has_built_this_round) {
                // Buildings on wait this round:
                ChangeButtonSprite(building.type, wait_built);
                btn.interactable = false;
            } else if(city.CanBuild(building.costs)) {
                // Buildings that can be built this round:
                ChangeButtonSprite(building.type, can_build);
                btn.interactable = true;
            } else {
                // Buildings that connot be built due to the lack of resources:
                ChangeButtonSprite(building.type, cannot_build);
                btn.interactable = false;
            }
        }

        // Special treatment for the city defences:
        UpdateDefenceButtonStatus(city, city.fort2, can_build, cannot_build, already_built, wait_built);
        UpdateDefenceButtonStatus(city, city.citadel2, can_build, cannot_build, already_built, wait_built);
        UpdateDefenceButtonStatus(city, city.stronghold2, can_build, cannot_build, already_built, wait_built);
    }

    public void UpdateRecruitButton(Button btn, bool is_built, float stock, bool affordable, 
                                    Sprite can_recruit, Sprite cannot_recruit, Sprite soldout) {
        if(is_built) {
            if(Mathf.FloorToInt(stock) > 0) {
                if(affordable) {
                    ChangeButtonSprite(btn, can_recruit);
                } else {
                    ChangeButtonSprite(btn, cannot_recruit);
                }
            } else {
                ChangeButtonSprite(btn, soldout);
            }
        }
    }

    public void UpdateAllRecruitButtons(City city, Sprite can_recruit, Sprite cannot_recruit, Sprite soldout) {
        UpdateRecruitButton(creature_i_1_recruite_btn, city.creatures_i_1.is_built, city.creature_i_1_stock, 
                            city.CanRecruit(city.recr_creature_i_1.costs), can_recruit, cannot_recruit, soldout);
        UpdateRecruitButton(creature_i_2_recruite_btn, city.creatures_i_2.is_built, city.creature_i_2_stock, 
                            city.CanRecruit(city.recr_creature_i_2.costs), can_recruit, cannot_recruit, soldout);
        UpdateRecruitButton(creature_ii_1_recruite_btn, city.creatures_ii_1.is_built, city.creature_ii_1_stock, 
                            city.CanRecruit(city.recr_creature_ii_1.costs), can_recruit, cannot_recruit, soldout);
        UpdateRecruitButton(creature_ii_2_recruite_btn, city.creatures_ii_2.is_built, city.creature_ii_2_stock, 
                            city.CanRecruit(city.recr_creature_ii_2.costs), can_recruit, cannot_recruit, soldout);
        UpdateRecruitButton(creature_iii_1_recruite_btn, city.creatures_iii_1.is_built, city.creature_iii_1_stock, 
                            city.CanRecruit(city.recr_creature_iii_1.costs), can_recruit, cannot_recruit, soldout);
        UpdateRecruitButton(creature_iii_2_recruite_btn, city.creatures_iii_2.is_built, city.creature_iii_2_stock, 
                            city.CanRecruit(city.recr_creature_iii_2.costs), can_recruit, cannot_recruit, soldout);
        
        if(city.creatures_iv_1.is_built) {
            UpdateRecruitButton(creature_iv_recruite_btn, city.creatures_iv_1.is_built, city.creature_iv_1_stock, 
                                city.CanRecruit(city.recr_creature_iv_1.costs), can_recruit, cannot_recruit, soldout);
        } else {
            UpdateRecruitButton(creature_iv_recruite_btn, city.creatures_iv_2.is_built, city.creature_iv_2_stock, 
                                city.CanRecruit(city.recr_creature_iv_2.costs), can_recruit, cannot_recruit, soldout);
        }
    }
}

//===== UI CITY MENUE TEXTS ===========================================================================================

public class UICityMenueTexts {
    [Header("Main Building Displays")]
    public Text camp_disp;
    public Text village_disp;
    public Text town_disp;
    public Text city_disp;
    public Text monopolis_disp;

    [Header("Mage Tower Displays")]
    public Text mage_tower_lvl_1_disp;
    public Text mage_tower_lvl_2_disp;
    public Text mage_tower_lvl_3_disp;
    public Text mage_tower_lvl_4_disp;
    public Text mage_tower_lvl_5_disp;
    public Text mage_tower_lvl_6_disp;
    public Text mage_tower_lvl_7_disp;
    public Text mage_tower_lvl_8_disp;

    [Header("Main District Displays")]
    public Text blacksmith_disp;
    public Text market_disp;
    public Text hero_altar_disp;

    [Header("District I")]
    public Text district_i_disp;
    public Text creatures_i_1_disp;
    public Text creatures_i_2_disp;
    public Text basic_resources_i_disp;

    [Header("District II")]
    public Text district_ii_disp;
    public Text creatures_ii_1_disp;
    public Text creatures_ii_2_disp;
    public Text basic_resources_ii_disp;

    [Header("District III")]
    public Text district_iii_disp;
    public Text creatures_iii_1_disp;
    public Text creatures_iii_2_disp;
    public Text special_resources_i_disp;

    [Header("District IV")]
    public Text district_iv_disp;
    public Text creatures_iv_1_disp;
    public Text creatures_iv_2_disp;
    public Text special_resources_ii_disp;
    public Text faction_special_disp;

    [Header("Defense")]
    public Text fort_disp;
    public Text citadel_disp;
    public Text stronghold_disp;

    [Header("Recruitation")]
    public Text creature_i_1_disp;
    public Text creature_i_1_amnt_disp;
    public Text creature_i_2_disp;
    public Text creature_i_2_amnt_disp;
    public Text creature_ii_1_disp;
    public Text creature_ii_1_amnt_disp;
    public Text creature_ii_2_disp;
    public Text creature_ii_2_amnt_disp;
    public Text creature_iii_1_disp;
    public Text creature_iii_1_amnt_disp;
    public Text creature_iii_2_disp;
    public Text creature_iii_2_amnt_disp;
    public Text creature_iv_disp;
    public Text creature_iv_amnt_disp;

    public void UpdateTextDisplays(City city) {
        camp_disp.text = city.camp.building_name;
        village_disp.text = city.village.building_name;
        town_disp.text = city.town.building_name;
        city_disp.text = city.city.building_name;
        monopolis_disp.text = city.monopolis.building_name;
        mage_tower_lvl_1_disp.text = city.mage_tower_lvl_1.building_name;
        mage_tower_lvl_2_disp.text = city.mage_tower_lvl_2.building_name;
        mage_tower_lvl_3_disp.text = city.mage_tower_lvl_3.building_name;
        mage_tower_lvl_4_disp.text = city.mage_tower_lvl_4.building_name;
        mage_tower_lvl_5_disp.text = city.mage_tower_lvl_5.building_name;
        mage_tower_lvl_6_disp.text = city.mage_tower_lvl_6.building_name;
        mage_tower_lvl_7_disp.text = city.mage_tower_lvl_7.building_name;
        mage_tower_lvl_8_disp.text = city.mage_tower_lvl_8.building_name;
        blacksmith_disp.text = city.blacksmith.building_name;
        market_disp.text = city.market.building_name;
        hero_altar_disp.text = city.hero_altar.building_name;
        district_i_disp.text = city.district_i.building_name;
        creatures_i_1_disp.text = city.creatures_i_1.building_name;
        creatures_i_2_disp.text = city.creatures_i_2.building_name;
        basic_resources_i_disp.text = city.basic_resources_i.building_name;
        district_ii_disp.text = city.district_ii.building_name;
        creatures_ii_1_disp.text = city.creatures_ii_1.building_name;
        creatures_ii_2_disp.text = city.creatures_ii_2.building_name;
        basic_resources_ii_disp.text = city.basic_resources_ii.building_name;
        district_iii_disp.text = city.district_iii.building_name;
        creatures_iii_1_disp.text = city.creatures_iii_1.building_name;
        creatures_iii_2_disp.text = city.creatures_iii_2.building_name;
        special_resources_i_disp.text = city.special_resources_i.building_name;
        district_iv_disp.text = city.district_iv.building_name;
        creatures_iv_1_disp.text = city.creatures_iv_1.building_name;
        creatures_iv_2_disp.text = city.creatures_iv_2.building_name;
        special_resources_ii_disp.text = city.special_resources_ii.building_name;
        faction_special_disp.text = city.faction_special.building_name;
        fort_disp.text = city.fort2.building_name;
        citadel_disp.text = city.citadel2.building_name;
        stronghold_disp.text = city.stronghold2.building_name;

        creature_i_1_disp.text = city.recr_creature_i_1.creature_name;
        creature_i_1_amnt_disp.text = city.creature_i_1_stock.ToString();
        creature_i_2_disp.text = city.recr_creature_i_2.creature_name;
        creature_i_2_amnt_disp.text = city.creature_i_2_stock.ToString();
        creature_ii_1_disp.text = city.recr_creature_ii_1.creature_name;
        creature_ii_1_amnt_disp.text = city.creature_ii_1_stock.ToString();
        creature_ii_2_disp.text = city.recr_creature_ii_2.creature_name;
        creature_ii_2_amnt_disp.text = city.creature_ii_2_stock.ToString();
        creature_iii_1_disp.text = city.recr_creature_iii_1.creature_name;
        creature_iii_1_amnt_disp.text = city.creature_iii_1_stock.ToString();
        creature_iii_2_disp.text = city.recr_creature_iii_2.creature_name;
        creature_iii_2_amnt_disp.text = city.creature_iii_2_stock.ToString();
        if(city.creatures_iv_1.is_built) {
            creature_iv_disp.text = city.recr_creature_iv_1.creature_name;
            creature_iv_amnt_disp.text = city.creature_iv_1_stock.ToString();
        } else {
            creature_iv_disp.text = city.recr_creature_iv_2.creature_name;
            creature_iv_amnt_disp.text = city.creature_iv_2_stock.ToString();
        }
    }
}
