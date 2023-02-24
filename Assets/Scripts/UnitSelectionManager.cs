using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionManager : MonoBehaviour {
    public GameObject current_selection;
    public CameraMovement cam_movement;

    private Keyboard keyboard = Keyboard.current;

    public static UnitSelectionManager Connect() {
        return GameObject
                .FindGameObjectWithTag("UnitSelectionManager")
                .GetComponent<UnitSelectionManager>();
    }
    
    public void ChangeSelection(GameObject sel) {
        if (current_selection != sel) {

            if(current_selection != null) {
                current_selection.GetComponent<HighlightEffect>().Deselect();
            }

            current_selection = sel;
            cam_movement.FocusOn(current_selection, true);
        }
    }

    public void UnsetSelection() {
        current_selection = null;
        cam_movement.FocusOn(null, false);
    }

    public void Update() {
        // Place a torch when T-key is pressed:
        if(keyboard.tKey.wasPressedThisFrame) {
            current_selection.GetComponent<Unit>().PlaceTorch();
        }
    }
}
