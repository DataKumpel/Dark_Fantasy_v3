using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICampaign : MonoBehaviour{
    public string scene = string.Empty;

    public void OnClick() {
        // TODO: Add fade effect to black out of the menue...
        //       Also add a fade in effect on the game scene...
        SceneManager.LoadScene(scene);
    }
}
