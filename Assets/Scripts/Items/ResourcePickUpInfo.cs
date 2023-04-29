using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePickUpInfo : MonoBehaviour {
    public Text ui_text;
    public Image ui_image;
    
    public void SetTextAndImage(string text, Sprite img) {
        ui_text.text = $"+{text}";
        ui_image.sprite = img;
    }
    
    public void DestroyOnEnd() {
        Destroy(gameObject);
    }
}
