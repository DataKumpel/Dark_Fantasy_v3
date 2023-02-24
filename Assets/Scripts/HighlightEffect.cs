using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HighlightEffect : MonoBehaviour {
    public Color highlight_color = Color.yellow;
    public Color highlight_enemy_color = Color.red;

    [Header("Marker Options")]
    public GameObject marker;
    public Material marker_hover_mat;
    public Material marker_select_mat;
    
    [HideInInspector] public bool is_highlighted = false;
    [HideInInspector] public bool is_selected = false;

    private int num_materials;
    private Color[] original_colors;
    private MeshRenderer rend;
    private MeshRenderer marker_rend;
    private Keyboard keyboard;
    private Mouse mouse;
    private UnitSelectionManager selection_manager;
    private RoundManager round_manager;


    public void Start() {
        rend = GetComponent<MeshRenderer>();
        num_materials = rend.materials.Length;
        original_colors = new Color[num_materials];
        for(int i = 0; i < num_materials; i++) {
            original_colors[i] = rend.materials[i].color;
        }
        mouse = Mouse.current;
        keyboard= Keyboard.current;
        marker_rend = marker.GetComponent<MeshRenderer>();
        marker_rend.enabled= false;
        
        // Connect to selection manager:
        selection_manager = UnitSelectionManager.Connect();
        
        // Connect to round manager:
        round_manager = RoundManager.Connect();
    }


    public void OnMouseEnter() {
        for(int i = 0; i < num_materials; i++) {
            if(GetComponent<Unit>().owner == round_manager.current_player) {
                rend.materials[i].color = highlight_color;
            } else {
                rend.materials[i].color = highlight_enemy_color;
            }
        }
        marker_rend.material = marker_hover_mat;
        marker_rend.enabled= true;
        is_highlighted = true;
    }


    public void OnMouseExit() {
        for(int i = 0; i < num_materials; i++) {
            rend.materials[i].color = original_colors[i];
        }
        is_highlighted = false;
        
        if(!is_selected) {
            marker_rend.enabled= false;
        } else {
            marker_rend.material = marker_select_mat;
        }
    }


    public void Select() {
        // Don't select a unit if there is already one marked:
        if(selection_manager.current_selection != null) return;

        // Also don't select a unit if it doesnt belong to the current player:
        if(GetComponent<Unit>().owner != round_manager.current_player) return;

        selection_manager.ChangeSelection(gameObject);
        marker_rend.material = marker_select_mat;
        is_selected = true;
    }


    public void Deselect() {
        selection_manager.UnsetSelection();
        marker_rend.enabled = false;
        is_selected = false;
    }


    public void Update() {
        if(is_highlighted && mouse.leftButton.wasPressedThisFrame) {
            Select();
        }

        if(is_selected && (mouse.rightButton.wasPressedThisFrame || 
                           keyboard.escapeKey.wasPressedThisFrame)) {
            Deselect();
        }
    }
}
