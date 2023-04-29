using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitInventorySlot : MonoBehaviour {
    public Image item_image;
    public Text amount_display;

    public void SetItem(Collectable item) {
        item_image.sprite = item.item_img;
        amount_display.text = item.quantity.ToString();
    }

    public void Clear() {
        item_image.sprite = null;
        amount_display.text = "";
    }
}
