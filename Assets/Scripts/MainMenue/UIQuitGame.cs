using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuitGame : MonoBehaviour {
    public void OnClick() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
